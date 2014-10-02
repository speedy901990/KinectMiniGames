using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System.ComponentModel;

namespace DrawingGame
{
    public class DrawingGameConfig : INotifyPropertyChanged
    {
         #region Private State
        private KinectSensorChooser kinectSensor;
        private DatabaseManagement.Player player;
        private string username = "Gracz";
        private string userSurname = "Testowy";
         
          private  int difficulty ;
          private  int handsState ;
          private int precision;
         #endregion 

        #region Public State
        
        public KinectSensorChooser PassedKinectSensorChooser
        {
            get { return this.kinectSensor; }
            set { this.kinectSensor = value;
            ;
            }
        }
        public DatabaseManagement.Player Player
        {
            get { return player; }
            set { player = value; }
        }
        public string UserName { 
            get { return username;} 
            set{username = value;
            OnPropertyChanged("DrawingUserName");
            } 
        }
        public string UserSurname 
        { get { return userSurname; }
            set { userSurname = value;
            OnPropertyChanged("DrawingUserSurname");
            }
        }
        public int Difficulty {
            get { return difficulty; } 
            set { difficulty = value;
            OnPropertyChanged("DrawingDifficulty");
            }
        }
        public int HandsState { 
            get { return handsState; } 
            set { handsState = value;
            OnPropertyChanged("DrawingHandsState");
            }
        }
        public int Precision { 
            get { return precision; }
            set { precision = value;
            OnPropertyChanged("DrawingPrecision");
            }
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
