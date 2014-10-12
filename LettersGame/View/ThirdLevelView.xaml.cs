using System;
using System.Drawing;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Brushes = System.Windows.Media.Brushes;

namespace LettersGame.View
{
    /// <summary>
    /// Interaction logic for ThirdLevelView.xaml
    /// </summary>
    public partial class ThirdLevelView
    {
        private readonly LettersGameConfig _config;
        private readonly Game _game;
        private readonly int _letterWidth;
        private readonly int _letterHeight;
        private Letter _selectedLetter;
        private KinectTileButton _selectedLetterButton;
        private Line _linkingLine;
        private bool _drawingEnabled;
        private Timer _endGamePopupTimer;
        private KinectSensorChooser _sensorChooser;
        private bool _isInGripInteraction;

        public ThirdLevelView(LettersGameConfig config)
        {
            _config = config;
            InitializeComponent();
            Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.ApplesGameBackground));
            _game = new Game(config);
            //do sprawdzenia
            Loaded += OnLoaded;
            _isInGripInteraction = false;
            _letterHeight = (int)(_config.WindowHeight / ButtonsGrid.RowDefinitions.Count);
            _letterWidth = (int)(_config.WindowWidth / _config.LettersCount);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _sensorChooser = new KinectSensorChooser();
            _sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            KinectSensorChooserUi.KinectSensorChooser = _sensorChooser;
            _sensorChooser.Start();
            SetGameField();
        }

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }

        private void SetGameField()
        {
            MainCanvas.MouseMove += OnMouseMove;
            MainCanvas.MouseUp += OnMouseUp;
            KinectRegion.AddQueryInteractionStatusHandler(MainCanvas, OnQuery);
            KinectRegion.AddHandPointerMoveHandler(MainCanvas, OnHandPointerMove);
            KinectRegion.AddHandPointerGripHandler(MainCanvas, OnHandPointerGrip);
            KinectRegion.AddHandPointerGripReleaseHandler(MainCanvas, OnHandPointerGripRelease);
            for (var i = 0; i < _config.LettersCount; i++)
            {
                var rand = new Random(Guid.NewGuid().GetHashCode());
                var columnDefinition = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                ButtonsGrid.ColumnDefinitions.Add(columnDefinition);
                var index = rand.Next(_game.BigLetters.Count);
                var bigLetter = new KinectTileButton
                {
                    Tag = _game.BigLetters[index],
                    Content = _game.BigLetters[index].BigLetter,
                    Foreground = new SolidColorBrush(Colors.Purple),
                    Background = new SolidColorBrush(Colors.White),
                    Width = _letterWidth,
                    Height = _letterHeight,
                    FontSize = _config.LettersFontSize,
                    FontWeight = FontWeights.ExtraBold
                };
                bigLetter.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                bigLetter.PreviewMouseLeftButtonUp += OnMouseUp;
                bigLetter.MouseEnter += OnMouseEnter;
                KinectRegion.AddHandPointerGripHandler(bigLetter, OnHandPointerGrip);
                KinectRegion.AddHandPointerGripReleaseHandler(bigLetter, OnHandPointerGripRelease);
                KinectRegion.AddHandPointerMoveHandler(bigLetter, OnHandPointerMove);
                KinectRegion.AddQueryInteractionStatusHandler(bigLetter, OnQuery);
                KinectRegion.AddHandPointerEnterHandler(bigLetter, OnHandPointerEnter);
                ButtonsGrid.Children.Add(bigLetter);
                Grid.SetColumn(bigLetter, i);
                Grid.SetRow(bigLetter, 2);
                _game.BigLetters.RemoveAt(index);

                index = rand.Next(_game.SmallLetters.Count);
                var word = new KinectTileButton
                {
                    Tag = _game.SmallLetters[index],
                    Label = _game.SmallLetters[index].Word,
                    Foreground = new SolidColorBrush(Colors.Purple),
                    Background = _game.SmallLetters[index].Image,
                    Width = _letterWidth,
                    Height = _letterHeight,
                    FontSize = _config.LabelFontSize,
                    FontWeight = FontWeights.Light
                };
                word.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                word.MouseEnter += OnMouseEnter;
                word.PreviewMouseLeftButtonUp += OnMouseUp;
                KinectRegion.AddHandPointerGripHandler(word, OnHandPointerGrip);
                KinectRegion.AddHandPointerGripReleaseHandler(word, OnHandPointerGripRelease);
                KinectRegion.AddHandPointerMoveHandler(word, OnHandPointerMove);
                KinectRegion.AddQueryInteractionStatusHandler(word, OnQuery);
                KinectRegion.AddHandPointerEnterHandler(word, OnHandPointerEnter);
                ButtonsGrid.Children.Add(word);
                Grid.SetColumn(word, i);
                Grid.SetRow(word, 0);
                _game.SmallLetters.RemoveAt(index);
            }
            Grid.SetColumnSpan(MainCanvas, _config.FirstLevelLettersCount);
        }

        #region kinect events
        private void OnHandPointerMove(object sender, HandPointerEventArgs e)
        {
            if (e.HandPointer.IsInGripInteraction)
            {
                if (_linkingLine != null)
                {
                    var point = e.HandPointer.GetPosition(MainCanvas);
                    _linkingLine.X2 = point.X;
                    _linkingLine.Y2 = point.Y;
                }
                e.Handled = true;
            }
        }

        private void OnHandPointerEnter(object sender, HandPointerEventArgs e)
        {
            if (_selectedLetter != null && _selectedLetterButton != null)
            {
                var letterButton = sender as KinectTileButton;
                if (letterButton != null)
                {
                    var letter = letterButton.Tag as Letter;
                    var position = e.HandPointer.GetPosition(MainCanvas);

                    if (letter != null && (!Equals(_selectedLetterButton, letterButton) && _selectedLetter.SmallLetter == letter.SmallLetter))
                    {
                        _selectedLetter = null;
                        _linkingLine.X2 = position.X;
                        _linkingLine.Y2 = position.Y;
                        _linkingLine.Stroke = new SolidColorBrush(Colors.Green);

                        _selectedLetterButton.IsEnabled = false;
                        letterButton.IsEnabled = false;
                        _selectedLetterButton.Foreground = new SolidColorBrush(Colors.Green);
                        letterButton.Foreground = new SolidColorBrush(Colors.Green);
                        NotifySuccess();
                    }
                    else
                    {
                        _selectedLetter = null;
                        _selectedLetterButton = null;
                        MainCanvas.Children.Remove(_linkingLine);
                        NotifyFail();
                    }
                    _linkingLine = null;
                    e.Handled = true;
                }
            }
        }

        private void OnHandPointerGrip(object sender, HandPointerEventArgs e)
        {
            PopupPanel.Children.Clear();
            _selectedLetterButton = sender as KinectTileButton;
            if (_selectedLetterButton != null)
            {
                _selectedLetter = _selectedLetterButton.Tag as Letter;
                if (e.HandPointer.IsInGripInteraction && e.HandPointer.Capture(MainCanvas))
                {
                    var point = e.HandPointer.GetPosition(MainCanvas);
                    _linkingLine = new Line
                    {
                        X1 = point.X,
                        X2 = point.X,
                        Y1 = point.Y,
                        Y2 = point.Y,
                        Stroke = new SolidColorBrush(Colors.Red),
                        StrokeThickness = 10
                    };
                    MainCanvas.Children.Add(_linkingLine);
                    e.Handled = true;
                }
            }
        }

        private void OnHandPointerGripRelease(object sender, HandPointerEventArgs e)
        {
            e.HandPointer.Captured = null;
            if (_selectedLetterButton != null)
                _selectedLetterButton.Visibility = Visibility.Visible;
            if (_drawingEnabled)
            {
                MainCanvas.Children.Remove(_linkingLine);
                e.Handled = true;
            }
        }

        private void OnQuery(object sender, QueryInteractionStatusEventArgs handPointerEventArgs)
        {

            //If a grip detected change the cursor image to grip
            if (handPointerEventArgs.HandPointer.HandEventType == HandEventType.Grip)
            {
                _isInGripInteraction = true;
                handPointerEventArgs.IsInGripInteraction = true;
            }

           //If Grip Release detected change the cursor image to open
            else if (handPointerEventArgs.HandPointer.HandEventType == HandEventType.GripRelease)
            {
                _isInGripInteraction = false;
                handPointerEventArgs.IsInGripInteraction = false;
            }

            //If no change in state do not change the cursor
            else if (handPointerEventArgs.HandPointer.HandEventType == HandEventType.None)
            {
                handPointerEventArgs.IsInGripInteraction = _isInGripInteraction;
            }

            handPointerEventArgs.Handled = true;
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
                if (e.NewSensor.Status == KinectStatus.Connected)
                {
                    KinectUserViewer.KinectSensor = e.NewSensor;
                    MyKinectRegion.KinectSensor = e.NewSensor;
                }
            }
        }

        #endregion

        #region mouse events
        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.ReleaseMouseCapture();
            if (_drawingEnabled)
            {
                MainCanvas.Children.Remove(_linkingLine);
                _drawingEnabled = false;
            }

        }

        void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (_selectedLetter != null && _drawingEnabled)
            {
                var letterButton = sender as KinectTileButton;
                if (letterButton != null)
                {
                    var letter = letterButton.Tag as Letter;
                    var position = e.GetPosition(MainCanvas);

                    if (!Equals(_selectedLetterButton, letterButton) && Equals(_selectedLetter, letter))
                    {
                        _selectedLetter = null;
                        _linkingLine.X2 = position.X;
                        _linkingLine.Y2 = position.Y;
                        _linkingLine.Stroke = new SolidColorBrush(Colors.Green);
                        _drawingEnabled = false;
                        _linkingLine = null;
                        letterButton.IsEnabled = false;
                        _selectedLetterButton.IsEnabled = false;
                        NotifySuccess();
                    }
                    else
                    {
                        _selectedLetter = null;
                        MainCanvas.Children.Remove(_linkingLine);
                        _linkingLine = null;
                        _drawingEnabled = false;
                        NotifyFail();
                    }
                }
            }
        }

        void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _selectedLetterButton = sender as KinectTileButton;
            if (_selectedLetterButton != null) _selectedLetter = _selectedLetterButton.Tag as Letter;
            if (MainCanvas.CaptureMouse())
            {
                _drawingEnabled = true;
                var point = e.GetPosition(MainCanvas);
                _linkingLine = new Line
                {
                    X1 = point.X,
                    X2 = point.X,
                    Y1 = point.Y,
                    Y2 = point.Y,
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 10
                };
                MainCanvas.Children.Add(_linkingLine);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (MainCanvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                if (_linkingLine != null && _drawingEnabled)
                {
                    var point = e.GetPosition(MainCanvas);
                    _linkingLine.X2 = point.X;
                    _linkingLine.Y2 = point.Y;
                }
            }
        }
        #endregion

        #region notifications
        private void NotifySuccess()
        {
            _game.CorrectTrials++;
            _game.LettersLeft--;
            if (_game.LettersLeft == 0)
            {
                EndGame();
            }
            else
            {
                QuickSuccesPopup();
            }
        }

        private void NotifyFail()
        {
            _game.Fails++;
            QuickFailurePopup();
        }

        private void EndGame()
        {
            _game.CalculateTime(DateTime.Now);
            _game.SaveResults();
            var popup = new GameOverPopup
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            ButtonsGrid.Children.Add(popup);
            Grid.SetColumn(popup, 0);
            Grid.SetRow(popup, 1);
            Grid.SetColumnSpan(popup, ButtonsGrid.ColumnDefinitions.Count);
            _endGamePopupTimer = new Timer {Interval = 3000};
            _endGamePopupTimer.Elapsed += endGamePopupTimer_Elapsed;
            _endGamePopupTimer.Start();
        }

        void endGamePopupTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                _sensorChooser.Stop();
                _game.SaveResultsThread.Join();
                var parentGrid = (Grid)Parent;
                var parent = (MainWindow)parentGrid.Parent;
                parent.Close();
            }), null);
        }

        private void QuickSuccesPopup()
        {
            Grid.SetColumnSpan(PopupPanel, ButtonsGrid.ColumnDefinitions.Count);
            var popup = new SmallPopup
            {
                Message = "DOBRZE!",
                PopupColor = Brushes.WhiteSmoke
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
        #endregion
    }
}
