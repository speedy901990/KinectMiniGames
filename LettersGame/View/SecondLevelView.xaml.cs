using System.Drawing;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace LettersGame.View
{
    /// <summary>
    /// Interaction logic for SecondLevelView.xaml
    /// </summary>
    public partial class SecondLevelView
    {
        private readonly LettersGameConfig _config;
        private readonly Game _game;
        private readonly int _letterWidth;
        private readonly int _letterHeight;
        private Letter _selectedLetter;
        private KinectTileButton _selectedLetterButton;
        private Timer _endGamePopupTimer;
        private bool _isInGripInteraction;
        private KinectSensorChooser _sensorChooser;

        public SecondLevelView()
        {
            InitializeComponent();
        }

        public SecondLevelView(LettersGameConfig config)
        {
            InitializeComponent();
            Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.ApplesGameBackground));
            _config = config;
            _game = new Game(config);
            //do sprawdzenia
            _letterHeight = (int)(_config.WindowHeight / 6);
            _letterWidth = (int)(_config.WindowWidth / (_config.LettersCount / 2));
            _isInGripInteraction = false;
            Loaded += OnLoaded;
        }
        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _sensorChooser = new KinectSensorChooser();
            _sensorChooser.KinectChanged += SensorChooserOnKinectChanged;
            KinectSensorChooserUi.KinectSensorChooser = _sensorChooser;
            _sensorChooser.Start();
            SetGameField();
        }

        private void SetGameField()
        {
            KinectRegion.AddQueryInteractionStatusHandler(MyKinectRegion, OnQuery);
            KinectRegion.AddQueryInteractionStatusHandler(MainCanvas, OnQuery);
            KinectRegion.AddQueryInteractionStatusHandler(PopupPanel, OnQuery);
            KinectRegion.AddHandPointerGripHandler(MainCanvas, OnHandPointerGrip);
            KinectRegion.AddHandPointerGripReleaseHandler(MainCanvas, OnHandPointerGripRelease);
            for (var i = 0; i < _game.SmallLetters.Count; i++)
            {
                if (i % 2 == 0)
                {
                    var column = new ColumnDefinition { Width = new GridLength(1.0, GridUnitType.Star) };
                    MainGrid.ColumnDefinitions.Add(column);
                }
                var smallLetter = new KinectTileButton
                {
                    Content = _game.SmallLetters[i].SmallLetter,
                    Tag = _game.SmallLetters[i],
                    Foreground = new SolidColorBrush(Colors.Green),
                    Background = new SolidColorBrush(Colors.White),
                    Width = _letterWidth,
                    Height = _letterHeight,
                    FontSize = _config.LettersFontSize,
                    FontWeight = FontWeights.ExtraBold
                };
                smallLetter.PreviewMouseLeftButtonDown += smallLetter_MouseLeftButtonDown;
                KinectRegion.AddQueryInteractionStatusHandler(smallLetter, OnQuery);
                KinectRegion.AddHandPointerGripHandler(smallLetter, OnHandPointerGrip);
                KinectRegion.AddHandPointerGripReleaseHandler(smallLetter, OnHandPointerGripRelease);
                MainGrid.Children.Add(smallLetter);
                Grid.SetRow(smallLetter, i % 2);
                Grid.SetColumn(smallLetter, i / 2);
            }

            for (var i = 0; i < _game.Trolleys.Count; i++)
            {
                var trolley = new Label
                {
                    Content = _game.Trolleys[i].BigLetter,
                    Tag = _game.Trolleys[i],
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Height = 200,
                    Width = _config.WindowWidth / (_game.Trolleys.Count + 1),
                    Margin = new Thickness{Left = 20, Right = 20},
                    FontSize = 90,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.White,
                    Background = new ImageBrush(ConvertBitmapToBitmapSource(Properties.Resources.basket))
                };
                trolley.MouseEnter += trolley_MouseEnter;
                KinectRegion.AddQueryInteractionStatusHandler(trolley, OnQuery);
                KinectRegion.AddHandPointerEnterHandler(trolley, OnHandPointerEnter);
                KinectRegion.AddHandPointerGripReleaseHandler(trolley, OnHandPointerGripRelease);
                TrolleyPanel.Children.Add(trolley);
                var columnsCount = MainGrid.ColumnDefinitions.Count;
                Grid.SetColumnSpan(TrolleyPanel, columnsCount);
            }
            Grid.SetColumnSpan(PopupPanel, _game.SmallLetters.Count / 2);
        }

        #region kinect changed
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
                    //MessageBox.Show("Kinect Error");
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
        void trolley_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_selectedLetterButton == null) 
                return;
            _selectedLetterButton.Visibility = Visibility.Visible;
            var trolley = sender as Label;
            if (trolley == null) 
                return;
            var trolleysLetter = trolley.Tag as Letter;
            if (trolleysLetter != null && trolleysLetter.SmallLetter == _selectedLetter.SmallLetter)
            {
                _selectedLetterButton.IsEnabled = false;
                NotifySuccess();
                _selectedLetter = null;
                _selectedLetterButton = null;
            }
            else
            {
                NotifyFail();
                _selectedLetter = null;
                _selectedLetterButton = null;
            }
        }

        void smallLetter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as KinectTileButton;

            if (button == null) 
                return;
            _selectedLetterButton = button;
            _selectedLetterButton.Visibility = Visibility.Hidden;
            _selectedLetter = (Letter)button.Tag;
        }
        #endregion

        #region kinect events
        private void OnHandPointerEnter(object sender, HandPointerEventArgs e)
        {
            if (_selectedLetterButton == null) 
                return;
            _selectedLetterButton.Visibility = Visibility.Visible;
            var trolley = sender as Label;
            if (trolley == null) 
                return;
            var trolleysLetter = trolley.Tag as Letter;
            if (trolleysLetter != null && trolleysLetter.SmallLetter == _selectedLetter.SmallLetter)
            {
                _selectedLetterButton.IsEnabled = false;
                NotifySuccess();
                _selectedLetter = null;
                _selectedLetterButton = null;
                e.Handled = true;
            }
            else
            {
                NotifyFail();
                _selectedLetter = null;
                _selectedLetterButton = null;
                e.Handled = true;
            }
        }

        private void OnHandPointerGrip(object sender, HandPointerEventArgs e)
        {
            var button = sender as KinectTileButton;
            if (button == null) 
                return;
            e.HandPointer.Capture(MainCanvas);
            _selectedLetterButton = button;
            _selectedLetterButton.Visibility = Visibility.Hidden;
            _selectedLetter = (Letter)button.Tag;
            e.Handled = true;
        }

        private void OnHandPointerGripRelease(object sender, HandPointerEventArgs e)
        {
            e.HandPointer.Captured = null;
            if (_selectedLetterButton != null)
                _selectedLetterButton.Visibility = Visibility.Visible;
            e.Handled = true;
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
                VerticalAlignment = VerticalAlignment.Center,
            };
            MainGrid.Children.Add(popup);
            Grid.SetColumnSpan(popup, MainGrid.ColumnDefinitions.Count);
            Grid.SetRowSpan(popup, MainGrid.RowDefinitions.Count);
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
            var popup = new SmallPopup
            {
                Message = "DOBRZE!",
                PopupColor = Brushes.WhiteSmoke,
                Size = 150
            };
            popup.Update();
            PopupPanel.Children.Add(popup);
        }

        private void QuickFailurePopup()
        {
            var popup = new SmallPopup
            {
                Message = "ŹLE!",
                PopupColor = Brushes.Red,
                Size = 150
            };
            popup.Update();
            PopupPanel.Children.Add(popup);
        }
        #endregion
    }
}