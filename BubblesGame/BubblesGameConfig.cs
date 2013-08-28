using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;

namespace BubblesGame
{
    public class BubblesGameConfig
    {
        private KinectSensorChooser kinectSensor;
        private string username;
        private int bubbleSize;
        private int bubblesCount;
        private int bubbleFallSpeed;
        private int bubblesApperanceFrequency;

        public string Username
        {
            get { return this.username; }
            set { this.username = value;}
        }

        public int BubblesFallSpeed
        {
            get { return this.bubbleFallSpeed; }
            set { this.bubbleFallSpeed = value; }
        }

        public int BubblesCount
        {
            get { return this.bubblesCount; }
            set { this.bubblesCount = value; }
        }

        public int BubblesSize 
        {
            get { return this.bubbleSize; }
            set { this.bubbleSize = value; }
        }

        public int BubblesApperanceFrequency
        {
            get { return this.bubblesApperanceFrequency; }
            set { this.bubblesApperanceFrequency = value; }
        }

        public KinectSensorChooser PassedKinectSensorChooser
        {
            get { return this.kinectSensor; }
            set { this.kinectSensor = value; }
        }
    }
}
