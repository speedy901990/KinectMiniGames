using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatabaseManagement.Params;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using System.Timers;

namespace BubblesGame
{
    /// <summary>
    /// Interaction logic for GameOverPopup.xaml
    /// </summary>
    public partial class GameOverPopup : UserControl
    {
        private Timer _timer;

        public bool Ok { get; private set; }

        public GameOverPopup()
        {
            InitializeComponent();
            tblResult.Text = String.Format("Spadły wszystkie bańki");
            Ok = false;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Ok = true;
            }), null);
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            _timer = new Timer {Interval = 3000};
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }
    }
}
