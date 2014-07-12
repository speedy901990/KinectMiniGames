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

        private void ShowGameWindow()
        {
            if (MainWindow.SelectedPlayer != null)
            {
                Config.Player = MainWindow.SelectedPlayer;
                LettersGame.MainWindow window = new LettersGame.MainWindow(Config);
                window.Show();
            }
        }

        private void ktbBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
        private void kcbLevel1_Click(object sender, RoutedEventArgs e)
        {
            this.sensorChooser.Stop();
            Config.FirstLevelLettersCount = 8;
            Config.CurrentLevel = 1;

            this.ShowGameWindow();
        }

        private void kcbLevel2_Click(object sender, RoutedEventArgs e)
        {
            this.sensorChooser.Stop();
            Config.LettersCount = 4;
            Config.CurrentLevel = 2;

            this.ShowGameWindow();
        }

        private void kcbLevel3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSelectPlayer_Click(object sender, RoutedEventArgs e)
        {
            var playerSelection = new PlayerSelection();
            rootGrid.Children.Add(playerSelection);
        }

        private void UserControl_LayoutUpdated_1(object sender, EventArgs e)
        {
            if (this.IsInitialized && MainWindow.SelectedPlayer != null)
            {
                lbPlayer.Content = MainWindow.SelectedPlayer.name + " " + MainWindow.SelectedPlayer.surname;
            }
        }
    }
}
