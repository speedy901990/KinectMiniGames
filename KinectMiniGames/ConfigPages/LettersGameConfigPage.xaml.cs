using LettersGame;
using Microsoft.Kinect.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectMiniGames.ConfigPages
{
    /// <summary>
    /// Interaction logic for LettersGameConfigPage.xaml
    /// </summary>
    public partial class LettersGameConfigPage : UserControl
    {
        private LettersGameConfig config;

        public LettersGameConfig Config
        {
            get { return config; }
            set { config = value; }
        }
        private KinectSensorChooser sensorChooser;

        public KinectSensorChooser SensorChooser
        {
            get { return sensorChooser; }
            set { sensorChooser = value; }
        }

        public LettersGameConfigPage(string itemId, KinectSensorChooser sensorChooser)
        {
            InitializeComponent();
            this.sensorChooser = sensorChooser;
            this.config = new LettersGameConfig();
            this.DataContext = Config;
        }

        private void ktbBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }

        private void kcbLevel1_Click(object sender, RoutedEventArgs e)
        {
            this.sensorChooser.Stop();
            this.config.PlayerName = this.tbUsername.Text;
            //troche hardkodu
            this.config.CurrentLevel = 1;
            LettersGame.MainWindow window = new LettersGame.MainWindow(Config);
            window.Show();
        }

        private void kcbLevel2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void kcbLevel3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
