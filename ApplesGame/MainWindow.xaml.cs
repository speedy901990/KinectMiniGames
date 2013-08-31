using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Samples.Kinect.WpfViewers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ApplesGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int windowHeight = 1080;
        private int windowWidth = 1920;

        private int treesCount = 3;
        private int applesOnTree = 10;
        bool GripOverButton = false;
        private KinectSensorChooser sensorChooser;
        private Apple[,] myApple;

        public static readonly DependencyProperty KinectSensorManagerProperty =
            DependencyProperty.Register(
                "KinectSensorManager",
                typeof(KinectSensorManager),
                typeof(MainWindow),
                new PropertyMetadata(null));

        public MainWindow()
        {
            this.InitializeComponent();

            // initialize the sensor chooser and UI
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUI.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();

            // Bind the sensor chooser's current sensor to the KinectRegion
            var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

            //generating trees
            Canvas[] tree = new Canvas[treesCount];
            myApple = new Apple[treesCount, applesOnTree];
            ImageBrush treeBg = new ImageBrush();
            treeBg.ImageSource = new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/tree.png", UriKind.Relative));
            for (int i = 0; i < treesCount; i++)
            {
                tree[i] = new Canvas();
                tree[i].Width = (windowWidth - 300) / treesCount;
                tree[i].Height = 1000;
                Canvas.SetLeft(tree[i], (i * tree[i].Width + 50));
                tree[i].Name = "tree" + i;
                playfield.Children.Add(tree[i]);
                tree[i].Background = treeBg;
                for (int j = 0; j < applesOnTree; j++)
                {
                    //add apple (minX,maxX,minY,maxY)
                    myApple[i, j] = new Apple(50, (int)(tree[i].Width) - 80,
                        80, (int)(tree[i].Height) - 400);
                    tree[i].Children.Add(myApple[i, j].Figure);

                    //add button
                    var button = new KinectCircleButton
                    {
                        Height = myApple[i, j].Figure.Height,
                        Width = myApple[i, j].Figure.Width,
                        Margin = myApple[i, j].Figure.Margin,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Content = Convert.ToString(i) + Convert.ToString(j),
                        //Visibility=Visibility.Hidden
                    };
                    KinectRegion.AddQueryInteractionStatusHandler(button, OnQuery);
                    KinectRegion.AddHandPointerGripHandler(button, OnHandPointerGrip);
                    tree[i].Children.Add(button);

                }
            }


        }

        public MainWindow(ApplesGameConfig config)
        {
            treesCount = config.TreesCount;
            applesOnTree = config.ApplesOnTreeCount;
            this.InitializeComponent();

            // initialize the sensor chooser and UI
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUI.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();

            // Bind the sensor chooser's current sensor to the KinectRegion
            var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);

            //generating trees
            Canvas[] tree = new Canvas[treesCount];
            Apple[,] myApple = new Apple[treesCount, applesOnTree];
            ImageBrush treeBg = new ImageBrush();
            treeBg.ImageSource = new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/tree.png", UriKind.Relative));
            for (int i = 0; i < treesCount; i++)
            {
                tree[i] = new Canvas();
                tree[i].Width = (windowWidth - 300) / treesCount;
                tree[i].Height = 1000;
                Canvas.SetLeft(tree[i], (i * tree[i].Width + 50));
                tree[i].Name = "tree" + i;
                playfield.Children.Add(tree[i]);
                tree[i].Background = treeBg;
                for (int j = 0; j < applesOnTree; j++)
                {
                    //add apple (minX,maxX,minY,maxY)
                    myApple[i, j] = new Apple(50, (int)(tree[i].Width) - 80,
                        80, (int)(tree[i].Height) - 400);
                    tree[i].Children.Add(myApple[i, j].Figure);

                    //add button
                    var button = new KinectCircleButton
                    {
                        Height = myApple[i, j].Figure.Height,
                        Width = myApple[i, j].Figure.Width,
                        Margin = myApple[i, j].Figure.Margin,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Content = Convert.ToString(i) + Convert.ToString(j),
                        //Visibility=Visibility.Hidden
                    };
                    KinectRegion.AddQueryInteractionStatusHandler(button, OnQuery);
                    KinectRegion.AddHandPointerGripHandler(button, OnHandPointerGrip);
                    tree[i].Children.Add(button);
                }
            }

        }

        //private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        //{
        //    this.sensorChooser = new KinectSensorChooser();
        //    this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
        //    this.sensorChooserUI.KinectSensorChooser = this.sensorChooser;
        //    this.sensorChooser.Start();
        //    var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
        //    BindingOperations.SetBinding(this.kinectRegion, Microsoft.Kinect.Toolkit.Controls.KinectRegion.KinectSensorProperty, regionSensorBinding);
        //}

        #region Kinect discovery + setup
        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }

        public KinectSensorManager KinectSensorManager
        {
            get { return (KinectSensorManager)GetValue(KinectSensorManagerProperty); }
            set { SetValue(KinectSensorManagerProperty, value); }
        }
        #endregion Kinect discovery + setup

        #region Closing window
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sensorChooser.Stop();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            KinectSensorManager.KinectSensor = null;
        }
        #endregion

        private void ApplesGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        private void OnHandPointerGrip(object sender, HandPointerEventArgs handPointerEventArgs)
        {
            
            var button = sender as KinectCircleButton;
            if (handPointerEventArgs.HandPointer.IsInGripInteraction == true)
            {
                GripOverButton = true;
                KinectRegion.AddQueryInteractionStatusHandler(kinectRegion, OnQuery);
                KinectRegion.AddHandPointerGripReleaseHandler(kinectRegion, OnHandPointerGripRelase);

                var Tree = button.Parent as Canvas;
                Tree.Children.Remove(button);
                String ButtonContent = Convert.ToString(button.Content);
                int TreeNumber = Convert.ToInt32(ButtonContent.Substring(0,1));
                int AppleNumber = Convert.ToInt32(ButtonContent.Substring(1,ButtonContent.Length-1));
                Tree.Children.Remove(myApple[TreeNumber,AppleNumber].Figure);
                handPointerEventArgs.Handled = true;
            }
        }

        private void OnHandPointerGripRelase(object sender, HandPointerEventArgs handPointerEventArgs)
        {
            if (handPointerEventArgs.HandPointer.IsInGripInteraction == false && GripOverButton == true)
            {
                var point = handPointerEventArgs.HandPointer.GetPosition(playfield);

                int x = Convert.ToInt32(point.X);
                int y = Convert.ToInt32(point.Y);

                Apple FallingApple = new Apple(x, x, y, y);
                playfield.Children.Add(FallingApple.Figure);
                handPointerEventArgs.Handled = true;
            }
            GripOverButton = false;
        }

        //Variable to track GripInterationStatus
        bool isGripinInteraction = false;
        private void OnQuery(object sender, QueryInteractionStatusEventArgs handPointerEventArgs)
        {

            //If a grip detected change the cursor image to grip
            if (handPointerEventArgs.HandPointer.HandEventType == HandEventType.Grip)
            {
                isGripinInteraction = true;
                handPointerEventArgs.IsInGripInteraction = true;
            }

           //If Grip Release detected change the cursor image to open
            else if (handPointerEventArgs.HandPointer.HandEventType == HandEventType.GripRelease)
            {
                isGripinInteraction = false;
                handPointerEventArgs.IsInGripInteraction = false;
            }

            //If no change in state do not change the cursor
            else if (handPointerEventArgs.HandPointer.HandEventType == HandEventType.None)
            {
                handPointerEventArgs.IsInGripInteraction = isGripinInteraction;
            }

            handPointerEventArgs.Handled = true;
        }
    }
}
