﻿using System.Windows.Controls;
using BubblesGame;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace KinectMiniGames
{
    public partial class BubblesGameConfigPage : UserControl
    {
        KinectSensorChooser kinectSensor;
        public BubblesGameConfig Config { get; set; }

        public BubblesGameConfigPage(string itemId, KinectSensorChooser kinectSensor)
        {
            this.InitializeComponent();
            this.kinectSensor = kinectSensor;
            Config = new BubblesGameConfig();
            this.DataContext = Config;
        }

        private void OnLoadedStoryboardCompleted(object sender, System.EventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }

        private void RunGame()
        {
            this.kinectSensor.Stop();
            BubblesGame.MainWindow window = new BubblesGame.MainWindow(Config);
            window.Show();  
        }

        private void kcbLevel1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //username and surname will be choosen from database soon but not yet
            Config.Username = "Gracz";
            Config.UserSurname = "Testowy";
            Config.BubblesFallSpeed = 1;
            Config.BubblesApperanceFrequency = 1;
            Config.BubblesCount = 20;
            Config.BubblesSize = 90;
            this.RunGame();
        }

        private void kcbLevel2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //username and surname will be choosen from database soon but not yet
            Config.Username = "Gracz";
            Config.UserSurname = "Testowy";
            Config.BubblesFallSpeed = 2;
            Config.BubblesApperanceFrequency = 2;
            Config.BubblesCount = 40;
            Config.BubblesSize = 70;
            this.RunGame();
        }

        private void kcbLevel3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //username and surname will be choosen from database soon but not yet
            Config.Username = "Gracz";
            Config.UserSurname = "Testowy";
            Config.BubblesFallSpeed = 3;
            Config.BubblesApperanceFrequency = 3;
            Config.BubblesCount = 60;
            Config.BubblesSize = 50;
            this.RunGame();
        }

        private void kcbLevel4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //username and surname will be choosen from database soon but not yet
            Config.Username = "Gracz";
            Config.UserSurname = "Testowy";
            Config.BubblesFallSpeed = 5;
            Config.BubblesApperanceFrequency = 5;
            Config.BubblesCount = 60;
            Config.BubblesSize = 40;
            this.RunGame();
        }

        private void ktbBackToMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
    }
}