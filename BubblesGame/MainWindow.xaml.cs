using Microsoft.Kinect;
using System.Linq;
using System.Windows;

namespace BubblesGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Member Variables
        private KinectSensor _kinect;
        #endregion Member Variables

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => DiscoverKinectSensor();
            Unloaded += (s, e) => { Kinect = null; };
        }
        #endregion Constructor

        #region Methods
        private void DiscoverKinectSensor()
        {
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            Kinect = KinectSensor.KinectSensors
                                      .FirstOrDefault(x => x.Status == KinectStatus.Connected);
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (Kinect == null)
                    {
                        Kinect = e.Sensor;
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (Kinect == e.Sensor)
                    {
                        Kinect = null;
                        Kinect = KinectSensor.KinectSensors
                                      .FirstOrDefault(x => x.Status == KinectStatus.Connected);
                        if (Kinect == null)
                        {
                            //Notify the user that the sensor is disconnected
                        }
                    }
                    break;

                //Handle all other statuses according to needs
            }
        }
        #endregion Methods

        #region Properties
        public KinectSensor Kinect
        {
            get { return _kinect; }
            set
            {
                if (_kinect != value)
                {
                    if (_kinect != null)
                    {
                        //Uninitialize
                        _kinect = null;
                    }
                    if (value != null && value.Status == KinectStatus.Connected)
                    {
                        _kinect = value;
                        //Initialize
                    }
                }
            }
        }
        #endregion Properties
    }
}
