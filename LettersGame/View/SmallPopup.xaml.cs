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
        private Timer timer;

        public SmallPopup()
        {
            InitializeComponent();
        }

        public void Update()
        {
            lbContent.Content = this.Message;
            lbContent.Foreground = this.PopupColor;
        }

        private Brush popupColor;

        public Brush PopupColor
        {
            get { return popupColor; }
            set { popupColor = value; }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            timer = new Timer();
            timer.Interval = 1500;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                var parent = this.Parent as Panel;
                if (parent != null)
                {
                    parent.Children.Remove(this);
                }
                this.timer.Stop();
            }), null);
        }
    }
}
