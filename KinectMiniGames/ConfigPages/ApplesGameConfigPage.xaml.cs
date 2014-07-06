using System.Windows.Controls;
using ApplesGame;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Windows;

namespace KinectMiniGames
{
    public partial class ApplesGameConfigPage : UserControl
    {
        KinectSensorChooser kinectSensor;
        public ApplesGameConfig Config { get; set; }

        public ApplesGameConfigPage(string itemId, KinectSensorChooser kinectSensor)
        {
            this.InitializeComponent();
            this.kinectSensor = kinectSensor;
            Config = new ApplesGameConfig();
            this.DataContext = Config;
        }

        private void OnLoadedStoryboardCompleted(object sender, System.EventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }

        private void ShowGameWindow()
        {
            Config.PassedKinectSensorChooser = this.kinectSensor;
            ApplesGame.MainWindow window = new ApplesGame.MainWindow(Config);
            window.Show();
        }

        private void kcbLevel1_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.Width < 1440)
                Config.TreesCount = 2;
            else
                Config.TreesCount = 3;

            Config.ApplesOnTreeCount = 4;
            Config.ColorCount = 3;
            Config.BasketCount = 3;

            this.ShowGameWindow();
        }

        private void kcbLevel2_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.Width < 1440)
                Config.TreesCount = 2;
            else
                Config.TreesCount = 3;

            Config.ApplesOnTreeCount = 6;
            Config.ColorCount = 4;
            Config.BasketCount = 4;

            this.ShowGameWindow();
        }

        private void kcbLevel3_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.Width < 1440)
                Config.TreesCount = 2;
            else
                Config.TreesCount = 3;

            Config.ApplesOnTreeCount = 10;
            Config.ColorCount = 5;
            Config.BasketCount = 6;

            this.ShowGameWindow();
        }

        private void ktbBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
    }
}
