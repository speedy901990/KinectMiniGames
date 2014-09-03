using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Kinect;
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

        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs e)
        {
            if (e.OldSensor != null)
            {
                try
                {
                    e.OldSensor.DepthStream.Range = DepthRange.Default;
                    e.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    e.OldSensor.DepthStream.Disable();
                    e.OldSensor.SkeletonStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (e.NewSensor != null)
            {
                try
                {
                    e.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    e.NewSensor.SkeletonStream.Enable();

                    try
                    {
                        e.NewSensor.DepthStream.Range = DepthRange.Near;
                        e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                        e.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    }
                    catch (InvalidOperationException)
                    {
                        // Non Kinect for Windows devices do not support Near mode, so reset back to default mode.
                        e.NewSensor.DepthStream.Range = DepthRange.Default;
                        e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    }
                }
                catch (InvalidOperationException)
                {
                    //MessageBox.Show("Kinect Setup Error");
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
                if (e.NewSensor.Status != KinectStatus.Connected) return;
                KinectUserViewer.KinectSensor = e.NewSensor;
                MyKinectRegion.KinectSensor = e.NewSensor;
            }
        }

        private void SetFirstStep()
        {
            KinectRegion.AddQueryInteractionStatusHandler(MyKinectRegion, OnQuery);
            KinectRegion.AddQueryInteractionStatusHandler(MainCanvas, OnQuery);
            KinectRegion.AddQueryInteractionStatusHandler(PopupPanel, OnQuery);
            KinectRegion.AddHandPointerGripHandler(MainCanvas, OnGrip);
            KinectRegion.AddHandPointerGripReleaseHandler(MainCanvas, OnGripRelease);
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
                KinectRegion.AddQueryInteractionStatusHandler(letterButton, OnQuery);
                KinectRegion.AddHandPointerGripHandler(letterButton, OnGrip);
                KinectRegion.AddHandPointerGripReleaseHandler(letterButton, OnGripRelease);
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
            KinectRegion.AddQueryInteractionStatusHandler(train, OnQuery);
            KinectRegion.AddHandPointerEnterHandler(train, OnHandPointerEnter);
            KinectRegion.AddHandPointerGripReleaseHandler(train, OnGripRelease);
            ButtonsGrid.Children.Add(train);
            Grid.SetRow(train,2);
            Grid.SetColumnSpan(train, _game.Letters.Count / 2);
        }

        private void OnGripRelease(object sender, HandPointerEventArgs e)
        {
            e.HandPointer.Captured = null;
            e.Handled = true;
        }

        private void OnGrip(object sender, HandPointerEventArgs e)
        {
            e.HandPointer.Capture(MainCanvas);
            var button = sender as KinectTileButton;
            if (button == null) return;
            var letter = button.Tag as string;
            if (letter == null) return;
            _selectedLetter = letter;
            _selectedLetterButton = button;
            e.Handled = true;
        }

        private void OnHandPointerEnter(object sender, HandPointerEventArgs e)
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
                _selectedLetterButton.Foreground = new SolidColorBrush(Colors.Green);
                NotifySuccess();
                _selectedLetter = null;
                _selectedLetterButton = null;
                return;
            }
            NotifyFail();
            _selectedLetter = null;
            _selectedLetterButton = null;
        }

        private void OnQuery(object sender, QueryInteractionStatusEventArgs e)
        {
            switch (e.HandPointer.HandEventType)
            {
                case HandEventType.Grip:
                    _isInGripInteraction = true;
                    e.IsInGripInteraction = true;
                    break;
                case HandEventType.GripRelease:
                    _isInGripInteraction = false;
                    e.IsInGripInteraction = false;
                    break;
                case HandEventType.None:
                    e.IsInGripInteraction = _isInGripInteraction;
                    break;
            }
            e.Handled = true;
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
            var endGamePopupTimer = new Timer { Interval = 3000 };
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
