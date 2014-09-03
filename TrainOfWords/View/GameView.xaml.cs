using System;
using System.Linq;
using System.Timers;
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

        private string _selectedLetter;

        private KinectTileButton _selectedLetterButton;

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

        private void LetterButtonOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var button = sender as KinectTileButton;
            if (button == null) return;
            var letter = button.Tag as string;
            if (letter == null) return;
            _selectedLetter = letter;
            _selectedLetterButton = button;
        }

        private void TrainOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            if (_selectedLetterButton == null) return;
            var train = sender as KinectTileButton;
            if (train == null) return;
            var word = train.Tag as Word;
            if (word == null) return;
            if (word.Letters.Contains(_selectedLetter))
            {
                word.Letters.Remove(_selectedLetter);
                _selectedLetterButton.IsEnabled = false;
                NotifySuccess();
                return;
            }
            NotifyFail();
        }

        private void NotifySuccess()
        {
            _game.Score.CorrectTrials++;
            _game.Config.AllLettersCount--;
            if (!_game.Words[0].Letters.Any() && !_game.FirstStepFinished)
            {
                _game.FirstStepFinished = true;
                EndGame();
                return;
                //przejdz do drugiego
            }
            if (!_game.Words[1].Letters.Any() && !_game.SecondStepFinished)
            {
                _game.SecondStepFinished = true;
                return;
                //przejdz do trzeciego
            }
            if (!_game.Words[2].Letters.Any() && !_game.ThirdStepFinished)
            {
                _game.ThirdStepFinished = true;
                EndGame();
                //zakoncz
            }
            QuickSuccessPopup();
        }

        private void NotifyFail()
        {
            _game.Score.Failures++;
            QuickFailurePopup();
        }

        private void EndGame()
        {
            _game.SaveResult();
            var popup = new GameOverPopup
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            ButtonsGrid.Children.Add(popup);
            Grid.SetColumn(popup, 0);
            Grid.SetRow(popup, 0);
            Grid.SetColumnSpan(popup, ButtonsGrid.ColumnDefinitions.Count);
            Grid.SetRowSpan(popup, ButtonsGrid.RowDefinitions.Count);
            var endGamePopupTimer = new System.Timers.Timer { Interval = 3000 };
            endGamePopupTimer.Elapsed += EndGamePopupTimerOnElapsed;
            endGamePopupTimer.Start();
        }

        private void EndGamePopupTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                //_sensorChooser.Kinect.Stop();
                _sensorChooser.Stop();
                //_game.SaveResultsThread.Join();
                var parentGrid = (Grid)Parent;
                var parent = (MainWindow)parentGrid.Parent;
                parent.Close();
            }), null);
        }

        private void QuickSuccessPopup()
        {
            Grid.SetColumnSpan(PopupPanel, ButtonsGrid.ColumnDefinitions.Count);
            var popup = new SmallPopup
            {
                Message = "DOBRZE!",
                PopupColor = Brushes.Green
            };
            popup.Update();
            PopupPanel.Children.Add(popup);
        }

        private void QuickFailurePopup()
        {
            Grid.SetColumnSpan(PopupPanel, ButtonsGrid.ColumnDefinitions.Count);
            var popup = new SmallPopup
            {
                Message = "ŹLE!",
                PopupColor = Brushes.Red
            };
            popup.Update();
            PopupPanel.Children.Add(popup);
        }
    }
}
