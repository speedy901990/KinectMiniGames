using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace TrainOfWords.View
{
    /// <summary>
    /// Interaction logic for GameOverPopup.xaml
    /// </summary>
    public partial class GameOverPopup : UserControl, IDisposable
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
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_window == null) return;
            _timer = new Timer { Interval = 3000 };
            _timer.Elapsed += timer_Elapsed;
            _timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => _window.Close()), null);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
