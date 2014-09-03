using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Input;
using TrainOfWords.Model;
using TrainOfWords.Resources;

namespace TrainOfWords.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TrainOfWordsGameConfig _config;

        private Game _game;

        public MainWindow()
            : this(new TrainOfWordsGameConfig { Level = 1 })
        {
            
        }

        public MainWindow(TrainOfWordsGameConfig config)
        {
            _config = config;

            InitializeComponent();
            KeyDown += OnKeyDown;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            switch (_config.Level)
            {
                case 1:
                    _game = new FirstLevelGame(_config);
                    MainGrid.Children.Add(new GameView(_game));
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
