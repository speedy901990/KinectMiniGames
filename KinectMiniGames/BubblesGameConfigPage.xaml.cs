using System.Windows.Controls;
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

        private void submitBubblesConfig_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.kinectSensor.Stop();
            BubblesGame.MainWindow window = new BubblesGame.MainWindow(Config);
            window.Show();
            App.Current.MainWindow.Close();            
        }

        private void btnBackToMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
    }
}
