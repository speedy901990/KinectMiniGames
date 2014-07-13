using Microsoft.Kinect.Toolkit.Controls;
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
    /// Interaction logic for ThirdLevelView.xaml
    /// </summary>
    public partial class ThirdLevelView : UserControl
    {
        private LettersGameConfig config;
        private Game game;
        private int letterWidth;
        private int letterHeight;
        private Letter selectedLetter;
        private KinectTileButton selectedLetterButton;
        private Line linkingLine;
        private bool drawingEnabled;
        private Timer endGamePopupTimer;

        public ThirdLevelView()
        {
            InitializeComponent();
        }

        public ThirdLevelView(LettersGameConfig config)
        {
            InitializeComponent();
            this.config = config;
            this.game = new Game(config);
            //do sprawdzenia
            this.letterHeight = (int)(this.config.WindowHeight / buttonsGrid.RowDefinitions.Count);
            this.letterWidth = (int)(this.config.WindowWidth / this.config.LettersCount);
            this.SetGameField();
        }

        private void SetGameField()
        {
            for (int i = 0; i < config.LettersCount; i++)
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                var columnDefinition = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                buttonsGrid.ColumnDefinitions.Add(columnDefinition);
                int index = rand.Next(game.BigLetters.Count);
                var bigLetter = new KinectTileButton
                {
                    Tag = this.game.BigLetters[index],
                    Content = this.game.BigLetters[index].BigLetter,
                    Foreground = new SolidColorBrush(Colors.Purple),
                    Background = new SolidColorBrush(Colors.White),
                    Width = letterWidth,
                    Height = letterHeight,
                    FontSize = this.config.LettersFontSize,
                    FontWeight = FontWeights.ExtraBold
                };
                bigLetter.PreviewMouseLeftButtonDown += letter_MouseDown;
                bigLetter.PreviewMouseLeftButtonUp += letter_MouseUp;
                bigLetter.MouseEnter += letter_MouseEnter;
                buttonsGrid.Children.Add(bigLetter);
                Grid.SetColumn(bigLetter, i);
                Grid.SetRow(bigLetter, 2);
                this.game.BigLetters.RemoveAt(index);

                index = rand.Next(game.SmallLetters.Count);
                var word = new KinectTileButton
                {
                    Tag = this.game.SmallLetters[index],
                    Content = this.game.SmallLetters[index].Word,
                    Foreground = new SolidColorBrush(Colors.Purple),
                    Background = new SolidColorBrush(Colors.White),
                    Width = letterWidth,
                    Height = letterHeight,
                    FontSize = this.config.LettersFontSize,
                    FontWeight = FontWeights.Light
                };
                word.PreviewMouseLeftButtonDown += letter_MouseDown;
                word.MouseEnter += letter_MouseEnter;
                word.PreviewMouseLeftButtonUp += letter_MouseUp;
                buttonsGrid.Children.Add(word);
                Grid.SetColumn(word, i);
                Grid.SetRow(word, 0);
                this.game.SmallLetters.RemoveAt(index);
            }
            Grid.SetColumnSpan(mainCanvas, config.FirstLevelLettersCount);
        }

        void letter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mainCanvas.ReleaseMouseCapture();
            if (this.drawingEnabled)
            {
                mainCanvas.Children.Remove(this.linkingLine);
                this.drawingEnabled = false;
            }

        }

        void letter_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectedLetter != null && this.drawingEnabled)
            {
                var letterButton = sender as KinectTileButton;
                var letter = letterButton.Tag as Letter;
                var position = e.GetPosition(mainCanvas);

                if (this.selectedLetterButton != letterButton && this.selectedLetter == letter)
                {
                    this.selectedLetter = null;
                    this.linkingLine.X2 = position.X;
                    this.linkingLine.Y2 = position.Y;
                    this.linkingLine.Stroke = new SolidColorBrush(Colors.Green);
                    this.drawingEnabled = false;
                    this.linkingLine = null;
                    letterButton.IsEnabled = false;
                    this.selectedLetterButton.IsEnabled = false;
                    this.NotifySuccess();
                }
                else
                {
                    this.selectedLetter = null;
                    mainCanvas.Children.Remove(this.linkingLine);
                    this.linkingLine = null;
                    this.drawingEnabled = false;
                    this.NotifyFail();
                }
            }
        }

        void letter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.selectedLetterButton = sender as KinectTileButton;
            this.selectedLetter = selectedLetterButton.Tag as Letter;
            if (mainCanvas.CaptureMouse())
            {
                var mousePosition = e.GetPosition(mainCanvas);
                this.drawingEnabled = true;
                var point = e.GetPosition(mainCanvas);
                this.linkingLine = new Line
                {
                    X1 = point.X,
                    X2 = point.X,
                    Y1 = point.Y,
                    Y2 = point.Y,
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 10
                };
                mainCanvas.Children.Add(this.linkingLine);
            }
        }

        private void letter_MouseMove(object sender, MouseEventArgs e)
        {
            if (mainCanvas.IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
            {
                if (this.linkingLine != null && this.drawingEnabled)
                {
                    var point = e.GetPosition(mainCanvas);
                    this.linkingLine.X2 = point.X;
                    this.linkingLine.Y2 = point.Y;
                }
            }
        }

        private void NotifySuccess()
        {
            this.game.CorrectTrials++;
            this.game.LettersLeft--;
            if (this.game.LettersLeft == 0)
            {
                this.EndGame();
            }
            else
            {
                this.QuickSuccesPopup();
            }
        }

        private void NotifyFail()
        {
            this.game.Fails++;
            this.QuickFailurePopup();
        }

        private void EndGame()
        {
            this.game.CalculateTime(DateTime.Now);
            this.game.SaveResults();
            var popup = new GameOverPopup
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            buttonsGrid.Children.Add(popup);
            Grid.SetColumn(popup, 0);
            Grid.SetRow(popup, 1);
            Grid.SetColumnSpan(popup, buttonsGrid.ColumnDefinitions.Count);
            this.endGamePopupTimer = new Timer();
            endGamePopupTimer.Interval = 3000;
            endGamePopupTimer.Elapsed += endGamePopupTimer_Elapsed;
            endGamePopupTimer.Start();
        }

        void endGamePopupTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.game.SaveResultsThread.Join();
                var parentGrid = (Grid)this.Parent;
                var parent = (MainWindow)parentGrid.Parent;
                parent.Close();
            }), null);
        }

        private void QuickSuccesPopup()
        {
            Grid.SetColumnSpan(popupPanel, buttonsGrid.ColumnDefinitions.Count);
            var popup = new SmallPopup
            {
                Message = "DOBRZE!",
                PopupColor = Brushes.WhiteSmoke
            };
            popup.Update();
            popupPanel.Children.Add(popup);
        }

        private void QuickFailurePopup()
        {
            Grid.SetColumnSpan(popupPanel, buttonsGrid.ColumnDefinitions.Count);
            var popup = new SmallPopup
            {
                Message = "ŹLE!",
                PopupColor = Brushes.Red
            };
            popup.Update();
            popupPanel.Children.Add(popup);
        }
    }
}
