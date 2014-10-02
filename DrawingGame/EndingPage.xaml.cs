using System.Windows.Controls;
using TestGame;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Windows;


namespace TestGame // mam tutaj wrzucic polaczenie z kinectem bo nie przekaze go z gry, kod jest w 
    //klasie main window  w minigames, przekazac jakos w konstruktorze czas, pomozyc razy milion wyliczyc pkt, np jako roznice jakiejs duzej liczby i tego, dac ify z przedzialami ocen, do tego menu czy grac dalej czy zakonczyc
{
    /// <summary>
    /// Interaction logic for TestGameConfigPage.xaml
    /// </summary>
    public partial class EndingPage : UserControl
    {
        private KinectSensorChooser sensorChooser;
        public TestGameConfig Config { get; set; }

        public EndingPage(string itemId, KinectSensorChooser kinectSensor)
        {
            this.InitializeComponent();
            setupKinectSensor();
        }

        private void OnLoadedStoryboardCompleted(object sender, System.EventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }


        private void btnBackToMenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = (Panel)this.Parent;
            parent.Children.Remove(this);
        }
    

     #region Kinect discovery + setup
        private static void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        args.NewSensor.DepthStream.Range = DepthRange.Near;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        args.NewSensor.DepthStream.Range = DepthRange.Default;
                        args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }

        private void KinectTileButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (KinectTileButton)e.OriginalSource;
            if (button.Label == "Test Game")
            {
                
                e.Handled = true;
            }
             else if (button.Label == "Bubbles Game")
             {
                 
                 e.Handled = true;
             }
        }
        #endregion
private void setupKinectSensor()
        {   
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.Start();
            var regionSensorBinding = new Binding("Kinect") { Source = this.sensorChooser };
            BindingOperations.SetBinding(this.kinectRegion, KinectRegion.KinectSensorProperty, regionSensorBinding);
        }
        #region Window closing

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sensorChooser.Stop();
        }

        private void key_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
                this.Close();
        }

        private void KinectMiniGames_Deactivated(object sender, EventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
            kinectViewBorder.Visibility = System.Windows.Visibility.Hidden;
            sensorChooserUi.Visibility = System.Windows.Visibility.Hidden;
        }

        #endregion

        private void KinectMiniGames_Activated(object sender, EventArgs e)
        {
            //this.WindowState = WindowState.Maximized;
            kinectViewBorder.Visibility = System.Windows.Visibility.Visible;
            sensorChooserUi.Visibility = System.Windows.Visibility.Visible;
        }
    }
}

