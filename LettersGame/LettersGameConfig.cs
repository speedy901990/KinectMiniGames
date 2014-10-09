using DatabaseManagement;
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
    public class LettersGameConfig
    {
        public LettersGameConfig()
        {
            FirstLevelLettersCount = 8;
            LettersCount = 4;
            LettersFontSize = 100;
            LabelFontSize = 50;
        }

        public Player Player { get; set; }

        public string PlayerName { get; set; }

        public KinectSensorChooser PassedSensorChooser { get; set; }

        public int CurrentLevel { get; set; }

        public double WindowHeight { get; set; }

        public double WindowWidth { get; set; }

        public int FirstLevelLettersCount { get; set; }

        public int LettersFontSize { get; set; }

        public int LabelFontSize { get; set; }

        public int LettersCount { get; set; }

        public int TrolleysCount { get; set; }
    }
}
