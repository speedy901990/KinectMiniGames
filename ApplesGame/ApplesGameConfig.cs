using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace ApplesGame
{
    public class ApplesGameConfig
    {
        #region Private State
        private string username;
        private int treesCount;
        private int applesOnTreeCount;
        private int colorCount;
        private int basketCount;
        private KinectSensorChooser kinectSensor;
        #endregion

        #region Public State
        public int BasketCount
        {
            get { return this.basketCount; }
            set { this.basketCount = value; }
        }

        public int ColorCount
        {
            get { return this.colorCount; }
            set { this.colorCount = value; }
        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public int TreesCount
        {
            get { return this.treesCount; }
            set { this.treesCount = value; }
        }

        public int ApplesOnTreeCount
        {
            get { return this.applesOnTreeCount; }
            set { this.applesOnTreeCount = value; }
        }

        public KinectSensorChooser PassedKinectSensorChooser
        {
            get { return this.kinectSensor; }
            set { this.kinectSensor = value; }
        }
        #endregion

        #region Ctor
        public ApplesGameConfig()
        {

        }  
        #endregion
    }
}
