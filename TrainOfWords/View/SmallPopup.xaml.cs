using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TrainOfWords.View
{
    /// <summary>
    /// Interaction logic for SmallPopup.xaml
    /// </summary>
    public partial class SmallPopup : UserControl, IDisposable
    {
        private Timer _timer;

        public SmallPopup()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public void Update()
        {
            LbContent.Content = Message;
            LbContent.Foreground = PopupColor;
            if (Size != 0)
                LbContent.FontSize = Size;
        }

        public Brush PopupColor { get; set; }

        public string Message { get; set; }

        public int Size { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _timer = new Timer { Interval = 1500 };
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var parent = Parent as Panel;
                if (parent != null)
                    parent.Children.Remove(this);
                _timer.Stop();
            }), null);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
