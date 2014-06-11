using Microsoft.Kinect.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LettersGame
{
    public class LettersGameConfig : DependencyObject
    {
        public LettersGameConfig()
        {
            this.firstLevelLettersCount = 8;
        }

        private KinectSensorChooser passedSensorChooser;

        public KinectSensorChooser PassedSensorChooser
        {
            get { return passedSensorChooser; }
            set { passedSensorChooser = value; }
        }

        private int currentLevel;

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value;}
        }        

        private double windowHeight;

        public double WindowHeight
        {
            get { return windowHeight; }
            set { windowHeight = value; }
        }

        private double windowWidth;

        public double WindowWidth
        {
            get { return windowWidth; }
            set { windowWidth = value; }
        }

        private int firstLevelLettersCount;

        public int FirstLevelLettersCount
        {
            get { return firstLevelLettersCount; }
            set { firstLevelLettersCount = value; }
        }
    }
}
