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
        public LettersGameConfig Config { get; set; }

        public KinectSensorChooser SensorChooser { get; set; }

        public LettersGameConfigPage(string itemId, KinectSensorChooser sensorChooser)
        {
            InitializeComponent();
            SensorChooser = sensorChooser;
            Config = new LettersGameConfig();
            DataContext = Config;
        }

        private void ShowGameWindow()
        {
            if (MainWindow.SelectedPlayer != null)
            {
                Config.Player = MainWindow.SelectedPlayer;
                var window = new LettersGame.MainWindow(Config);
                window.Show();
            }
        }

        private void ktbBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var parent = (Panel)Parent;
            parent.Children.Remove(this);
        }
        private void kcbLevel1_Click(object sender, RoutedEventArgs e)
        {
            SensorChooser.Stop();
            Config.FirstLevelLettersCount = 8;
            Config.CurrentLevel = 1;

            ShowGameWindow();
        }

        private void kcbLevel2_Click(object sender, RoutedEventArgs e)
        {
            SensorChooser.Stop();
            Config.LettersCount = 16;
            Config.TrolleysCount = 4;
            Config.CurrentLevel = 2;

            ShowGameWindow();
        }

        private void kcbLevel3_Click(object sender, RoutedEventArgs e)
        {
            SensorChooser.Stop();
            Config.LettersCount = 4;
            Config.CurrentLevel = 3;

            ShowGameWindow();
        }

        private void btnSelectPlayer_Click(object sender, RoutedEventArgs e)
        {
            var playerSelection = new PlayerSelection();
            rootGrid.Children.Add(playerSelection);
        }

        private void UserControl_LayoutUpdated_1(object sender, EventArgs e)
        {
            if (IsInitialized && MainWindow.SelectedPlayer != null)
            {
                lbPlayer.Content = MainWindow.SelectedPlayer.name + " " + MainWindow.SelectedPlayer.surname;
            }
        }
    }
}
