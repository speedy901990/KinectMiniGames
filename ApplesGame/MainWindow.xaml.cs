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
    public partial class MainWindow : Window
    {
        #region Public State
        public static readonly DependencyProperty KinectSensorManagerProperty =
            DependencyProperty.Register(
                "KinectSensorManager",
                typeof(KinectSensorManager),
                typeof(MainWindow),
                new PropertyMetadata(null));
        #endregion

        #region Private State
        private double windowHeight = 768;
        private double windowWidth = 1024;
        private int treesCount = 3;
        private int applesOnTree = 5;
        private int appleSize = 150;
        private int colorsCount = 4;
        private int basketCount = 3;
        private bool GripOverButton = false;
        private Apple[] myApple;
        private Basket[] basket;
        private Apple GripApple;
        private int Treenum;
        private Score gameScore;
        private KinectSensorChooser sensorChooser;
        #endregion

        #region Ctor + Config
        public MainWindow()
        {
            this.InitializeComponent();
            setupKinectSensor();
            runGame();
        }

        public MainWindow(ApplesGameConfig config)
        {
            this.InitializeComponent();
            setupConfiguration(config);
            setupKinectSensor(config);
            runGame();
        }

        private void setupConfiguration(ApplesGameConfig config)
        {
            treesCount = config.TreesCount;
            applesOnTree = config.ApplesOnTreeCount;
            basketCount = config.BasketCount;
            windowWidth = Application.Current.MainWindow.Width;
            windowHeight = Application.Current.MainWindow.Height;
        }

        private void setupKinectSensor(ApplesGameConfig config = null)
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;

            if (config == null)
            {
                this.sensorChooserUI.KinectSensorChooser = this.sensorChooser;
                this.sensorChooser.Start();
                var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
                BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
            }
            else
            {
                this.sensorChooserUI.KinectSensorChooser = config.PassedKinectSensorChooser;
                this.sensorChooser.Start();
                var regionSensorBinding = new Binding("Kinect") { Source = config.PassedKinectSensorChooser };
                BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
            }
        }

        private void runGame()
        {
            setBackground();
            setScores();
            createApplesAndTrees();
            createBaskets();
        }

        private void setBackground()
        {
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri(@"../../../Graphics/Common/ApplesGameBackground.png", UriKind.Relative));
            bg.Stretch = Stretch.UniformToFill;
            kinectRegionGrid.Background = bg;
        }

        private void setScores()
        {
            gameScore = new Score(treesCount * applesOnTree);
            kinectRegionGrid.Children.Add(gameScore.Scoreboard);
        }

        private void createApplesAndTrees()
        {
            setAppleSize();

            Point rangeMin = new Point();
            Point rangeMax = new Point();
            Canvas[] tree = new Canvas[treesCount];
            myApple = new Apple[treesCount * applesOnTree];
            ImageBrush treeBg = new ImageBrush();
            treeBg.ImageSource = new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/tree.png", UriKind.Relative));

            int appleCounter = 0;
            for (int i = 0; i < treesCount; i++)
            {
                tree[i] = new Canvas();
                tree[i].Width = (int)(windowWidth / treesCount * 0.5) * 1.5;//*1.7 - full screen of trees;
                tree[i].Height = (int)(windowHeight / 1.25);
                Canvas.SetTop(tree[i], 50);
                Canvas.SetLeft(tree[i], (i * tree[i].Width + 20));
                tree[i].Margin = new Thickness((i * 0.2 * tree[i].Width), 0, 20, 0);
                tree[i].Name = "tree" + i;
                playfield.Children.Add(tree[i]);
                tree[i].Background = treeBg;

                setApplesArena(ref rangeMin, ref rangeMax);
                colorsCount = basketCount;

                for (int j = 0; j < applesOnTree; j++)
                {
                    myApple[appleCounter] = new Apple(rangeMin, rangeMax, appleSize, appleCounter, i, colorsCount);
                    var button = myApple[appleCounter].Figure;
                    button.Foreground = new SolidColorBrush(Colors.Transparent);
                    KinectRegion.AddQueryInteractionStatusHandler(button, OnQuery);
                    KinectRegion.AddHandPointerGripHandler(button, OnHandPointerGrip);
                    tree[i].Children.Add(button);
                    button.MouseEnter += button_MouseEnter;
                    button.StylusEnter += button_StylusEnter;
                    button.MouseLeave += button_MouseLeave;
                    button.StylusLeave += button_StylusLeave;
                    appleCounter++;
                }
            }
        }

        private void setApplesArena(ref Point rangeMin, ref Point rangeMax)
        {
            //TODO: Check if always looks good --first test passed--
            rangeMin.X = 10;
            rangeMin.Y = 10;
            if (windowWidth < 1440)
            {
                rangeMax.X = 280;
                rangeMax.Y = 180;
            }
            else
            {
                rangeMax.X = 350;
                rangeMax.Y = 380;
            }
        }

        private void setAppleSize()
        {
            if (applesOnTree < 10)
                appleSize = 200;
            else if (applesOnTree >= 10 && applesOnTree <= 20)
                appleSize = 150;
            else
                appleSize = 125;
        }

        private void createBaskets()
        {
            basket = new Basket[basketCount];
            bool [] basketColors = new bool[basketCount];

            int basketSize;
            if (windowWidth < 1440)
                basketSize = 300;
            else
                basketSize = 400;

            for (int i = 0; i < basketCount; i++)
            {
                double x = (windowWidth / basketCount) * i - 30;
                double y = windowHeight - basketSize * 0.85;
                Point basketPosition = new Point((int)x, (int)y);                
                basket[i] = new Basket(basketSize, basketSize, basketPosition, randomColor(ref basketColors, basketCount));
                playfield.Children.Add(basket[i].Figure);
            }
        }

        private int randomColor(ref bool[] basketColors, int size)
        {
            System.Random rand = new Random(Guid.NewGuid().GetHashCode());
            int color = rand.Next(0, size - 1);
            while (basketColors[color % size] == true)
            {
                color++;
            }
            basketColors[color % size] = true;
            return color;
        }
        #endregion

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

        #region Kinect Grip Events
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
                        && (point.Y <= basket[i].EndPosition.Y))
                    {
                        if (GripApple.Color == basket[i].Color)
                        {
                            handPointerEventArgs.Handled = true;
                            gameScore.collectSuccess();
                            check = true;
                        }
                        else
                        {
                            handPointerEventArgs.Handled = true;
                            gameScore.collectFail();
                            check = false;
                        }
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

        void button_StylusEnter(object sender, StylusEventArgs e)
        {
            Apple hoveredApple;
            var button = sender as KinectCircleButton;
            hoveredApple = myApple[(int)button.Content];
            appleHoverBackground(hoveredApple);    
        }
        void button_MouseEnter(object sender, MouseEventArgs e)
        {
            Apple hoveredApple;
            var button = sender as KinectCircleButton;
            hoveredApple = myApple[(int)button.Content];
            appleHoverBackground(hoveredApple);
        }

        void button_StylusLeave(object sender, StylusEventArgs e)
        {
            Apple hoveredApple;
            var button = sender as KinectCircleButton;
            hoveredApple = myApple[(int)button.Content];
            restoreAppleBackground(hoveredApple);
        }
        void button_MouseLeave(object sender, MouseEventArgs e)
        {
            Apple hoveredApple;
            var button = sender as KinectCircleButton;
            hoveredApple = myApple[(int)button.Content];
            restoreAppleBackground(hoveredApple);
        }

        void appleHoverBackground(Apple sender)
        {
            ImageBrush bg = new ImageBrush();
            //conditions
            {
                if (sender.Color == Colors.Red)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/bgl_red.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Yellow)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/bgl_yellow.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Green)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/bgl_green.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Orange)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/bgl_orange.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Brown)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/bgl_brown.png", UriKind.Relative));
                }
            }
            sender.Figure.Background = bg;
        }

        void restoreAppleBackground(Apple sender)
        {
            ImageBrush bg = new ImageBrush();
            //conditions
            {
                if (sender.Color == Colors.Red)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/red_apple.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Yellow)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/yellow_apple.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Green)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/green_apple.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Orange)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/orange_apple.png", UriKind.Relative));
                }
                if (sender.Color == Colors.Brown)
                {
                    bg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/brown_apple.png", UriKind.Relative));
                }
            }
            sender.Figure.Background = bg;
        }
        #endregion

        #region Closing window
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sensorChooser.Stop();
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            KinectSensorManager.KinectSensor = null;
        }


        private void ApplesGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }
        #endregion
    }
}
