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
        private bool ok;
        private Timer timer;


        public bool Ok
        {
            get { return ok; }
            private set { ok = value; }
        }

        public GameOverPopup()
        {
            InitializeComponent();
            tblResult.Text = "Liczba zbitych baniek: " + GameWindow.bubblesPopped;
            this.Ok = false;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Ok = true;
            }), null);
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.timer = new Timer();
            timer.Interval = 3000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }
    }
}
