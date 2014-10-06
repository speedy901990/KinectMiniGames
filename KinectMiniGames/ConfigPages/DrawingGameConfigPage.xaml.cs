using System.Windows.Controls;
using BubblesGame;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System.Windows;
using DrawingGame;
using System.Threading;

namespace KinectMiniGames.ConfigPages
{
    public partial class DrawingGameConfigPage : UserControl
    {
        KinectSensorChooser kinectSensor;
        public DrawingGameConfig Config { get { return config; } set { config = value;} }
        private DrawingGameConfig config;

        public DrawingGameConfigPage(string itemId, KinectSensorChooser kinectSensor)
        {
           
            this.InitializeComponent();
            this.kinectSensor = kinectSensor;
            config = new DrawingGameConfig();                                 
            this.DataContext = Config;
        }

        private void OnLoadedStoryboardCompleted(object sender, System.EventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }

        private void RunGame()                          
        {
            if (MainWindow.SelectedPlayer != null)
            {
                Config.Player = MainWindow.SelectedPlayer;

                //this.kinectSensor.Stop();

                DrawingGame.MainWindow window = new DrawingGame.MainWindow(Config);
                window.Show();
            }
            
        }

        private void kcbLevel1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Config.Precision = 1;
            Config.HandsState = 1;
            Config.Difficulty = 1;
            
            this.RunGame();
        }

        private void kcbLevel2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Config.Precision = 1;
            Config.HandsState = 2;
            Config.Difficulty = 1;
            this.RunGame();
        }

        private void kcbLevel3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Config.Precision = 2;
            Config.HandsState = 1;
            Config.Difficulty = 2;
            this.RunGame();
        }

        private void kcbLevel4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Config.Precision = 2;
            Config.HandsState = 2;
            Config.Difficulty = 2;
            this.RunGame();
        }

        private void ktbBackToMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
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
