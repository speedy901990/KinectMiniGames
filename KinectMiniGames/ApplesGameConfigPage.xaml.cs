namespace KinectMiniGames
{
    using System.Windows.Controls;
    using ApplesGame;
    /// <summary>
    /// Interaction logic for ApplesGameConfigPage.xaml
    /// </summary>
    public partial class ApplesGameConfigPage : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionDisplay"/> class. 
        /// </summary>
        /// <param name="itemId">ID of the item that was selected</param>
        public ApplesGameConfigPage(string itemId)
        {
            this.InitializeComponent();

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
