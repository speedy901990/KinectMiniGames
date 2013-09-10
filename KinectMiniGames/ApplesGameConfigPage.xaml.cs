using System.Windows.Controls;
using ApplesGame;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace KinectMiniGames
{
    
    /// <summary>
    /// Interaction logic for ApplesGameConfigPage.xaml
    /// </summary>
    public partial class ApplesGameConfigPage : UserControl
    {
        KinectSensorChooser kinectSensor;
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionDisplay"/> class. 
        /// </summary>
        /// <param name="itemId">ID of the item that was selected</param>
        public ApplesGameConfigPage(string itemId, KinectSensorChooser kinectSensor)
        {
            this.InitializeComponent();
            this.kinectSensor = kinectSensor;

            //this.messageTextBlock.Text = string.Format(CultureInfo.CurrentCulture, KinectMiniGames.Properties.Resources.SelectedMessage, itemId);
        }

        /// <summary>
        /// Called when the OnLoaded storyboard completes.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private void OnLoadedStoryboardCompleted(object sender, System.EventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }

        private void submitApplesConfig_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplesGameConfig config = new ApplesGameConfig();
            config = (ApplesGameConfig)this.FindResource("applesGameConfig");
            config.PassedKinectSensorChooser = this.kinectSensor;
            config.TreesCount = 3;
            if (rbLvl1.IsChecked == true)
            {
                config.ApplesOnTreeCount = 4;
                config.ColorCount = 3;
                config.BasketCount = 3;
            }
            else if (rbLvl2.IsChecked == true)
            {
                config.ApplesOnTreeCount = 6;
                config.ColorCount = 4;
                config.BasketCount = 4;
            }
            else if (rbLvl3.IsChecked == true)
            {
                config.ApplesOnTreeCount = 10;
                config.ColorCount = 5;
                config.BasketCount = 6;
            }  
            ApplesGame.MainWindow window = new ApplesGame.MainWindow(config);
            window.Show();
        }

        private void btnBackToMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
    }
}
