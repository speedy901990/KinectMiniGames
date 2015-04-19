using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace DrawingGame
{
    /// <summary>
    /// Interaction logic for GameOverPopup.xaml
    /// </summary>
    public partial class GameOverPopup : UserControl
    {
        private MainWindow _mw;
        private Timer _timer;

        public GameOverPopup()
        {
            InitializeComponent();
        }

        public GameOverPopup(MainWindow mw)
        {
            _mw = mw;
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            _timer = new Timer();
            _timer.Interval = 3000;
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            _timer.Elapsed -= timer_Elapsed;

            Dispatcher.Invoke(new Action(() =>
            {
                _mw.Close();
            }), null);
        }
    }
}
