using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace LettersGame.View
{
    /// <summary>
    /// Interaction logic for GameOverPopup.xaml
    /// </summary>
    public partial class GameOverPopup : UserControl
    {
        private Timer _timer;
        private readonly Window _window;

        public GameOverPopup()
        {
            InitializeComponent();
        }

        public GameOverPopup(Window window)
        {
            InitializeComponent();
            _window = window;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (_window != null)
            {
                _timer = new Timer {Interval = 3000};
                _timer.Elapsed += timer_Elapsed;
                _timer.Start();
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => _window.Close()), null);
        }
    }
}
