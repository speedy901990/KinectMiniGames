using Microsoft.Kinect.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LettersGame
{
    class LettersGameConfig
    {
        private KinectSensorChooser passedSensorChooser;

        public KinectSensorChooser PassedSensorChooser
        {
            get { return passedSensorChooser; }
            set { passedSensorChooser = value; }
        }

        public LettersGameConfig()
        {

        }
    }
}
