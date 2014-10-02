using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrawingGame
{
    /// <summary>
    /// Interaction logic for GameOverPopup.xaml
    /// </summary>
    public partial class GameOverPopup : UserControl
    {
        private MainWindow mw;
        private Timer timer;

        public GameOverPopup()
        {
            InitializeComponent();
        }

        public GameOverPopup(MainWindow mw)
        {
            this.mw = mw;
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.timer = new Timer();
            timer.Interval = 3000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            timer.Elapsed -= timer_Elapsed;

            this.Dispatcher.Invoke(new Action(() =>
            {
                this.mw.Close();
            }), null);
        }
    }
}
