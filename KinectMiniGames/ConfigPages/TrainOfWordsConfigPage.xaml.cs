using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect.Toolkit;
using TrainOfWords.Model;
using TrainOfWords.View;

namespace KinectMiniGames.ConfigPages
{
    /// <summary>
    /// Interaction logic for TrainOfWordsConfigPage.xaml
    /// </summary>
    public partial class TrainOfWordsConfigPage
    {
        public TrainOfWordsConfigPage()
        {
            InitializeComponent();
        }

        public TrainOfWordsGameConfig Config { get; set; }

        public KinectSensorChooser SensorChooser { get; set; }

        public TrainOfWordsConfigPage(string itemId, KinectSensorChooser sensorChooser)
        {
            InitializeComponent();
            SensorChooser = sensorChooser;
            Config = new TrainOfWordsGameConfig
            {
                WindowWidth = (int) Application.Current.MainWindow.Width
            };
            DataContext = Config;
            LayoutUpdated += OnLayoutUpdated;
        }

        private void ShowGameWindow()
        {
            if (MainWindow.SelectedPlayer == null) 
                return;
            Config.Player = MainWindow.SelectedPlayer;
            var window = new TrainOfWords.View.MainWindow(Config);
            window.Show();
        }

        private void ktbBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var parent = (Panel)Parent;
            parent.Children.Remove(this);
        }
        private void kcbLevel1_Click(object sender, RoutedEventArgs e)
        {
            SensorChooser.Stop();
            Config.Level = 1;

            ShowGameWindow();
        }

        private void kcbLevel2_Click(object sender, RoutedEventArgs e)
        {
            SensorChooser.Stop();
            Config.Level = 2;

            ShowGameWindow();
        }

        private void kcbLevel3_Click(object sender, RoutedEventArgs e)
        {
            SensorChooser.Stop();
            Config.Level = 3;

            ShowGameWindow();
        }

        private void btnSelectPlayer_Click(object sender, RoutedEventArgs e)
        {
            var playerSelection = new PlayerSelection();
            rootGrid.Children.Add(playerSelection);
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            if (IsInitialized && MainWindow.SelectedPlayer != null)
            {
                lbPlayer.Content = String.Format("{0} {1}", MainWindow.SelectedPlayer.Name, MainWindow.SelectedPlayer.Surname);
            }
        }
    }
}
