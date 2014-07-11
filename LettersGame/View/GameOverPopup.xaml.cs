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
        private Timer timer;
        private Window window;

        public GameOverPopup()
        {
            InitializeComponent();
        }

        public GameOverPopup(Window window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (window != null)
            {
                this.timer = new Timer();
                timer.Interval = 3000;
                timer.Elapsed += timer_Elapsed;
                timer.Start();
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.window.Close();
            }), null);
        }
    }
}
