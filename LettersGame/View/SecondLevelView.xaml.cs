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
    /// Interaction logic for SecondLevelView.xaml
    /// </summary>
    public partial class SecondLevelView : UserControl
    {
        private LettersGameConfig config;
        private Game game;
        private int letterWidth;
        private int letterHeight;
        private int trolleyWidth;
        private int trolleyHeight;
        private Letter selectedLetter;
        private KinectTileButton selectedLetterButton;
        private Timer endGamePopupTimer;

        public SecondLevelView()
        {
            InitializeComponent();
        }

        public SecondLevelView(LettersGameConfig config)
        {
            InitializeComponent();
            this.config = config;
            this.game = new Game(config);
            //do sprawdzenia
            this.letterHeight = (int)(this.config.WindowHeight / 6);
            this.letterWidth = (int)(this.config.WindowWidth / (this.config.LettersCount/2));
            this.SetGameField();
        }

        private void SetGameField()
        {
            for (int i = 0; i < game.Trolleys.Count; i++)
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                var column = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                trolleyGrid.ColumnDefinitions.Add(column);

                //var trolley = new Rectangle
                //{
                //    Tag = game.Trolleys[i],
                //    Width = this.config.WindowWidth / (game.Trolleys.Count + 1),
                //    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                //    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                //    Fill = Brushes.Red // podstawic odpowiednia bitmape z game.Trolleys[i].Image
                //};
                var trolley = new Label 
                { 
                    Content = game.Trolleys[i].BigLetter,
                    Tag = game.Trolleys[i],
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Width = this.config.WindowWidth / (game.Trolleys.Count + 1),
                    FontSize = 200,
                    FontWeight = FontWeights.ExtraBold,
                    Foreground = Brushes.White
                };
                trolley.MouseEnter += trolley_MouseEnter;
                trolleyGrid.Children.Add(trolley);
                Grid.SetColumn(trolley, i);
            }
            foreach (var item in game.SmallLetters)
            {
                var smallLetter = new KinectTileButton
                {
                    Content = item.SmallLetter,
                    Tag = item,
                    Foreground = new SolidColorBrush(Colors.Purple),
                    Background = new SolidColorBrush(Colors.White),
                    Width = letterWidth,
                    Height = letterHeight,
                    FontSize = this.config.LettersFontSize,
                    FontWeight = FontWeights.ExtraBold
                };
                smallLetter.PreviewMouseLeftButtonDown += smallLetter_MouseLeftButtonDown;
                lettersPanel.Children.Add(smallLetter);
            }
        }

        void trolley_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.selectedLetterButton != null)
            {
                var trolley = sender as Label;
                var trolleysLetter = trolley.Tag as Letter;
                if (trolleysLetter.SmallLetter == this.selectedLetter.SmallLetter)
                {
                    this.NotifySuccess();
                    this.selectedLetter = null;
                    this.selectedLetterButton = null;
                }
                else
                {
                    this.NotifyFail();
                    this.selectedLetter = null;
                    this.selectedLetterButton = null;
                }
            }
        }

        void smallLetter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as KinectTileButton;
            this.selectedLetterButton = button;
            this.selectedLetter = (Letter)button.Tag;
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
            rootGrid.Children.Add(popup);
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
