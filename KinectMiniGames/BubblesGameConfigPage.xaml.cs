using System.Windows.Controls;
using BubblesGame;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace KinectMiniGames
{
    /// <summary>
    /// Interaction logic for BubblesGameConfigPage.xaml
    /// </summary>
    public partial class BubblesGameConfigPage : UserControl
    {
        KinectSensorChooser kinectSensor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionDisplay"/> class. 
        /// </summary>
        /// <param name="itemId">ID of the item that was selected</param>
        public BubblesGameConfigPage(string itemId, KinectSensorChooser kinectSensor)
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

        private void submitBubblesConfig_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BubblesGameConfig config = new BubblesGameConfig();
            config = (BubblesGameConfig)this.FindResource("bubblesGameConfig");
            config.PassedKinectSensorChooser = this.kinectSensor;
            BubblesGame.MainWindow window = new BubblesGame.MainWindow(config);
            window.Show();
        }

        private void btnBackToMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
    }
}
