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
    /// Interaction logic for SmallPopup.xaml
    /// </summary>
    public partial class SmallPopup : UserControl
    {
        private Timer _timer;

        public SmallPopup()
        {
            InitializeComponent();
        }

        public void Update()
        {
            LbContent.Content = Message;
            LbContent.Foreground = PopupColor;
            if (Size != 0)
            {
                LbContent.FontSize = Size;
            }
        }

        public Brush PopupColor { get; set; }

        public string Message { get; set; }

        public int Size { get; set; }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            _timer = new Timer {Interval = 1500};
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var parent = Parent as Panel;
                if (parent != null)
                {
                    parent.Children.Remove(this);
                }
                _timer.Stop();
            }), null);
        }
    }
}
