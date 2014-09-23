using System;
using System.Windows;
using System.Windows.Data;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using KinectMiniGames.ConfigPages;
using DatabaseManagement;
using DatabaseManagement.Managers;
using System.Collections.Generic;

namespace KinectMiniGames
{
    public partial class MainWindow
    {
        #region Public State
        public int gamesCount = 3;

        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public bool PageLeftEnabled
        {
            get { return (bool)GetValue(PageLeftEnabledProperty); }
            set { this.SetValue(PageLeftEnabledProperty, value); }
        }
        public bool PageRightEnabled
        {
            get { return (bool)GetValue(PageRightEnabledProperty); }
            set { this.SetValue(PageRightEnabledProperty, value); }
        }

        public static Thread PlayersThread
        {
            get { return playersThread; }
        }

        public static List<Player> PlayerList
        {
            get { return MainWindow.playerList; }
            set { MainWindow.playerList = value; }
        }

        public static Player SelectedPlayer
        {
            get { return MainWindow.selectedPlayer; }
            set { MainWindow.selectedPlayer = value; }
        }
        #endregion

        #region Private State
        private const double ScrollErrorMargin = 0.001;
        private const int PixelScrollByAmount = 10;
        private KinectSensorChooser sensorChooser;
        private static Thread playersThread;
        private static List<Player> playerList;
        private static Player selectedPlayer;
        #endregion

        #region Ctor + Config
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            //setMenuBackground();
            setupKinectSensor();
            createMenuButtons();
            playersThread = new Thread(GetPlayersFromDatabase);
            playersThread.Start();
        }

        private void InitializeDatabase()
        {
            using (var context = new GameModelContainer())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();  
                }
                context.SaveChanges();
            }
        }

        private void setMenuBackground()
        {
            ImageBrush sky = new ImageBrush(convertBitmapToBitmapSource(Properties.Resources.sky));
            sky.Stretch = Stretch.UniformToFill;
            mainGrid.Background = sky;
        }

        private BitmapSource convertBitmapToBitmapSource(System.Drawing.Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }

        private void setupKinectSensor()
        {
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
            var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
        }

        private void createMenuButtons()
        {
            wrapPanel.Children.Clear();
            wrapPanel.Children.Add(createSingleButton("Apples Game"));
            wrapPanel.Children.Add(createSingleButton("Bubbles Game"));
            wrapPanel.Children.Add(createSingleButton("Letters Game"));
            //this.wrapPanel.Children.Add(this.createSingleButton("Labyrinth Game"));
            //this.wrapPanel.Children.Add(this.createSingleButton("Painting Game"));
            //this.wrapPanel.Children.Add(this.createSingleButton("Dancing Steps"));
            //this.wrapPanel.Children.Add(this.createSingleButton("Song Movements"));
            //this.wrapPanel.Children.Add(this.createSingleButton("Simple Excersises"));
            wrapPanel.Children.Add(createSingleButton("Train of Words"));
            //this.wrapPanel.Children.Add(this.createSingleButton("Steps of Activity"));
            //this.wrapPanel.Children.Add(this.createSingleButton("Educational Kinesiology"));
        }

        private KinectTileButton createSingleButton(String buttonLabel)
        {
            var newButton = new KinectTileButton { Label = buttonLabel, Width = 450, Height = 450 };
            return newButton;
        }

        private void GetPlayersFromDatabase()
        {
            var manager = new PlayersManager();
            playerList = manager.PlayerList;
        }
        #endregion

        #region Kinect discovery + setup
        private static void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
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

        private void KinectTileButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (KinectTileButton)e.OriginalSource;
            switch ((String)button.Label)
            {
                case "Apples Game":
                    var applesConfigPage = new ApplesGameConfigPage(button.Label as string, sensorChooser);
                    kinectRegionGrid.Children.Add(applesConfigPage);
                    e.Handled = true;
                    break;
                case "Bubbles Game":
                    var bubblesConfigPage = new BubblesGameConfigPage(button.Label as string, sensorChooser);
                    kinectRegionGrid.Children.Add(bubblesConfigPage);
                    e.Handled = true;
                    break;
                case "Letters Game":
                    var lettersConfigPage = new LettersGameConfigPage(button.Label as string, sensorChooser);
                    kinectRegionGrid.Children.Add(lettersConfigPage);
                    e.Handled = true;
                    break;
                case "Train of Words":
                    var trainConfigPage = new TrainOfWordsConfigPage(button.Label as string, sensorChooser);
                    kinectRegionGrid.Children.Add(trainConfigPage);
                    e.Handled = true;
                    break;
            }
        }
        #endregion

        #region Window events
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sensorChooser.Stop();
        }

        private void key_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        private void KinectMiniGames_Activated(object sender, EventArgs e)
        {
            kinectViewBorder.Visibility = System.Windows.Visibility.Visible;
            sensorChooserUi.Visibility = System.Windows.Visibility.Visible;

            if (sensorChooser.Status == ChooserStatus.None)
            {
                sensorChooser.Start();
            }
        }

        private void KinectMiniGames_Deactivated(object sender, EventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
            kinectViewBorder.Visibility = System.Windows.Visibility.Hidden;
            sensorChooserUi.Visibility = System.Windows.Visibility.Hidden;
        }

        #endregion
    }
}
