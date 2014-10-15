using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KinectMiniGames.Models;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System.Threading;
using KinectMiniGames.ConfigPages;
using DatabaseManagement;
using DatabaseManagement.Managers;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KinectMiniGames
{
    public partial class MainWindow
    {
        #region Public State
        public int GamesCount = 3;

        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public bool PageLeftEnabled
        {
            get { return (bool)GetValue(PageLeftEnabledProperty); }
            set { SetValue(PageLeftEnabledProperty, value); }
        }
        public bool PageRightEnabled
        {
            get { return (bool)GetValue(PageRightEnabledProperty); }
            set { SetValue(PageRightEnabledProperty, value); }
        }

        public static Thread PlayersThread { get; private set; }

        public static List<Player> PlayerList { get; set; }

        public static Player SelectedPlayer { get; set; }

        #endregion

        #region Private State

        private KinectSensorChooser _sensorChooser;

        private DatabaseInitializer _databaseInitializer;

        #endregion

        #region Ctor + Config
        public MainWindow()
        {
            var executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            InitializeComponent();
            _databaseInitializer = new DatabaseInitializer();
            SetupKinectSensor();
            CreateMenuButtons();
            PlayersThread = new Thread(GetPlayersFromDatabase);
            PlayersThread.Start();
        }

        private void SetupKinectSensor()
        {
            _sensorChooser = new KinectSensorChooser();
            _sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            sensorChooserUi.KinectSensorChooser = _sensorChooser;
            _sensorChooser.Start();
            var regionSensorBinding = new Binding("Kinect") { Source = _sensorChooser };
            BindingOperations.SetBinding(kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
        }

        private void CreateMenuButtons()
        {
            wrapPanel.Children.Clear();
            wrapPanel.Children.Add(new KinectTileButton { Label = "Apples Game", Width = 450, Height = 450, Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.jablka))});
            wrapPanel.Children.Add(new KinectTileButton { Label = "Bubbles Game", Width = 450, Height = 450, Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.babelki)) });
            wrapPanel.Children.Add(new KinectTileButton { Label = "Letters Game", Width = 450, Height = 450, Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.litery)) });
            wrapPanel.Children.Add(new KinectTileButton { Label = "Drawing Game", Width = 450, Height = 450, Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.kinezjologia)) });
            wrapPanel.Children.Add(new KinectTileButton { Label = "Train of Words", Width = 450, Height = 450, Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.wagon1)) });
            //this.wrapPanel.Children.Add(this.createSingleButton("Labyrinth Game"));
            //this.wrapPanel.Children.Add(this.CreateSingleButton("Painting Game"));
            //this.wrapPanel.Children.Add(this.CreateSingleButton("Dancing Steps"));
            //this.wrapPanel.Children.Add(this.CreateSingleButton("Song Movements"));
            //this.wrapPanel.Children.Add(this.CreateSingleButton("Simple Excersises"));
            //this.wrapPanel.Children.Add(this.CreateSingleButton("Steps of Activity"));
            //this.wrapPanel.Children.Add(this.CreateSingleButton("Educational Kinesiology"));
        }

        private KinectTileButton CreateSingleButton(String buttonLabel)
        {
            var newButton = new KinectTileButton { Label = buttonLabel, Width = 450, Height = 450 };
            return newButton;
        }

        private void GetPlayersFromDatabase()
        {
            var manager = new PlayersManager();
            PlayerList = manager.PlayerList;
        }

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
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
                    var applesConfigPage = new ApplesGameConfigPage(button.Label as string, _sensorChooser);
                    kinectRegionGrid.Children.Add(applesConfigPage);
                    e.Handled = true;
                    break;
                case "Bubbles Game":
                    var bubblesConfigPage = new BubblesGameConfigPage(button.Label as string, _sensorChooser);
                    kinectRegionGrid.Children.Add(bubblesConfigPage);
                    e.Handled = true;
                    break;
                case "Letters Game":
                    var lettersConfigPage = new LettersGameConfigPage(button.Label as string, _sensorChooser);
                    kinectRegionGrid.Children.Add(lettersConfigPage);
                    e.Handled = true;
                    break;
                case "Drawing Game":
                    var drawingConfigPage = new DrawingGameConfigPage(button.Label as string, _sensorChooser);
                    kinectRegionGrid.Children.Add(drawingConfigPage);
                    e.Handled = true;
                    break;
                case "Train of Words":
                    var trainConfigPage = new TrainOfWordsConfigPage(button.Label as string, _sensorChooser);
                    kinectRegionGrid.Children.Add(trainConfigPage);
                    e.Handled = true;
                    break;
            }
        }
        #endregion

        #region Window events
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _sensorChooser.Stop();
        }

        private void key_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                Close();
        }

        private void KinectMiniGames_Activated(object sender, EventArgs e)
        {
            kinectViewBorder.Visibility = Visibility.Visible;
            sensorChooserUi.Visibility = Visibility.Visible;

            

            if (_sensorChooser.Status == ChooserStatus.None)
            {
                _sensorChooser.Start();
            }
            kinectRegion.KinectSensor = _sensorChooser.Kinect;
            GetPlayersFromDatabase();

        }

        private void KinectMiniGames_Deactivated(object sender, EventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
            kinectViewBorder.Visibility = Visibility.Hidden;
            sensorChooserUi.Visibility = Visibility.Hidden;
            kinectRegion.KinectSensor = null;
        }

        #endregion

        private void MenagementButton_Click(object sender, RoutedEventArgs e)
        {
            GameManagement.MainWindow window = new GameManagement.MainWindow();
            window.Show();

        }
    }
}
