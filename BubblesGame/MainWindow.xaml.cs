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
        private readonly KinectSensorChooser _sensorChooser = new KinectSensorChooser();

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

        private void KinectSensorChanged(object sender, KinectSensorManagerEventArgs<KinectSensor> args)
        {
            if (_sensorChooser.Status == ChooserStatus.SensorStarted)
            {
                GameWindow window = new GameWindow(this.config);
                window.Show();
                this.Close();
            }
        }
        #endregion

        #region ctor
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(BubblesGameConfig config)
        {
            InitializeComponent();
            this.config = config;
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
    }
}
