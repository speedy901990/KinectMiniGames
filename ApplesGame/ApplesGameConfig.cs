using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace ApplesGame
{
    public class ApplesGameConfig
    {
        private string username;
        private int treesCount;
        private int applesOnTreeCount;
        private KinectSensorChooser kinectSensor;

        public ApplesGameConfig()
        {

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
    }
}
