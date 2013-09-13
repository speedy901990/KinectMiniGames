using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System.ComponentModel;

namespace BubblesGame
{
    public class BubblesGameConfig : INotifyPropertyChanged
    {
        #region Private State
        private KinectSensorChooser kinectSensor;
        private string username = "Gracz";
        private int bubbleSize = 40;
        private int bubblesCount = 20;
        private int bubbleFallSpeed = 3;
        private int bubblesApperanceFrequency = 2;
        #endregion

        #region Public State
        public string Username
        {
            get { return this.username; }
            set
            {
                this.username = value;
                OnPropertyChanged("Username");
            }
        }

        public int BubblesFallSpeed
        {
            get { return this.bubbleFallSpeed; }
            set
            {
                this.bubbleFallSpeed = value;
                OnPropertyChanged("BubblesFallSpeed");
            }
        }

        public int BubblesCount
        {
            get { return this.bubblesCount; }
            set
            {
                this.bubblesCount = value;
                OnPropertyChanged("BubblesCount");
            }
        }

        public int BubblesSize
        {
            get { return this.bubbleSize; }
            set
            {
                this.bubbleSize = value;
                OnPropertyChanged("BubblesSize");
            }
        }

        public int BubblesApperanceFrequency
        {
            get { return this.bubblesApperanceFrequency; }
            set
            {
                this.bubblesApperanceFrequency = value;
                OnPropertyChanged("BubblesApperanceFrequency");
            }
        }

        public KinectSensorChooser PassedKinectSensorChooser
        {
            get { return this.kinectSensor; }
            set { this.kinectSensor = value; }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propname));
            }
        }

        #endregion
    }
}
