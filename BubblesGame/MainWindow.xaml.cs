using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Samples.Kinect.WpfViewers;

namespace BubblesGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BubblesGameConfig config;
        private KinectSensorChooser _sensorChooser;

        #region kinect setup
        public static readonly DependencyProperty KinectSensorManagerProperty =
            DependencyProperty.Register(
                "KinectSensorManager",
                typeof(KinectSensorManager),
                typeof(MainWindow),
                new PropertyMetadata(null));

        public KinectSensorManager KinectSensorManager
        {
            get { return (KinectSensorManager)GetValue(KinectSensorManagerProperty); }
            set { SetValue(KinectSensorManagerProperty, value); }
        }

        #endregion

        #region ctor
        public MainWindow()
        {
            InitializeComponent();
            this.config = new BubblesGameConfig();
            this._sensorChooser = new KinectSensorChooser();
            this._sensorChooser.KinectChanged +=_sensorChooser_KinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this._sensorChooser;
            this.config.PassedKinectSensorChooser = this._sensorChooser;
            this._sensorChooser.Start();
        }

        public MainWindow(BubblesGameConfig config)
        {
            InitializeComponent();
            this.config = config;
            this._sensorChooser = new KinectSensorChooser();
            this._sensorChooser.KinectChanged += _sensorChooser_KinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this._sensorChooser;
            this.config.PassedKinectSensorChooser = this._sensorChooser;
            this._sensorChooser.Start();
        }
        #endregion

        #region Closing window
        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            // dla testow przycisk R
            if (e.Key == Key.R)
            {
                GameWindow window = new GameWindow(this.config);
                window.Show();
                this.Close();
            }
        }
        #endregion

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
           
        }

        void _sensorChooser_KinectChanged(object sender, KinectChangedEventArgs e)
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
                    e.NewSensor.SkeletonStream.Enable();
                    //e.NewSensor.ColorStream.Enable();

                    try
                    {
                        e.NewSensor.DepthStream.Range = DepthRange.Near;
                        e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                        e.NewSensor.Start();
                        this.openGameWindow();
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        e.NewSensor.DepthStream.Range = DepthRange.Default;
                        e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                        e.NewSensor.Start();
                        this.openGameWindow();
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
            //throw new NotImplementedException();
        }

        void newSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void openGameWindow()
        {

            if (_sensorChooser.Kinect.IsRunning)
            {
                GameWindow window = new GameWindow(this.config);
                window.Show();
                this.Close();
            }
        }

        private void stopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                sensor.Stop();
                sensor.AudioSource.Stop();
                this._sensorChooser.Stop();
            }
        }

    }
}
