using System.Windows.Controls;
using ApplesGame;
using Microsoft.Kinect.Toolkit;
using System;
using System.Windows;

namespace KinectMiniGames.ConfigPages
{
    public partial class ApplesGameConfigPage : UserControl
    {
        readonly KinectSensorChooser _kinectSensor;
        public ApplesGameConfig Config { get; set; }

        public ApplesGameConfigPage(string itemId, KinectSensorChooser kinectSensor)
        {
            InitializeComponent();
            _kinectSensor = kinectSensor;
            Config = new ApplesGameConfig();
            DataContext = Config;
        }

        private void OnLoadedStoryboardCompleted(object sender, EventArgs e)
        {
            var parent = (Panel)Parent;
            parent.Children.Remove(this);
        }

        private void ShowGameWindow()
        {
            if (MainWindow.SelectedPlayer == null) 
                return;
            Config.Player = MainWindow.SelectedPlayer;
            Config.PassedKinectSensorChooser = _kinectSensor;
            var window = new ApplesGame.MainWindow(Config);
            window.Show();
        }

        private void kcbLevel1_Click(object sender, RoutedEventArgs e)
        {
            Config.TreesCount = Application.Current.MainWindow.Width < 1440 ? 2 : 3;

            Config.ApplesOnTreeCount = 4;
            Config.ColorCount = 3;
            Config.BasketCount = 3;

            ShowGameWindow();
        }

        private void kcbLevel2_Click(object sender, RoutedEventArgs e)
        {
            Config.TreesCount = Application.Current.MainWindow.Width < 1440 ? 2 : 3;

            Config.ApplesOnTreeCount = 6;
            Config.ColorCount = 4;
            Config.BasketCount = 4;

            ShowGameWindow();
        }

        private void kcbLevel3_Click(object sender, RoutedEventArgs e)
        {
            Config.TreesCount = Application.Current.MainWindow.Width < 1440 ? 2 : 3;

            Config.ApplesOnTreeCount = 10;
            Config.ColorCount = 5;
            Config.BasketCount = 6;

            ShowGameWindow();
        }

        private void ktbBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var parent = (Panel)Parent;
            parent.Children.Remove(this);
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
                lbPlayer.Content = MainWindow.SelectedPlayer.Name + " " + MainWindow.SelectedPlayer.Surname;
            }
        }
    }
}
