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
using System.Windows.Media.Animation;

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
        private int appleSize = 150;
        private int basketCount = 3;
        private bool GripOverButton = false;
        private Apple[] myApple;
        private Basket[] basket;
        private Apple GripApple = new Apple(new Point(0,0), new Point(0,0),150,0,0);
        private int Treenum;

        private KinectSensorChooser sensorChooser;

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

            runGame();
        }

        public MainWindow(ApplesGameConfig config)
        {
            treesCount = config.TreesCount;
            applesOnTree = config.ApplesOnTreeCount;
            basketCount = config.TreesCount;

            this.InitializeComponent();

            // initialize the sensor chooser and UI
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUI.KinectSensorChooser = config.PassedKinectSensorChooser;
            this.sensorChooser.Start();

            // Bind the sensor chooser's current sensor to the KinectRegion
            var regionSensorBinding = new Binding("Kinect") { Source = config.PassedKinectSensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
            
            runGame();
        }

        private void runGame()
        {
            Point rangeMin = new Point();
            Point rangeMax = new Point();

            //generating trees
            Canvas[] tree = new Canvas[treesCount];
            myApple = new Apple[treesCount * applesOnTree];
            ImageBrush treeBg = new ImageBrush();
            treeBg.ImageSource = new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/tree.png", UriKind.Relative));
            
            basket = new Basket[basketCount];
            for (int i = 0; i < treesCount; i++)
            {
                tree[i] = new Canvas();
                tree[i].Width = (windowWidth - 300) / treesCount;
                tree[i].Height = 800;
                Canvas.SetTop(tree[i], 50);
                Canvas.SetLeft(tree[i], (i * tree[i].Width + 50));
                tree[i].Name = "tree" + i;
                playfield.Children.Add(tree[i]);
                tree[i].Background = treeBg;

                rangeMin.X = 50.0;
                rangeMin.Y = 80.0;
                rangeMax.X = (double)(tree[i].Width) - 80;
                rangeMax.Y = (double)(tree[i].Height) - 400;
                for (int j = i*applesOnTree; j < applesOnTree * (i+1); j++)
                {
                    myApple[j] = new Apple(rangeMin, rangeMax, appleSize, j, i);
                    
                    var button = myApple[j].Figure;
                    KinectRegion.AddQueryInteractionStatusHandler(button, OnQuery);
                    KinectRegion.AddHandPointerGripHandler(button, OnHandPointerGrip);
                    tree[i].Children.Add(button);
                }
            }
            for (int i = 0; i < basketCount; i++)
            {
                double x = (windowWidth / basketCount) * i;
                double y = windowHeight - 400;
                Point basketPosition = new Point((int)x, (int)y);
                System.Random rand = new Random(Guid.NewGuid().GetHashCode());
                int basketColor = rand.Next(1,4);
                if (i != 0)
                {
                    bool change;
                    for ( int j = 0 ; j < i ;)
                    {
                        change = false;
                        if (basketColor == basket[j].ColorNumber)
                        {
                            if (basketColor == 3)
                            {
                                basketColor--;
                            }
                            else
                            {
                                basketColor++;
                            }
                            change = true;
                        }
                        if (change)
                            j = 0;
                        else
                            j++;
                    }
                }
                basket[i] = new Basket(400, 400, basketPosition, basketColor);
                playfield.Children.Add(basket[i].Figure);
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
                int buttonContent;
                GripOverButton = true;
                KinectRegion.AddQueryInteractionStatusHandler(kinectRegion, OnQuery);
                KinectRegion.AddHandPointerGripReleaseHandler(kinectRegion, OnHandPointerGripRelase);

                var Tree = button.Parent as Canvas;
                Tree.Children.Remove(button);
                buttonContent = (int)button.Content;
                GripApple = myApple[buttonContent];
                Tree.Children.Remove(myApple[buttonContent].Figure);
                Treenum = GripApple.TreeNumber;
                handPointerEventArgs.Handled = true;
            }
        }
        private void OnHandPointerGripRelase(object sender, HandPointerEventArgs handPointerEventArgs)
        {
            if (handPointerEventArgs.HandPointer.IsInGripInteraction == false && GripOverButton == true)
            {
                bool check = false;
                Point point = handPointerEventArgs.HandPointer.GetPosition(playfield);
                for (int i = 0; i < basketCount; i++)
                 {
                     if ((point.X >= basket[i].Position.X)
                         && (point.X <= basket[i].EndPosition.X)
                         && (point.Y >= basket[i].Position.Y)
                         && (point.Y <= basket[i].EndPosition.Y)
                         && GripApple.Color == basket[i].Color)
                     {
                         handPointerEventArgs.Handled = true;
                         check = true;
                     }
                 }
                
                if (!check)
                {
                    Apple MovingApple = new Apple(GripApple, point.X, point.Y);
                    playfield.Children.Add(MovingApple.Figure);
                    MoveTo(MovingApple, GripApple.Pos.X, GripApple.Pos.Y, point.X, point.Y);
                    KinectRegion.AddQueryInteractionStatusHandler(MovingApple.Figure, OnQuery);
                    KinectRegion.AddHandPointerGripHandler(MovingApple.Figure, OnHandPointerGrip);
                    handPointerEventArgs.Handled = true;
                }
            }
            GripOverButton = false;
        }

        public void MoveTo(Apple target, double NewX, double NewY, double HandX, double HandY)
        {
            TranslateTransform trans = new TranslateTransform();
            target.Figure.RenderTransform = trans;
            //tree[] Canvas and [0,0] differences
            NewX += ((windowWidth - 300) / treesCount) * Treenum;
            NewY += (windowHeight - 1000);



            //ActualHand and [0,0] differences
            NewX -= HandX;
            NewY -= HandY;

            NewY -= target.Size;
            NewX += target.Size / 2;
            DoubleAnimation anim1 = new DoubleAnimation(0, NewX, TimeSpan.FromSeconds(1));
            DoubleAnimation anim2 = new DoubleAnimation(0, NewY, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
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
