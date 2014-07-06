using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Samples.Kinect.WpfViewers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LettersGame.View;

namespace LettersGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region private fields
        private KinectSensorChooser sensorChooser;
        private LettersGameConfig config;
        #endregion
        
        #region constructor
        public MainWindow()
        {
            InitializeComponent();
            this.config = new LettersGameConfig();
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += sensorChooser_KinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.config.PassedSensorChooser = this.sensorChooser;
            this.config.CurrentLevel = 1;
            this.config.WindowHeight = this.Height;
            this.config.WindowWidth = this.Width;
            this.sensorChooser.Start();
        }

        public MainWindow(LettersGameConfig config)
        {
            InitializeComponent();
            this.config = config;
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += sensorChooser_KinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.config.PassedSensorChooser = this.sensorChooser;
            this.config.WindowHeight = this.Height;
            this.config.WindowWidth = this.Width;
            this.sensorChooser.Start();
        }
        #endregion

        #region kinect setup
        void sensorChooser_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            if (e.OldSensor != null)
            {
                try
                {
                    e.OldSensor.DepthStream.Range = DepthRange.Default;
                    e.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    e.OldSensor.DepthStream.Disable();
                    e.OldSensor.SkeletonStream.Disable();
                    e.OldSensor.ColorStream.Disable();
                    e.OldSensor.Stop();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (e.NewSensor != null)
            {
                try
                {
                    e.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    //e.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        e.NewSensor.DepthStream.Range = DepthRange.Near;
                        //e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                        e.NewSensor.Start();
                        this.runGame();
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        e.NewSensor.DepthStream.Range = DepthRange.Default;
                        //e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                        e.NewSensor.Start();
                        this.runGame();
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }
        #endregion

        #region window operations

        private void runGame()
        {
            if (this.sensorChooser.Kinect.IsRunning)
            {
                switch (this.config.CurrentLevel)
                {
                    case 1:
                        this.clearMainGrid();
                        //this.mainGrid
                        break;
                    case 2:
                        this.clearMainGrid();
                        break;
                    case 3:
                        this.clearMainGrid();
                        break;
                    default:
                        break;
                }
            }
        }

        private void clearMainGrid()
        {
            this.mainGrid.Children.Clear();
            this.mainGrid.RowDefinitions.Clear();
            this.mainGrid.ColumnDefinitions.Clear();
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            if (e.Key == Key.C)
            {
                this.clearMainGrid();
            }
            if (e.Key == Key.R)
            {
                switch (this.config.CurrentLevel)
                {
                    case 1:
                        this.clearMainGrid();
                        Frame frame = new Frame { Content = new FirstLevelView(this.config) };
                        this.mainGrid.Children.Add(frame);
                        break;
                    case 2:
                        this.clearMainGrid();
                        break;
                    case 3:
                        this.clearMainGrid();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
