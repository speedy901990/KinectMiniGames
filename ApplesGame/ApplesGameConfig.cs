using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System.ComponentModel;

namespace ApplesGame
{
    public class ApplesGameConfig : INotifyPropertyChanged
    {
        #region Private State
        private string username = "Gracz";
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
            set
            {
                this.basketCount = value;
                OnPropertyChanged("BasketCount");
            }
        }

        public int ColorCount
        {
            get { return this.colorCount; }
            set
            {
                this.colorCount = value;
                OnPropertyChanged("ColorCount");
            }
        }

        public string Username
        {
            get { return this.username; }

            set
            {
                this.username = value;
                OnPropertyChanged("Username");
            }
        }

        public int TreesCount
        {
            get { return this.treesCount; }
            set
            {
                this.treesCount = value;
                OnPropertyChanged("TreesCount");
            }
        }

        public int ApplesOnTreeCount
        {
            get { return this.applesOnTreeCount; }
            set
            {
                this.applesOnTreeCount = value;
                OnPropertyChanged("ApplesOnTreeCount");
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
