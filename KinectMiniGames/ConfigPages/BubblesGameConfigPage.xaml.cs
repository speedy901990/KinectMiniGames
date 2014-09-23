using System.Windows.Controls;
using BubblesGame;
using Microsoft.Kinect.Toolkit;
using System.Windows;

namespace KinectMiniGames.ConfigPages
{
    public partial class BubblesGameConfigPage : UserControl
    {
        private readonly KinectSensorChooser _kinectSensor;
        public BubblesGameConfig Config { get; set; }

        public BubblesGameConfigPage(string itemId, KinectSensorChooser kinectSensor)
        {
            InitializeComponent();
            _kinectSensor = kinectSensor;
            Config = new BubblesGameConfig();
            DataContext = Config;
        }

        private void OnLoadedStoryboardCompleted(object sender, System.EventArgs e)
        {
            var parent = (Panel)Parent;
            parent.Children.Remove(this);
        }

        private void RunGame()
        {
            if (MainWindow.SelectedPlayer == null) 
                return;
            Config.Player = MainWindow.SelectedPlayer;
            _kinectSensor.Stop();
            var window = new BubblesGame.MainWindow(Config);
            window.Show();
        }

        private void kcbLevel1_Click(object sender, RoutedEventArgs e)
        {
            Config.BubblesFallSpeed = 1;
            Config.BubblesApperanceFrequency = 1;
            Config.BubblesCount = 20;
            Config.BubblesSize = 90;
            Config.Level = 1;
            RunGame();
        }

        private void kcbLevel2_Click(object sender, RoutedEventArgs e)
        {
            Config.BubblesFallSpeed = 2;
            Config.BubblesApperanceFrequency = 2;
            Config.BubblesCount = 40;
            Config.BubblesSize = 70;
            Config.Level = 2;
            RunGame();
        }

        private void kcbLevel3_Click(object sender, RoutedEventArgs e)
        {
            Config.BubblesFallSpeed = 3;
            Config.BubblesApperanceFrequency = 3;
            Config.BubblesCount = 60;
            Config.BubblesSize = 50;
            Config.Level = 3;
            RunGame();
        }

        private void kcbLevel4_Click(object sender, RoutedEventArgs e)
        {
            Config.BubblesFallSpeed = 5;
            Config.BubblesApperanceFrequency = 5;
            Config.BubblesCount = 60;
            Config.BubblesSize = 40;
            Config.Level = 4;
            RunGame();
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

        private void UserControl_LayoutUpdated_1(object sender, System.EventArgs e)
        {
            if (this.IsInitialized && MainWindow.SelectedPlayer != null)
            {
                lbPlayer.Content = MainWindow.SelectedPlayer.Name + " " + MainWindow.SelectedPlayer.Surname;
            }
        }
    }
}
