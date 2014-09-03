using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using TrainOfWords.Model;

namespace TrainOfWords.View
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        private readonly Game _game;

        private KinectSensorChooser _sensorChooser;

        private bool _isInGripInteraction;

        private string selectedLetter;

        private KinectTileButton selectedLetterButton;

        public GameView() : this(new FirstLevelGame(new TrainOfWordsGameConfig{Level = 1}))
        {}

        public GameView(Game game)
        {
            InitializeComponent();
            Loaded += OnLoaded;
            _game = game;
            _isInGripInteraction = false;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _sensorChooser = new KinectSensorChooser();
            _sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            _sensorChooser.Start();
            KinectSensorChooserUi.KinectSensorChooser = _sensorChooser;
            SetFirstStep();
        }

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs kinectChangedEventArgs)
        {
            throw new NotImplementedException();
        }

        private void SetFirstStep()
        {
            var word = _game.Words[0];
            for (var i = 0; i < _game.Letters.Count/2 ; i++)
            {
                var column = new ColumnDefinition {Width = new GridLength(1.0, GridUnitType.Star)};
                ButtonsGrid.ColumnDefinitions.Add(column);
            }
            var j = 0;
            foreach (var letter in _game.Letters[word.Name])
            {
                var letterButton = new KinectTileButton
                {
                    Tag = letter,
                    Content = letter,
                    Foreground = new SolidColorBrush(Colors.BlueViolet),
                    Background = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.ExtraBold,
                    FontSize = 100,
                    Height = 150
                };
                letterButton.PreviewMouseLeftButtonDown += LetterButtonOnMouseLeftButtonDown;
                //kinectowe eventy
                LettersPanel.Children.Add(letterButton);
                Grid.SetRow(letterButton, j % 2);
                Grid.SetColumn(letterButton, j / 2);
                j++;
            }

            var train = new KinectTileButton
            {
                Tag = word,
                Label = word.Name,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            train.MouseEnter += TrainOnMouseEnter;
            //kinectowe eventy
            ButtonsGrid.Children.Add(train);
            Grid.SetRow(train,2);
            Grid.SetColumnSpan(train, _game.Letters.Count / 2);
        }

        private void TrainOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (selectedLetterButton == null) return;
            var train = sender as KinectTileButton;
            if (train == null) return;
            var word = train.Tag as Word;
            
            if (word == null) return;
            if (word.Letters.Contains(selectedLetter))
            {
                word.Letters.Remove(selectedLetter);
                selectedLetterButton.IsEnabled = false;
                //notify success
            }
        }

        private void LetterButtonOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var button = sender as KinectTileButton;
            if (button == null) return;
            var letter = button.Tag as string;
            if (letter == null) return;
            selectedLetter = letter;
            selectedLetterButton = button;
        }
    }
}
