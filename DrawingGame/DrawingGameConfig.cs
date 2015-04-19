using System.ComponentModel;
using Microsoft.Kinect.Toolkit;

namespace DrawingGame
{
    public class DrawingGameConfig : INotifyPropertyChanged
    {
         #region Private State
        private KinectSensorChooser _kinectSensor;
        private DatabaseManagement.Player _player;
        private string _username = "Gracz";
        private string _userSurname = "Testowy";
         
          private  int _difficulty ;
          private  int _handsState ;
          private int _precision;
         #endregion 

        #region Public State
        
        public KinectSensorChooser PassedKinectSensorChooser
        {
            get { return _kinectSensor; }
            set { _kinectSensor = value;
            ;
            }
        }
        public DatabaseManagement.Player Player
        {
            get { return _player; }
            set { _player = value; }
        }
        public string UserName { 
            get { return _username;} 
            set{_username = value;
            OnPropertyChanged("DrawingUserName");
            } 
        }
        public string UserSurname 
        { get { return _userSurname; }
            set { _userSurname = value;
            OnPropertyChanged("DrawingUserSurname");
            }
        }
        public int Difficulty {
            get { return _difficulty; } 
            set { _difficulty = value;
            OnPropertyChanged("DrawingDifficulty");
            }
        }
        public int HandsState { 
            get { return _handsState; } 
            set { _handsState = value;
            OnPropertyChanged("DrawingHandsState");
            }
        }
        public int Precision { 
            get { return _precision; }
            set { _precision = value;
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
