using System.Drawing;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using BubblesGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Forms;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Samples.Kinect.WpfViewers;
using BubblesGame.Utils;
using System.Windows.Media.Imaging;
using System.Resources;
using Binding = System.Windows.Data.Binding;
using Color = System.Windows.Media.Color;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using Point = System.Windows.Point;

namespace BubblesGame
{
    public partial class GameWindow : Window
    {
        #region Public State
        public static readonly DependencyProperty KinectSensorManagerProperty =
            DependencyProperty.Register(
                "KinectSensorManager",
                typeof(KinectSensorManager),
                typeof(GameWindow),
                new PropertyMetadata(null));
        #endregion

        #region Private State

        private const int TimerResolution = 2;  // ms
        private const int NumIntraFrames = 3;
        private const double MaxFramerate = 70;
        private const double MinFramerate = 15;
        private const double MinShapeSize = 12;
        private const double MaxShapeSize = 90;
        private const double DefaultDropRate = 1;
        private const double DefaultDropSize = 64.0;
        private const double DefaultDropGravity = 1.0;
        private static int _maxShapes = 80;

        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        private readonly SoundPlayer _popSound = new SoundPlayer();
        private readonly SoundPlayer _hitSound = new SoundPlayer();
        private readonly SoundPlayer _squeezeSound = new SoundPlayer();
        private readonly KinectSensorChooser _sensorChooser = new KinectSensorChooser();

        private double _dropRate = DefaultDropRate;
        private double _dropSize = DefaultDropSize;
        private double _dropGravity = DefaultDropGravity;
        private DateTime _lastFrameDrawn = DateTime.MinValue;
        private DateTime _predNextFrame = DateTime.MinValue;
        private double _actualFrameTime;

        private Skeleton[] _skeletonData;

        // Player(s) placement in scene (z collapsed):
        private Rect _playerBounds;
        private Rect _screenRect;

        private double _targetFramerate = MaxFramerate;
        private int _frameCount;
        private bool _runningGameThread;
        private FallingThings _myFallingThings;

        private BubblesGameConfig config;
        #endregion Private State

        #region ctor + Window Events

        public GameWindow()
        {
            InitializeComponent();
            SetupKinectSensor();
            RestoreWindowState();
        }

        public GameWindow(BubblesGameConfig config)
        {
            InitializeComponent();
            this.config = config;
            SetupKinectSensor(config);
            RestoreWindowState();
        }

        private void SetupKinectSensor(BubblesGameConfig config = null)
        {
            KinectSensorManager = new KinectSensorManager();
            KinectSensorManager.KinectSensorChanged += KinectSensorChanged;
            DataContext = KinectSensorManager;

            if (config == null)
            {
                SensorChooserUI.KinectSensorChooser = _sensorChooser;
                _sensorChooser.Start();
                var kinectSensorBinding = new Binding("Kinect") { Source = _sensorChooser };
                BindingOperations.SetBinding(KinectSensorManager, KinectSensorManager.KinectSensorProperty, kinectSensorBinding);
            }
            else
            {
                SensorChooserUI.KinectSensorChooser = config.PassedKinectSensorChooser;
                _sensorChooser.Start();
                var kinectSensorBinding = new Binding("Kinect") { Source = config.PassedKinectSensorChooser };
                BindingOperations.SetBinding(KinectSensorManager, KinectSensorManager.KinectSensorProperty, kinectSensorBinding);
            }
        }

        private void SetBackground()
        {
            var bg = new ImageBrush(convertBitmapToBitmapSource(Properties.Resources.ApplesGameBackground))
            {
                Stretch = Stretch.UniformToFill
            };
            grid.Background = bg;
        }

        private BitmapSource convertBitmapToBitmapSource(Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }

        private void SetupConfiguration()
        {
            _dropSize = config.BubblesSize;
            _maxShapes = config.BubblesCount;
            _dropGravity = config.BubblesFallSpeed * 0.2;
            _dropRate = config.BubblesApperanceFrequency * 0.4;

            _myFallingThings = new FallingThings(_maxShapes, _targetFramerate, NumIntraFrames, _maxShapes);

            UpdatePlayfieldSize();

            _myFallingThings.SetGravity(_dropGravity);
            _myFallingThings.SetDropRate(_dropRate);
            _myFallingThings.SetSize(_dropSize);
            _myFallingThings.SetPolies(PolyType.Circle);

            _popSound.Stream = Properties.Resources.Pop_5;
            _hitSound.Stream = Properties.Resources.Hit_2;
            _squeezeSound.Stream = Properties.Resources.Squeeze;

            _popSound.Play();

            TimeBeginPeriod(TimerResolution);
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

        private void WindowsRendered(object sender, EventArgs e)
        {
            var myGameThread = new Thread(GameThread);
            myGameThread.SetApartmentState(ApartmentState.STA);

            myGameThread.Start();
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
            playfield.ClipToBounds = true;
            SetBackground();
            SetupConfiguration();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            _sensorChooser.Stop();

            _runningGameThread = false;
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

        }

        // Kinect enabled apps should uninitialize all Kinect services that were initialized in InitializeKinectServices() here.
        private void UninitializeKinectServices(KinectSensor sensor)
        {
            sensor.SkeletonFrameReady -= SkeletonsReady;

            //enableAec.Visibility = Visibility.Collapsed;
        }

        #endregion Kinect discovery + setup

        #region Kinect Skeleton processing
        private static Skeleton GetPrimarySkeleton(Skeleton[] skeletons)
        {
            Skeleton skeleton = null;

            if (skeletons != null)
            {
                //Find the closest skeleton
                for (int i = 0; i < skeletons.Length; i++)
                {
                    if (skeletons[i].TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (skeleton == null)
                        {
                            skeleton = skeletons[i];
                        }
                        else
                        {
                            if (skeleton.Position.Z > skeletons[i].Position.Z)
                            {
                                skeleton = skeletons[i];
                            }
                        }
                    }
                }
            }

            return skeleton;
        }
        private void SkeletonsReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    int skeletonSlot = 0;

                    if ((_skeletonData == null) || (_skeletonData.Length != skeletonFrame.SkeletonArrayLength))
                    {
                        _skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    skeletonFrame.CopySkeletonDataTo(_skeletonData);

                    //foreach (Skeleton skeleton in skeletonData)
                    //{
                    Skeleton skeleton = GetPrimarySkeleton(_skeletonData);
                    if (skeleton != null)
                    {
                        if (SkeletonTrackingState.Tracked == skeleton.TrackingState)
                        {
                            Player player;
                            if (_players.ContainsKey(skeletonSlot))
                            {
                                player = _players[skeletonSlot];
                            }
                            else
                            {
                                player = new Player(skeletonSlot);
                                player.SetBounds(_playerBounds);
                                _players.Add(skeletonSlot, player);
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
                                player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderLeft,
                                    JointType.ShoulderCenter);
                                player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderCenter,
                                    JointType.ShoulderRight);

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
                    //}
                }
            }
        }

        private void PlayfieldSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePlayfieldSize();
        }

        private void UpdatePlayfieldSize()
        {
            // Size of player wrt size of playfield, putting ourselves low on the screen.
            _screenRect.X = 0;
            _screenRect.Y = 0;
            _screenRect.Width = playfield.ActualWidth;
            _screenRect.Height = playfield.ActualHeight;

            //BannerText.UpdateBounds(screenRect);

            _playerBounds.X = 0;
            _playerBounds.Width = playfield.ActualWidth;
            _playerBounds.Y = playfield.ActualHeight * 0.2;
            _playerBounds.Height = playfield.ActualHeight * 0.75;

            foreach (var player in _players)
            {
                player.Value.SetBounds(_playerBounds);
            }

            Rect fallingBounds = _playerBounds;
            fallingBounds.Y = 0;
            fallingBounds.Height = playfield.ActualHeight;
            if (_myFallingThings != null)
            {
                _myFallingThings.SetBoundaries(fallingBounds);
            }
        }
        #endregion Kinect Skeleton processing

        #region GameTimer/Thread
        private void GameThread()
        {
            _runningGameThread = true;
            _predNextFrame = DateTime.Now;
            _actualFrameTime = 1000.0 / _targetFramerate;

            // Try to dispatch at as constant of a framerate as possible by sleeping just enough since
            // the last time we dispatched.
            while (_runningGameThread)
            {
                // Calculate average framerate.  
                DateTime now = DateTime.Now;
                if (_lastFrameDrawn == DateTime.MinValue)
                {
                    _lastFrameDrawn = now;
                }

                double ms = now.Subtract(_lastFrameDrawn).TotalMilliseconds;
                _actualFrameTime = (_actualFrameTime * 0.95) + (0.05 * ms);
                _lastFrameDrawn = now;

                // Adjust target framerate down if we're not achieving that rate
                _frameCount++;
                if ((_frameCount % 100 == 0) && (1000.0 / _actualFrameTime < _targetFramerate * 0.92))
                {
                    _targetFramerate = Math.Max(MinFramerate, (_targetFramerate + (1000.0 / _actualFrameTime)) / 2);
                }

                if (now > _predNextFrame)
                {
                    _predNextFrame = now;
                }
                else
                {
                    double milliseconds = _predNextFrame.Subtract(now).TotalMilliseconds;
                    if (milliseconds >= TimerResolution)
                    {
                        Thread.Sleep((int)(milliseconds + 0.5));
                    }
                }

                _predNextFrame += TimeSpan.FromMilliseconds(1000.0 / _targetFramerate);

                Dispatcher.Invoke(DispatcherPriority.Send, new Action<int>(HandleGameTimer), 0);
            }
        }

        private void HandleGameTimer(int param)
        {
            // Every so often, notify what our actual framerate is
            if ((_frameCount % 100) == 0)
            {
                _myFallingThings.SetFramerate(1000.0 / _actualFrameTime);
            }

            // Advance animations, and do hit testing.
            for (int i = 0; i < NumIntraFrames; ++i)
            {
                foreach (var pair in _players)
                {
                    HitType hit = _myFallingThings.LookForHits(pair.Value.Segments, pair.Value.GetId());
                    if ((hit & HitType.Squeezed) != 0)
                    {
                        _squeezeSound.Play();
                    }
                    else if ((hit & HitType.Popped) != 0)
                    {
                        _popSound.Play();
                    }
                    else if ((hit & HitType.Hand) != 0)
                    {
                        _hitSound.Play();
                    }
                }

                _myFallingThings.AdvanceFrame();
            }

            // Draw new Wpf scene by adding all objects to canvas
            playfield.Children.Clear();
            _myFallingThings.DrawFrame(playfield.Children);
            if (_myFallingThings._things.Count == 0 && FallingThings.BubblesFallen > 0)
            {
                var result = MessageBox.Show("Gratulacje! Twój wynik to: " + FallingThings.BubblesPopped, "", MessageBoxButton.OK);
                if (result == MessageBoxResult.OK)
                {
                    this.exitGame();
                    //FallingThings.BubblesFallen = 0;
                    //FallingThings.BubblesPopped = 0;
                    //Close();
                }
            }

            /*foreach (var player in players)
            {
                player.Value.Draw(playfield.Children);
            }*/
            if (_players.FirstOrDefault().Value != null)
                _players.FirstOrDefault().Value.Draw(playfield.Children);
            BannerText.NewBanner(FallingThings.BubblesPopped.ToString(), _screenRect, false, Color.FromRgb(0, 0, 0));
            BannerText.Draw(playfield.Children);
            //FlyingText.Draw(playfield.Children);

            //CheckPlayers();
        }
        #endregion GameTimer/Thread

        #region Closing window
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.exitGame();
            }
        }
        private void exitGame()
        {
            FallingThings.BubblesFallen = 0;
            FallingThings.BubblesPopped = 0;
            //this.stopKinect();
            this.Close();
        }
        private void stopKinect()
        {
            try
            {
                this._sensorChooser.Kinect.Stop();
                this._sensorChooser.Kinect.AudioSource.Stop();
            }
            catch (NullReferenceException)
            {
                //throw;
            }
            this._sensorChooser.Stop();
        }
        #endregion
    }
}
