//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

// This module contains code to do Kinect NUI initialization,
// processing, displaying players on screen, and sending updated player
// positions to the game portion for hit testing.

using System.Windows.Media;
using ShapeGame.Properties;

namespace ShapeGame
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Media;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Threading;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Toolkit;
    using Microsoft.Samples.Kinect.WpfViewers;
    using Speech;
    using Utils;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static readonly DependencyProperty KinectSensorManagerProperty =
            DependencyProperty.Register(
                "KinectSensorManager",
                typeof(KinectSensorManager),
                typeof(MainWindow),
                new PropertyMetadata(null));

        #region Private State
        private const int TimerResolution = 2;  // ms
        private const int NumIntraFrames = 3;
        private const int MaxShapes = 80;
        private const double MaxFramerate = 70;
        private const double MinFramerate = 15;
        private const double MinShapeSize = 12;
        private const double MaxShapeSize = 90;
        private const double DefaultDropRate = 7;
        private const double DefaultDropSize = 64.0;
        private const double DefaultDropGravity = 1.0;

        private readonly Dictionary<int, Player> players = new Dictionary<int, Player>();
        private readonly SoundPlayer popSound = new SoundPlayer();
        private readonly SoundPlayer hitSound = new SoundPlayer();
        private readonly SoundPlayer squeezeSound = new SoundPlayer();
        private readonly KinectSensorChooser sensorChooser = new KinectSensorChooser();

        private double dropRate = DefaultDropRate;
        private double dropSize = DefaultDropSize;
        private double dropGravity = DefaultDropGravity;
        private DateTime lastFrameDrawn = DateTime.MinValue;
        private DateTime predNextFrame = DateTime.MinValue;
        private double actualFrameTime;

        private Skeleton[] skeletonData;

        // Player(s) placement in scene (z collapsed):
        private Rect playerBounds;
        private Rect screenRect;

        private double targetFramerate = MaxFramerate;
        private int frameCount;
        private bool runningGameThread;
        private FallingThings myFallingThings;
        
//        private SpeechRecognizer mySpeechRecognizer;
        #endregion Private State

        #region ctor + Window Events

        public MainWindow()
        {
            KinectSensorManager = new KinectSensorManager();
            KinectSensorManager.KinectSensorChanged += KinectSensorChanged;
            DataContext = KinectSensorManager;

            InitializeComponent();

            SensorChooserUI.KinectSensorChooser = sensorChooser;
            sensorChooser.Start();

            // Bind the KinectSensor from the sensorChooser to the KinectSensor on the KinectSensorManager
            var kinectSensorBinding = new Binding("Kinect") { Source = sensorChooser };
            BindingOperations.SetBinding(KinectSensorManager, KinectSensorManager.KinectSensorProperty, kinectSensorBinding);

            RestoreWindowState();
        }

        public KinectSensorManager KinectSensorManager
        {
            get { return (KinectSensorManager)GetValue(KinectSensorManagerProperty); }
            set { SetValue(KinectSensorManagerProperty, value); }
        }

        // Since the timer resolution defaults to about 10ms precisely, we need to
        // increase the resolution to get framerates above between 50fps with any
        // consistency.
        [DllImport("Winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern int TimeBeginPeriod(uint period);

        private void RestoreWindowState()
        {
            // Restore window state to that last used
            Rect bounds = Settings.Default.PrevWinPosition;
            if (bounds.Right != bounds.Left)
            {
                Top = bounds.Top;
                Left = bounds.Left;
                Height = bounds.Height;
                Width = bounds.Width;
            }

            WindowState = (WindowState)Settings.Default.WindowState;
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
            playfield.ClipToBounds = true;

            myFallingThings = new FallingThings(MaxShapes, targetFramerate, NumIntraFrames);

            UpdatePlayfieldSize();

            myFallingThings.SetGravity(dropGravity);
            myFallingThings.SetDropRate(dropRate);
            myFallingThings.SetSize(dropSize);
            myFallingThings.SetPolies(PolyType.Circle);
            
            popSound.Stream = Properties.Resources.Pop_5;
            hitSound.Stream = Properties.Resources.Hit_2;
            squeezeSound.Stream = Properties.Resources.Squeeze;

            popSound.Play();

            TimeBeginPeriod(TimerResolution);
            var myGameThread = new Thread(GameThread);
            myGameThread.SetApartmentState(ApartmentState.STA);
            myGameThread.Start();

      }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            sensorChooser.Stop();

            runningGameThread = false;
            Settings.Default.PrevWinPosition = RestoreBounds;
            Settings.Default.WindowState = (int)WindowState;
            Settings.Default.Save();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            KinectSensorManager.KinectSensor = null;
        }

        #endregion ctor + Window Events

        #region Kinect discovery + setup

        private void KinectSensorChanged(object sender, KinectSensorManagerEventArgs<KinectSensor> args)
        {
            if (null != args.OldValue)
            {
                UninitializeKinectServices(args.OldValue);
            }

            // Only enable this checkbox if we have a sensor
            //enableAec.IsEnabled = null != args.NewValue;

            if (null != args.NewValue)
            {
                InitializeKinectServices(KinectSensorManager, args.NewValue);
            }
        }

        // Kinect enabled apps should customize which Kinect services it initializes here.
        private void InitializeKinectServices(KinectSensorManager kinectSensorManager, KinectSensor sensor)
        {
            // Application should enable all streams first.
            kinectSensorManager.ColorFormat = ColorImageFormat.RgbResolution640x480Fps30;
            kinectSensorManager.ColorStreamEnabled = true;

            sensor.SkeletonFrameReady += SkeletonsReady;
            kinectSensorManager.TransformSmoothParameters = new TransformSmoothParameters
                                             {
                                                 Smoothing = 0.5f,
                                                 Correction = 0.5f,
                                                 Prediction = 0.5f,
                                                 JitterRadius = 0.05f,
                                                 MaxDeviationRadius = 0.04f
                                             };
            kinectSensorManager.SkeletonStreamEnabled = true;
            kinectSensorManager.KinectSensorEnabled = true;

/*
            if (!kinectSensorManager.KinectSensorAppConflict)
            {
                // Start speech recognizer after KinectSensor started successfully.
                mySpeechRecognizer = SpeechRecognizer.Create();

                if (null != mySpeechRecognizer)
                {
                    mySpeechRecognizer.SaidSomething += RecognizerSaidSomething;
                    mySpeechRecognizer.Start(sensor.AudioSource);
                }

                enableAec.Visibility = Visibility.Visible;
                UpdateEchoCancellation(enableAec);
            }
*/
        }

        // Kinect enabled apps should uninitialize all Kinect services that were initialized in InitializeKinectServices() here.
        private void UninitializeKinectServices(KinectSensor sensor)
        {
            sensor.SkeletonFrameReady -= SkeletonsReady;

/*
            if (null != mySpeechRecognizer)
            {
                mySpeechRecognizer.Stop();
                mySpeechRecognizer.SaidSomething -= RecognizerSaidSomething;
                mySpeechRecognizer.Dispose();
                mySpeechRecognizer = null;
            }
*/

            //enableAec.Visibility = Visibility.Collapsed;
        }

        #endregion Kinect discovery + setup

        #region Kinect Skeleton processing
        private void SkeletonsReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    int skeletonSlot = 0;

                    if ((skeletonData == null) || (skeletonData.Length != skeletonFrame.SkeletonArrayLength))
                    {
                        skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(skeletonData);

                    foreach (Skeleton skeleton in skeletonData)
                    {
                        if (SkeletonTrackingState.Tracked == skeleton.TrackingState)
                        {
                            Player player;
                            if (players.ContainsKey(skeletonSlot))
                            {
                                player = players[skeletonSlot];
                            }
                            else
                            {
                                player = new Player(skeletonSlot);
                                player.SetBounds(playerBounds);
                                players.Add(skeletonSlot, player);
                            }

                            player.LastUpdated = DateTime.Now;

                            // Update player's bone and joint positions
                            if (skeleton.Joints.Count > 0)
                            {
                                player.IsAlive = true;

                                // Head, hands, feet (hit testing happens in order here)
                                player.UpdateJointPosition(skeleton.Joints, JointType.Head);
                                player.UpdateJointPosition(skeleton.Joints, JointType.HandLeft);
                                player.UpdateJointPosition(skeleton.Joints, JointType.HandRight);
                                player.UpdateJointPosition(skeleton.Joints, JointType.FootLeft);
                                player.UpdateJointPosition(skeleton.Joints, JointType.FootRight);

                                // Hands and arms
                                player.UpdateBonePosition(skeleton.Joints, JointType.HandRight, JointType.WristRight);
                                player.UpdateBonePosition(skeleton.Joints, JointType.WristRight, JointType.ElbowRight);
                                player.UpdateBonePosition(skeleton.Joints, JointType.ElbowRight, JointType.ShoulderRight);

                                player.UpdateBonePosition(skeleton.Joints, JointType.HandLeft, JointType.WristLeft);
                                player.UpdateBonePosition(skeleton.Joints, JointType.WristLeft, JointType.ElbowLeft);
                                player.UpdateBonePosition(skeleton.Joints, JointType.ElbowLeft, JointType.ShoulderLeft);

                                // Head and Shoulders
                                player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderCenter, JointType.Head);
                                player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderLeft, JointType.ShoulderCenter);
                                player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderRight);

                                // Legs
                                player.UpdateBonePosition(skeleton.Joints, JointType.HipLeft, JointType.KneeLeft);
                                player.UpdateBonePosition(skeleton.Joints, JointType.KneeLeft, JointType.AnkleLeft);
                                player.UpdateBonePosition(skeleton.Joints, JointType.AnkleLeft, JointType.FootLeft);

                                player.UpdateBonePosition(skeleton.Joints, JointType.HipRight, JointType.KneeRight);
                                player.UpdateBonePosition(skeleton.Joints, JointType.KneeRight, JointType.AnkleRight);
                                player.UpdateBonePosition(skeleton.Joints, JointType.AnkleRight, JointType.FootRight);

                                player.UpdateBonePosition(skeleton.Joints, JointType.HipLeft, JointType.HipCenter);
                                player.UpdateBonePosition(skeleton.Joints, JointType.HipCenter, JointType.HipRight);

                                // Spine
                                player.UpdateBonePosition(skeleton.Joints, JointType.HipCenter, JointType.ShoulderCenter);
                            }
                        }

                        skeletonSlot++;
                    }
                }
            }
        }

    /*    private void CheckPlayers()
        {
            foreach (var player in players)
            {
                if (!player.Value.IsAlive)
                {
                    // Player left scene since we aren't tracking it anymore, so remove from dictionary
                    players.Remove(player.Value.GetId());
                    break;
                }
            }

            // Count alive players
            int alive = players.Count(player => player.Value.IsAlive);

            if (alive != playersAlive)
            {
                if (alive == 2)
                {
                    myFallingThings.SetGameMode(GameMode.TwoPlayer);
                }
                else if (alive == 1)
                {
                    myFallingThings.SetGameMode(GameMode.Solo);
                }
                else if (alive == 0)
                {
                    myFallingThings.SetGameMode(GameMode.Off);
                }

                if ((playersAlive == 0)) // && (mySpeechRecognizer != null))
                {
                    BannerText.NewBanner(
                        Properties.Resources.Vocabulary,
                        screenRect,
                        true,
                        Color.FromArgb(200, 255, 255, 255));
                }

                playersAlive = alive;
            }
        }
*/
        private void PlayfieldSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePlayfieldSize();
        }

        private void UpdatePlayfieldSize()
        {
            // Size of player wrt size of playfield, putting ourselves low on the screen.
            screenRect.X = 0;
            screenRect.Y = 0;
            screenRect.Width = playfield.ActualWidth;
            screenRect.Height = playfield.ActualHeight;

            //BannerText.UpdateBounds(screenRect);

            playerBounds.X = 0;
            playerBounds.Width = playfield.ActualWidth;
            playerBounds.Y = playfield.ActualHeight * 0.2;
            playerBounds.Height = playfield.ActualHeight * 0.75;

            foreach (var player in players)
            {
                player.Value.SetBounds(playerBounds);
            }

            Rect fallingBounds = playerBounds;
            fallingBounds.Y = 0;
            fallingBounds.Height = playfield.ActualHeight;
            if (myFallingThings != null)
            {
                myFallingThings.SetBoundaries(fallingBounds);
            }
        }
        #endregion Kinect Skeleton processing

        #region GameTimer/Thread
        private void GameThread()
        {
            runningGameThread = true;
            predNextFrame = DateTime.Now;
            actualFrameTime = 1000.0 / targetFramerate;

            // Try to dispatch at as constant of a framerate as possible by sleeping just enough since
            // the last time we dispatched.
            while (runningGameThread)
            {
                // Calculate average framerate.  
                DateTime now = DateTime.Now;
                if (lastFrameDrawn == DateTime.MinValue)
                {
                    lastFrameDrawn = now;
                }

                double ms = now.Subtract(lastFrameDrawn).TotalMilliseconds;
                actualFrameTime = (actualFrameTime * 0.95) + (0.05 * ms);
                lastFrameDrawn = now;

                // Adjust target framerate down if we're not achieving that rate
                frameCount++;
                if ((frameCount % 100 == 0) && (1000.0 / actualFrameTime < targetFramerate * 0.92))
                {
                    targetFramerate = Math.Max(MinFramerate, (targetFramerate + (1000.0 / actualFrameTime)) / 2);
                }

                if (now > predNextFrame)
                {
                    predNextFrame = now;
                }
                else
                {
                    double milliseconds = predNextFrame.Subtract(now).TotalMilliseconds;
                    if (milliseconds >= TimerResolution)
                    {
                        Thread.Sleep((int)(milliseconds + 0.5));
                    }
                }

                predNextFrame += TimeSpan.FromMilliseconds(1000.0 / targetFramerate);

                Dispatcher.Invoke(DispatcherPriority.Send, new Action<int>(HandleGameTimer), 0);
            }
        }

        private void HandleGameTimer(int param)
        {
            // Every so often, notify what our actual framerate is
            if ((frameCount % 100) == 0)
            {
                myFallingThings.SetFramerate(1000.0 / actualFrameTime);
            }

            // Advance animations, and do hit testing.
            for (int i = 0; i < NumIntraFrames; ++i)
            {
                foreach (var pair in players)
                {
                    HitType hit = myFallingThings.LookForHits(pair.Value.Segments, pair.Value.GetId());
                    if ((hit & HitType.Squeezed) != 0)
                    {
                        squeezeSound.Play();
                    }
                    else if ((hit & HitType.Popped) != 0)
                    {
                        popSound.Play();
                    }
                    else if ((hit & HitType.Hand) != 0)
                    {
                        hitSound.Play();
                    }
                }

                myFallingThings.AdvanceFrame();
            }

            // Draw new Wpf scene by adding all objects to canvas
            playfield.Children.Clear();
            myFallingThings.DrawFrame(playfield.Children);
            foreach (var player in players)
            {
                player.Value.Draw(playfield.Children);
            }

            //BannerText.Draw(playfield.Children);
            //FlyingText.Draw(playfield.Children);

            //CheckPlayers();
        }
        #endregion GameTimer/Thread

        #region Kinect Speech processing
      /*  private void RecognizerSaidSomething(object sender, SpeechRecognizer.SaidSomethingEventArgs e)
        {
            FlyingText.NewFlyingText(screenRect.Width / 30, new Point(screenRect.Width / 2, screenRect.Height / 2), e.Matched);
            switch (e.Verb)
            {
                case SpeechRecognizer.Verbs.Pause:
                    myFallingThings.SetDropRate(0);
                    myFallingThings.SetGravity(0);
                    break;
                case SpeechRecognizer.Verbs.Resume:
                    myFallingThings.SetDropRate(dropRate);
                    myFallingThings.SetGravity(dropGravity);
                    break;
                case SpeechRecognizer.Verbs.Reset:
                    dropRate = DefaultDropRate;
                    dropSize = DefaultDropSize;
                    dropGravity = DefaultDropGravity;
                    myFallingThings.SetPolies(PolyType.All);
                    myFallingThings.SetDropRate(dropRate);
                    myFallingThings.SetGravity(dropGravity);
                    myFallingThings.SetSize(dropSize);
                    myFallingThings.SetShapesColor(Color.FromRgb(0, 0, 0), true);
                    myFallingThings.Reset();
                    break;
                case SpeechRecognizer.Verbs.DoShapes:
                    myFallingThings.SetPolies(e.Shape);
                    break;
                case SpeechRecognizer.Verbs.RandomColors:
                    myFallingThings.SetShapesColor(Color.FromRgb(0, 0, 0), true);
                    break;
                case SpeechRecognizer.Verbs.Colorize:
                    myFallingThings.SetShapesColor(e.RgbColor, false);
                    break;
                case SpeechRecognizer.Verbs.ShapesAndColors:
                    myFallingThings.SetPolies(e.Shape);
                    myFallingThings.SetShapesColor(e.RgbColor, false);
                    break;
                case SpeechRecognizer.Verbs.More:
                    dropRate *= 1.5;
                    myFallingThings.SetDropRate(dropRate);
                    break;
                case SpeechRecognizer.Verbs.Fewer:
                    dropRate /= 1.5;
                    myFallingThings.SetDropRate(dropRate);
                    break;
                case SpeechRecognizer.Verbs.Bigger:
                    dropSize *= 1.5;
                    if (dropSize > MaxShapeSize)
                    {
                        dropSize = MaxShapeSize;
                    }

                    myFallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Biggest:
                    dropSize = MaxShapeSize;
                    myFallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Smaller:
                    dropSize /= 1.5;
                    if (dropSize < MinShapeSize)
                    {
                        dropSize = MinShapeSize;
                    }

                    myFallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Smallest:
                    dropSize = MinShapeSize;
                    myFallingThings.SetSize(dropSize);
                    break;
                case SpeechRecognizer.Verbs.Faster:
                    dropGravity *= 1.25;
                    if (dropGravity > 4.0)
                    {
                        dropGravity = 4.0;
                    }

                    myFallingThings.SetGravity(dropGravity);
                    break;
                case SpeechRecognizer.Verbs.Slower:
                    dropGravity /= 1.25;
                    if (dropGravity < 0.25)
                    {
                        dropGravity = 0.25;
                    }

                    myFallingThings.SetGravity(dropGravity);
                    break;
            }
        }
*/
/*
        private void EnableAecChecked(object sender, RoutedEventArgs e)
        {
            var enableAecCheckBox = (CheckBox)sender;
            UpdateEchoCancellation(enableAecCheckBox);
        }
*/

        /*private void UpdateEchoCancellation(CheckBox aecCheckBox)
        {
            mySpeechRecognizer.EchoCancellationMode = aecCheckBox.IsChecked != null && aecCheckBox.IsChecked.Value
                ? EchoCancellationMode.CancellationAndSuppression
                : EchoCancellationMode.None;
        }
*/
        #endregion Kinect Speech processing
    }
}
