using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Samples.Kinect.WpfViewers;
using System.Windows.Interop;
using System.Diagnostics;
using System.Threading;
using DatabaseManagement.Managers;
using DatabaseManagement.Params;





namespace DrawingGame
{
    public partial class MainWindow : Window
    {

        #region Member Variables

        Stopwatch stopwatchOfGame;
        Stopwatch stopwatchOfOutOfField; 
        uint time = 0;
        private bool gameOver = false;
        private bool _currentHand;
        private DrawingGameConfig _Configuration;
        Points points;
        private KinectSensor _KinectDevice;
        private readonly Brush[] _SkeletonBrushes;
        private Skeleton[] _FrameSkeletons;
        private List<Polyline> _Figures = new List<Polyline>();
        private List<DotPuzzle> _ListPuzzleL = new List<DotPuzzle>();
        private List<DotPuzzle> _ListPuzzleR = new List<DotPuzzle>();
        private int _PuzzleDotIndexL;
        private int _PuzzleDotIndexR;
        private Player _Player;
        private Rect _playerBounds = new Rect();
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        int precision;
        private Thread saveResults;
        #endregion Member Variables

        #region Constructor
        public MainWindow(DrawingGameConfig config)
        {
            stopwatchOfGame = new Stopwatch();
            stopwatchOfOutOfField = new Stopwatch(); 

            _Configuration = config;
            InitializeComponent();
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;

            this._SkeletonBrushes = new[] { Brushes.Green, Brushes.Blue, Brushes.Red, Brushes.Orange };
        }


        #endregion Constructor

        #region Methods

        private void EndGame()
        {
            

            stopwatchOfGame.Stop();

           
            this.saveResults = new Thread(SaveResults);
            this.saveResults.Start();
            gameOver = true;
            GameOverPopup popup = new GameOverPopup(this);
            popup.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            popup.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            Canvas.SetTop(popup, this.Height / 2);
            Canvas.SetLeft(popup, this.Width/2);
            EndPopUpCanvas.Children.Add(popup);
        }

        private void SaveResults()
        {
            int level = 0;

            if (_Configuration.HandsState == 1 && _Configuration.Difficulty == 1)
                level = 1;
            if (_Configuration.HandsState == 2 && _Configuration.Difficulty == 1)
                level = 2;
            if (_Configuration.HandsState == 1 && _Configuration.Difficulty == 2)
                level = 3;
            if (_Configuration.HandsState == 2 && _Configuration.Difficulty == 2)
                level = 4;

            DrawingGameManager manager = new DrawingGameManager(this._Configuration.Player);
            DrawingGameParams gameParams = new DrawingGameParams
            {
             TimeOfGame =(int)Math.Round((double)stopwatchOfGame.ElapsedMilliseconds/1000),
             TimeOutOfField = (int)Math.Round((double)stopwatchOfOutOfField.ElapsedMilliseconds / 1000),
             Level = level
            };
            manager.SaveGameResult(gameParams);
        }
        private void DiscoverKinectSensor()
        {

            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this._KinectDevice = KinectSensor.KinectSensors
            .FirstOrDefault(x => x.Status == KinectStatus.Connected);
            this._KinectDevice.Start();
        }

        private void PrepareGame()
        {

            LoadLevels();

            this._PuzzleDotIndexL = -1;
            this._PuzzleDotIndexR = -1;
            this._currentHand = false;

            if (_Configuration.Precision == 2)
            {
                precision = 50;
            }
            else if (_Configuration.Precision == 1)
            {
                precision = 90;
            }



            if (_Configuration.HandsState == 2)
            {
                DrawPuzzle(_currentHand);
                DrawPuzzle(!_currentHand);

            }
            if (_Configuration.HandsState == 1)
            {
                DrawPuzzle(_currentHand);
                
            }

            stopwatchOfGame.Start();
        }

        private void LoadLevels()
        {
            double centerX = (double)this.Width / 2;
            double centerY = (double)this.Height / 2;


            DotPuzzle FirstLeftHand = new DotPuzzle();
            DotPuzzle SecondLeftHand = new DotPuzzle();
            DotPuzzle ThirdLeftHand = new DotPuzzle();
            DotPuzzle FourthLeftHand = new DotPuzzle();
            DotPuzzle FifthLeftHand = new DotPuzzle();
            DotPuzzle SixthLeftHand = new DotPuzzle();
            DotPuzzle SeventhLeftHand = new DotPuzzle();
            DotPuzzle EignthLeftHand = new DotPuzzle();
            DotPuzzle NinthLeftHand = new DotPuzzle();
            DotPuzzle TenthLeftHand = new DotPuzzle();

            if (_Configuration.Difficulty == 1)//easy
            {
                #region Easy Levels Data For Left Hand
                FirstLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (50.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (60.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (80.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (90.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (110.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (130.0 / 100.0)));


                SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (60.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (100.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (110.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (130.0 / 100.0)));

                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (120.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (140.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (150.0 / 100.0)));

                FourthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (50.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (120.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (140.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (150.0 / 100.0)));

                FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (110.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (140.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (150.0 / 100.0)));

                SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (100.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (120.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (130.0 / 100.0)));

                SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (60.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));

                EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (60.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (80.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (90.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (110.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));

                NinthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (50.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (60.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (70.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));


                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (90.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (140.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (150.0 / 100.0)));
                #endregion

            }
            else if (_Configuration.Difficulty == 2)
            {
                #region Medium Levels Data For Left Hand
                FirstLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (50.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (50.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (70.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (90.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (110.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (130.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (130.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (150.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (100.0 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80 / 100.0)));
                FirstLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (60.0 / 100.0)));


                SecondLeftHand.Dots.Add(new Point(centerX * (65.0 / 100.0), centerY * (50.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (55.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (70.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (90.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (110.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (100.0 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80 / 100.0)));
                SecondLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (60.0 / 100.0)));

                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (50.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (70.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (90.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (110.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (130.0 / 100.0)));
                ThirdLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (130.0 / 100.0)));


                FourthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (50.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (50.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (90.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (120.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (100.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (100.0 / 100.0)));
                FourthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));


                FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (60.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (65.0 / 100.0), centerY * (70.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (65.0 / 100.0), centerY * (80.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (100.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (110.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (120.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (110.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (100.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (90.0 / 100.0)));
                FifthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (80.0 / 100.0)));

                SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (100.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (120.0 / 100.0)));
                SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (130.0 / 100.0)));

                SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (60.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
                SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));

                EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (60.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (80.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (90.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (110.0 / 100.0)));
                EignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));

                NinthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (50.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (60.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (70.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (80.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
                NinthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));


                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (90.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (140.0 / 100.0)));
                TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (150.0 / 100.0)));
                #endregion

            }
            //else if (_Configuration.Difficulty == 3)//hard
            //{
            //    #region Hard Levels Data For Left Hand
            //    FirstLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (50.0 / 100.0)));
            //    FirstLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (60.0 / 100.0)));
            //    FirstLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (80.0 / 100.0)));
            //    FirstLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (90.0 / 100.0)));
            //    FirstLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (110.0 / 100.0)));
            //    FirstLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));
            //    FirstLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (130.0 / 100.0)));


            //    SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (60.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (100.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (110.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
            //    SecondLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (130.0 / 100.0)));

            //    ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
            //    ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
            //    ThirdLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
            //    ThirdLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (120.0 / 100.0)));
            //    ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (140.0 / 100.0)));
            //    ThirdLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (150.0 / 100.0)));

            //    FourthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (50.0 / 100.0)));
            //    FourthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
            //    FourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
            //    FourthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
            //    FourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (120.0 / 100.0)));
            //    FourthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (140.0 / 100.0)));
            //    FourthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (150.0 / 100.0)));

            //    FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
            //    FifthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
            //    FifthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (110.0 / 100.0)));
            //    FifthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
            //    FifthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (140.0 / 100.0)));
            //    FifthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (150.0 / 100.0)));

            //    SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
            //    SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (90.0 / 100.0)));
            //    SixthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (100.0 / 100.0)));
            //    SixthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (120.0 / 100.0)));
            //    SixthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (130.0 / 100.0)));

            //    SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    SeventhLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (60.0 / 100.0)));
            //    SeventhLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
            //    SeventhLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (80.0 / 100.0)));
            //    SeventhLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
            //    SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (110.0 / 100.0)));
            //    SeventhLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));

            //    EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (60.0 / 100.0)));
            //    EignthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (70.0 / 100.0)));
            //    EignthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (80.0 / 100.0)));
            //    EignthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (90.0 / 100.0)));
            //    EignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (110.0 / 100.0)));
            //    EignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));

            //    NinthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (50.0 / 100.0)));
            //    NinthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (60.0 / 100.0)));
            //    NinthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (70.0 / 100.0)));
            //    NinthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (80.0 / 100.0)));
            //    NinthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (90.0 / 100.0)));
            //    NinthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
            //    NinthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (120.0 / 100.0)));


            //    TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (50.0 / 100.0)));
            //    TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (70.0 / 100.0)));
            //    TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (90.0 / 100.0)));
            //    TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
            //    TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (120.0 / 100.0)));
            //    TenthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (140.0 / 100.0)));
            //    TenthLeftHand.Dots.Add(new Point(centerX * (60.0 / 100.0), centerY * (150.0 / 100.0)));
            //    #endregion

            //}


            this._ListPuzzleL.Add(FirstLeftHand);
            this._ListPuzzleL.Add(SecondLeftHand);
            this._ListPuzzleL.Add(ThirdLeftHand);
            this._ListPuzzleL.Add(FourthLeftHand);
            this._ListPuzzleL.Add(FifthLeftHand);
            this._ListPuzzleL.Add(SixthLeftHand);
            this._ListPuzzleL.Add(SeventhLeftHand);
            this._ListPuzzleL.Add(EignthLeftHand);
            this._ListPuzzleL.Add(NinthLeftHand);
            this._ListPuzzleL.Add(TenthLeftHand);

            {
                DotPuzzle FirstRightHand = new DotPuzzle();
                DotPuzzle SecondRightHand = new DotPuzzle();
                DotPuzzle ThirdRightHand = new DotPuzzle();
                DotPuzzle FourthRightHand = new DotPuzzle();
                DotPuzzle FifthRightHand = new DotPuzzle();
                DotPuzzle SixthRightHand = new DotPuzzle();
                DotPuzzle SeventhRightHand = new DotPuzzle();
                DotPuzzle EignthRightHand = new DotPuzzle();
                DotPuzzle NinthRightHand = new DotPuzzle();
                DotPuzzle TenthRightHand = new DotPuzzle();


                if (_Configuration.Difficulty == 1)//easy
                {
                    #region Easy Levels Data For Right Hand
                    FirstRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (50.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (60.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (80.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (90.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (110.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (130.0 / 100.0)));


                    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (60.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (100.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (110.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (130.0 / 100.0)));

                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (120.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (140.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (150.0 / 100.0)));

                    FourthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (50.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (120.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (140.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (150.0 / 100.0)));

                    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (110.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (140.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (150.0 / 100.0)));

                    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (100.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (120.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (130.0 / 100.0)));

                    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (60.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));

                    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (60.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (80.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (90.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (110.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));

                    NinthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (50.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (60.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (70.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));


                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (90.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (140.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (150.0 / 100.0)));
                    #endregion

                }
                else if (_Configuration.Difficulty == 2)
                {
                    #region Medium Levels Data For Right Hand
                    FirstRightHand.Dots.Add(new Point(centerX * (139.0 / 100.0), centerY * (50.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (50.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (70.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (90.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (110.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (130.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (130.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (150.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (100.0 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80 / 100.0)));
                    FirstRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (60.0 / 100.0)));


                    SecondRightHand.Dots.Add(new Point(centerX * (135.0 / 100.0), centerY * (50.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (55.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (70.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (90.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (110.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (100.0 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80 / 100.0)));
                    SecondRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (60.0 / 100.0)));

                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (50.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (70.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (90.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (110.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (130.0 / 100.0)));
                    ThirdRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (130.0 / 100.0)));


                    FourthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (50.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (50.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (90.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (120.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (100.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (100.0 / 100.0)));
                    FourthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));


                    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (60.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (135.0 / 100.0), centerY * (70.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (135.0 / 100.0), centerY * (80.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (100.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (110.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (120.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (110.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (100.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (90.0 / 100.0)));
                    FifthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (80.0 / 100.0)));

                    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (100.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (120.0 / 100.0)));
                    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (130.0 / 100.0)));

                    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (60.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));

                    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (60.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (80.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (90.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (110.0 / 100.0)));
                    EignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));

                    NinthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (50.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (60.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (70.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (80.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                    NinthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));


                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (90.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (140.0 / 100.0)));
                    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (150.0 / 100.0)));
                    #endregion
                }
                //else if (_Configuration.Difficulty == 3)//hard
                //{
                //    #region Hard Levels Data For Right Hand
                //    FirstRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (50.0 / 100.0)));
                //    FirstRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (60.0 / 100.0)));
                //    FirstRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (80.0 / 100.0)));
                //    FirstRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (90.0 / 100.0)));
                //    FirstRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (110.0 / 100.0)));
                //    FirstRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));
                //    FirstRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (130.0 / 100.0)));


                //    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (60.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (100.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (110.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                //    SecondRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (130.0 / 100.0)));

                //    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                //    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                //    ThirdRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                //    ThirdRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (120.0 / 100.0)));
                //    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (140.0 / 100.0)));
                //    ThirdRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (150.0 / 100.0)));

                //    FourthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (50.0 / 100.0)));
                //    FourthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                //    FourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                //    FourthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                //    FourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (120.0 / 100.0)));
                //    FourthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (140.0 / 100.0)));
                //    FourthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (150.0 / 100.0)));

                //    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                //    FifthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                //    FifthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (110.0 / 100.0)));
                //    FifthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                //    FifthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (140.0 / 100.0)));
                //    FifthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (150.0 / 100.0)));

                //    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                //    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (90.0 / 100.0)));
                //    SixthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (100.0 / 100.0)));
                //    SixthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (120.0 / 100.0)));
                //    SixthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (130.0 / 100.0)));

                //    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    SeventhRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (60.0 / 100.0)));
                //    SeventhRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                //    SeventhRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (80.0 / 100.0)));
                //    SeventhRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                //    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (110.0 / 100.0)));
                //    SeventhRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));

                //    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (60.0 / 100.0)));
                //    EignthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (70.0 / 100.0)));
                //    EignthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (80.0 / 100.0)));
                //    EignthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (90.0 / 100.0)));
                //    EignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (110.0 / 100.0)));
                //    EignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));

                //    NinthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (50.0 / 100.0)));
                //    NinthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (60.0 / 100.0)));
                //    NinthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (70.0 / 100.0)));
                //    NinthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (80.0 / 100.0)));
                //    NinthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (90.0 / 100.0)));
                //    NinthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                //    NinthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (120.0 / 100.0)));


                //    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (50.0 / 100.0)));
                //    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (70.0 / 100.0)));
                //    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (90.0 / 100.0)));
                //    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                //    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (120.0 / 100.0)));
                //    TenthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (140.0 / 100.0)));
                //    TenthRightHand.Dots.Add(new Point(centerX * (140.0 / 100.0), centerY * (150.0 / 100.0)));
                //    #endregion
                //}


                this._ListPuzzleR.Add(FirstRightHand);
                this._ListPuzzleR.Add(SecondRightHand);
                this._ListPuzzleR.Add(ThirdRightHand);
                this._ListPuzzleR.Add(FourthRightHand);
                this._ListPuzzleR.Add(FifthRightHand);
                this._ListPuzzleR.Add(SixthRightHand);
                this._ListPuzzleR.Add(SeventhRightHand);
                this._ListPuzzleR.Add(EignthRightHand);
                this._ListPuzzleR.Add(NinthRightHand);
                this._ListPuzzleR.Add(TenthRightHand);


            }
        }

        private void DrawPuzzle(DotPuzzle puzzle, Canvas ColorPointCanvas, Polyline FigurePolyline)
        {

            try
            {
                if (puzzle != null)
                {
                    ColorPointCanvas.Children.Clear();
                    FigurePolyline.Points.Clear();
                    for (int i = 0; i < puzzle.Dots.Count; i++)
                    {
                        Grid dotContainer = new Grid();
                        dotContainer.Width = 80;
                        dotContainer.Height = 80;
                        if (i == 0 || i == puzzle.Dots.Count - 1)
                        {
                            if (i == 0)
                                dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.greenBall))) }); // do punktu startowego i koncowego potrzebna grafika, najlepiej jakas animacja
                            if (i == puzzle.Dots.Count - 1)
                                dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.redBall))) });
                            

                            TextBlock dotLabel = new TextBlock();
                            //dotLabel.Text = (i + 1).ToString();
                            dotLabel.Foreground = Brushes.White;
                            dotLabel.FontSize = 35;
                            dotLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            dotLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            dotContainer.Children.Add(dotLabel);

                            Canvas.SetTop(dotContainer, puzzle.Dots[i].Y - (dotContainer.Height / 2));
                            Canvas.SetLeft(dotContainer, puzzle.Dots[i].X - (dotContainer.Width / 2));
                            ColorPointCanvas.Children.Add(dotContainer);

                        }
                        else
                            dotContainer.Children.Add(new Ellipse() { Fill = FigurePolyline.Stroke });

                        //TextBlock dotLabel = new TextBlock();
                        ////dotLabel.Text = (i + 1).ToString();
                        //dotLabel.Foreground = Brushes.White;
                        //dotLabel.FontSize = 35;
                        //dotLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        //dotLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        //dotContainer.Children.Add(dotLabel);

                        //Canvas.SetTop(dotContainer, puzzle.Dots[i].Y - (dotContainer.Height / 2));
                        //Canvas.SetLeft(dotContainer, puzzle.Dots[i].X - (dotContainer.Width / 2));
                        //ColorPointCanvas.Children.Add(dotContainer);
                        FigurePolyline.Points.Add(new Point(puzzle.Dots[i].X, puzzle.Dots[i].Y));

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void ClearFigureBoards(bool side)
        {
            if (side)
            {
                PuzzleBoardElementL.Children.Clear();
                CrayonElementLforPuzzleLine.Points.Clear();
            }
            else
            {
                PuzzleBoardElementR.Children.Clear();
                CrayonElementRforPuzzleLine.Points.Clear();
            }

        }
        private void DrawPuzzle(bool side)
        {

            try
            {
                int currentFigureIndex;
                List<DotPuzzle> figuresList;
                Canvas startFinishPointBoard;
                Polyline figurePolyline;
                if (side)
                {

                    currentFigureIndex = indexOfCurrentFigureLeft;
                    figuresList = _ListPuzzleL;
                    startFinishPointBoard = PuzzleBoardElementL;
                    figurePolyline = CrayonElementLforPuzzleLine;
                }
                else
                {
                    currentFigureIndex = indexOfCurrentFigureRight;
                    figuresList = _ListPuzzleR;
                    startFinishPointBoard = PuzzleBoardElementR;
                    figurePolyline = CrayonElementRforPuzzleLine;


                }

                if (figuresList[currentFigureIndex] != null)
                {
                    startFinishPointBoard.Children.Clear();
                    figurePolyline.Points.Clear();



                    if (_Configuration.HandsState == 1)
                    {
                        ClearFigureBoards(!side);
                    }
                    else
                    {
                        ClearFigureBoards(side);
                    }






                    for (int i = 0; i < figuresList[currentFigureIndex].Dots.Count; i++)
                    {
                        Grid dotContainer = new Grid();
                        dotContainer.Width = 80;
                        dotContainer.Height = 80;
                        if (i == 0 || i == figuresList[currentFigureIndex].Dots.Count - 1)
                        {
                            if(i ==0)
                            dotContainer.Children.Add(new Canvas() {  Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.greenBall))) }); // do punktu startowego i koncowego potrzebna grafika, najlepiej jakas animacja
                            if(i == figuresList[currentFigureIndex].Dots.Count - 1)
                                dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.redBall))) });
                            
                            
                            //TextBlock dotLabel = new TextBlock();
                            ////dotLabel.Text = (i + 1).ToString();
                            //dotLabel.Foreground = Brushes.White;
                            //dotLabel.FontSize = 35;
                            //dotLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            //dotLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            //dotContainer.Children.Add(dotLabel);

                            Canvas.SetTop(dotContainer, figuresList[currentFigureIndex].Dots[i].Y - (dotContainer.Height / 2));
                            Canvas.SetLeft(dotContainer, figuresList[currentFigureIndex].Dots[i].X - (dotContainer.Width / 2));
                            startFinishPointBoard.Children.Add(dotContainer);

                        }
                        else
                            dotContainer.Children.Add(new Ellipse() { Fill = figurePolyline.Stroke });

                        figurePolyline.Points.Add(new Point(figuresList[currentFigureIndex].Dots[i].X, figuresList[currentFigureIndex].Dots[i].Y));

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        int indexOfCurrentFigureLeft = 0;
        int indexOfCurrentFigureRight = 0;
        bool rRaeady, lRaeady;

        private BitmapSource ConvertBitmapToBitmapSource(System.Drawing.Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }
        private Point GetCoordinatesFromJoint(Joint joint)
        {
            Point point = new Point();
            point.X = (joint.Position.X * _Player._playerScale) + _Player._playerCenter.X;
            point.Y = _Player._playerCenter.Y - (joint.Position.Y * _Player._playerScale);

            return point;

        }


        private void TrackPuzzle(Joint joint, ref int currentPointIndex, List<DotPuzzle> figuresList, Polyline statusPolilyline, Canvas startFinishPointBoard, ref int currentFigureIndex, ref bool sideIsDone)
        {
            Grid dotContainer = new Grid();
            dotContainer.Width = 80;
            dotContainer.Height = 80;


            if (!sideIsDone)
            {
              if (currentPointIndex == figuresList[currentFigureIndex].Dots.Count - 1)
                {
                    SetNewFigure(ref currentPointIndex, statusPolilyline, startFinishPointBoard, ref currentFigureIndex);

                    if (figuresList.Count == currentFigureIndex) // bieżąca figura jest ostatnia dla danej strony
                    {
                        currentFigureIndex = 0;
                        sideIsDone = true;

                        ClearFigureBoards(_currentHand);

                        if (lRaeady == true && rRaeady == true)
                        { 
                            ClearFigureBoards(!_currentHand);
                        ClearFigureBoards(_currentHand);
                            EndGame();
                        }
                        if (!gameOver)
                        {
                            if (_Configuration.HandsState == 1) // działa w trypie naprzemiennym
                            {
                                _currentHand = !_currentHand;
                                DrawPuzzle(_currentHand);
                            }
                        }
                    }
                    else
                    {
                        if (_Configuration.HandsState == 2)
                        {
                          //  DrawPuzzle(figuresList[currentFigureIndex], startFinishPointBoard, figurePolyline);
                            DrawPuzzle(_currentHand);
                            DrawPuzzle(!_currentHand);
                        }
                        else if (_Configuration.HandsState == 1)
                        {
                            ClearFigureBoards(_currentHand);
                            _currentHand = !_currentHand;
                            DrawPuzzle(_currentHand);
                        }
                    }
                }
                else
                {
                    Point dot;

                    if (currentPointIndex + 1 < figuresList[currentFigureIndex].Dots.Count)
                    {
                        dot = figuresList[currentFigureIndex].Dots[currentPointIndex + 1];
                    }
                    else
                    {
                        dot = figuresList[currentFigureIndex].Dots[0];
                    }


                    Point handPoint = GetCoordinatesFromJoint(joint);


                    Point dotDiff = new Point(dot.X - handPoint.X, dot.Y - handPoint.Y);
                    double length = Math.Sqrt(dotDiff.X * dotDiff.X + dotDiff.Y * dotDiff.Y);

                    int lastPoint = statusPolilyline.Points.Count - 1;


                    if (length < precision)
                    {
                        stopwatchOfOutOfField.Stop();
                        if (lastPoint > 0)
                        {

                            statusPolilyline.Points.RemoveAt(lastPoint);
                            //startFinishPointBoard.Children.Remove(dotContainer);
                        }


                        //dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.greenBall))) });
                        //Canvas.SetTop(dotContainer, dot.X + 40);
                        //Canvas.SetLeft(dotContainer, dot.X - 40);
                        //startFinishPointBoard.Children.Add(dotContainer);
                        
                        statusPolilyline.Points.Add(new Point(dot.X, dot.Y));


                        statusPolilyline.Points.Add(new Point(dot.X, dot.Y));


                        currentPointIndex++;

                    }
                    else
                    {

                        if (lastPoint > 0)
                        {
                            //startFinishPointBoard.Children.Remove(dotContainer);
                            //dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.yellowBall))) });
                            //Canvas.SetTop(dotContainer, dot.X + 40);
                            //Canvas.SetLeft(dotContainer, dot.X - 40);
                            //startFinishPointBoard.Children.Add(dotContainer);

                            stopwatchOfOutOfField.Start();

                            Point lineEndpoint = statusPolilyline.Points[lastPoint];
                            statusPolilyline.Points.RemoveAt(lastPoint);
                            lineEndpoint.X = handPoint.X;
                            lineEndpoint.Y = handPoint.Y;
                            statusPolilyline.Points.Add(lineEndpoint);
                        }
                    }
                }
            }
        }

        private void TrackPuzzle(Joint joint, ref int puzzleDotCurrentIndex, List<DotPuzzle> listOfPuzzle, Polyline crayonElement, Canvas puzzleBoardElement, ref int indexOfCurrentFigure, ref bool sideIsDone, Polyline crayonElementForPuzzleLine)
        {
            if (!sideIsDone)
            {

                if (puzzleDotCurrentIndex == listOfPuzzle[indexOfCurrentFigure].Dots.Count - 1)
                {
                    puzzleBoardElement.Children.Clear();
                    crayonElement.Points.Clear();
                    puzzleDotCurrentIndex = -1;
                    indexOfCurrentFigure++;
                    //if (_Configuration.HandsState == 1)
                    //{
                    //    _currentHand = !_currentHand;
                    //}

                    if (listOfPuzzle.Count == indexOfCurrentFigure)
                    {
                        indexOfCurrentFigure = 0;
                        sideIsDone = true;
                        crayonElementForPuzzleLine.Points.Clear();
                        if (lRaeady == true && rRaeady == true)
                        {
                            ClearFigureBoards(!_currentHand);
                            ClearFigureBoards(_currentHand);
                            EndGame();
                        }

                    }
                    else
                    {
                        if (_Configuration.HandsState == 2)
                        {
                            DrawPuzzle(listOfPuzzle[indexOfCurrentFigure], puzzleBoardElement, crayonElementForPuzzleLine);
                        }
                        else if (_Configuration.HandsState == 1)
                        {
                            puzzleBoardElement.Children.Clear();
                            _currentHand = !_currentHand;
                            if (!_currentHand)
                            {

                                DrawPuzzle(_ListPuzzleR[indexOfCurrentFigure], PuzzleBoardElementR, CrayonElementRforPuzzleLine);
                                PuzzleBoardElementL.Children.Clear();
                                CrayonElementLforPuzzleLine.Points.Clear();
                            }
                            else
                            {
                                DrawPuzzle(_ListPuzzleL[indexOfCurrentFigure - 1], PuzzleBoardElementL, CrayonElementLforPuzzleLine);
                                PuzzleBoardElementR.Children.Clear();
                                CrayonElementRforPuzzleLine.Points.Clear();
                            }
                        }
                    }
                }
                else
                {
                    Point dot;

                    if (puzzleDotCurrentIndex + 1 < listOfPuzzle[indexOfCurrentFigure].Dots.Count)
                    {
                        dot = listOfPuzzle[indexOfCurrentFigure].Dots[puzzleDotCurrentIndex + 1];
                    }
                    else
                    {
                        dot = listOfPuzzle[indexOfCurrentFigure].Dots[0];
                    }


                    Point handPoint = GetCoordinatesFromJoint(joint);


                    Point dotDiff = new Point(dot.X - handPoint.X, dot.Y - handPoint.Y);
                    double length = Math.Sqrt(dotDiff.X * dotDiff.X + dotDiff.Y * dotDiff.Y);

                    int lastPoint = crayonElement.Points.Count - 1;


                    if (length < precision)
                    {
                        stopwatchOfOutOfField.Stop();
                        if (lastPoint > 0)
                        {

                            crayonElement.Points.RemoveAt(lastPoint);
                        }


                        crayonElement.Points.Add(new Point(dot.X, dot.Y));


                        crayonElement.Points.Add(new Point(dot.X, dot.Y));


                        puzzleDotCurrentIndex++;

                    }
                    else
                    {

                        if (lastPoint > 0)
                        {
                            stopwatchOfOutOfField.Start();
                            Point lineEndpoint = crayonElement.Points[lastPoint];
                            crayonElement.Points.RemoveAt(lastPoint);
                            lineEndpoint.X = handPoint.X;
                            lineEndpoint.Y = handPoint.Y;
                            crayonElement.Points.Add(lineEndpoint);
                        }
                    }
                }
            }
        }


        private void SetNewFigure(ref int currentPointIndex, Polyline statusPolilyline, Canvas startFinishPointBoard, ref int currentFigureIndex)// czyści ekran, ustala index nowej figury i jej bieżącego punktu
        {


            startFinishPointBoard.Children.Clear();
            statusPolilyline.Points.Clear();
            currentPointIndex = -1;
            currentFigureIndex++;
        }



        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Initializing:
                case KinectStatus.Connected:
                    this.KinectDevice = e.Sensor;
                    break;
                case KinectStatus.Disconnected:

                    this.KinectDevice = null;
                    break;
                default:

                    break;
            }
        }


        private void SkeletonsReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {

                    Skeleton skeleton;



                    frame.CopySkeletonDataTo(this._FrameSkeletons);

                    SkeletonBoardElement.Children.Clear();
                    GetPrimarySkeleton(this._FrameSkeletons);

                    skeleton = GetPrimarySkeleton(this._FrameSkeletons);
                    if (skeleton != null)
                    {
                        if (SkeletonTrackingState.Tracked == skeleton.TrackingState)
                        {

                            if (_Player == null)
                            {
                                _Player = new Player(11);
                                _Player.SetBounds(_playerBounds);
                            }

                            _Player.LastUpdated = DateTime.Now;

                            // Update player's bone and joint positions
                            if (skeleton.Joints.Count > 0)
                            {
                                _Player.IsAlive = true;

                                // Head, hands, feet (hit testing happens in order here)
                                _Player.UpdateJointPosition(skeleton.Joints, JointType.Head);
                                _Player.UpdateJointPosition(skeleton.Joints, JointType.HandLeft);
                                _Player.UpdateJointPosition(skeleton.Joints, JointType.HandRight);
                                _Player.UpdateJointPosition(skeleton.Joints, JointType.FootLeft);
                                _Player.UpdateJointPosition(skeleton.Joints, JointType.FootRight);

                                // Hands and arms
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.HandRight, JointType.WristRight);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.WristRight, JointType.ElbowRight);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.ElbowRight, JointType.ShoulderRight);
                                //_Player.Draw(SkeletonBoardElement.Children);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.HandLeft, JointType.WristLeft);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.WristLeft, JointType.ElbowLeft);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.ElbowLeft, JointType.ShoulderLeft);


                                // Head and Shoulders
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderCenter, JointType.Head);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderLeft,
                                    JointType.ShoulderCenter);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.ShoulderCenter,
                                    JointType.ShoulderRight);

                                // Legs
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.HipLeft, JointType.KneeLeft);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.KneeLeft, JointType.AnkleLeft);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.AnkleLeft, JointType.FootLeft);

                                _Player.UpdateBonePosition(skeleton.Joints, JointType.HipRight, JointType.KneeRight);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.KneeRight, JointType.AnkleRight);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.AnkleRight, JointType.FootRight);

                                _Player.UpdateBonePosition(skeleton.Joints, JointType.HipLeft, JointType.HipCenter);
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.HipCenter, JointType.HipRight);

                                // Spine
                                _Player.UpdateBonePosition(skeleton.Joints, JointType.HipCenter, JointType.ShoulderCenter);
                            }
                            _Player.Draw(SkeletonBoardElement.Children);



                            if (!gameOver)
                            {
                                if (_Configuration.HandsState == 2)
                                {
                                    TrackPuzzle(skeleton.Joints[JointType.HandRight], ref _PuzzleDotIndexR, _ListPuzzleR, CrayonElementR, PuzzleBoardElementR, ref indexOfCurrentFigureRight, ref rRaeady, CrayonElementRforPuzzleLine);
                                    TrackPuzzle(skeleton.Joints[JointType.HandLeft], ref _PuzzleDotIndexL, _ListPuzzleL, CrayonElementL, PuzzleBoardElementL, ref indexOfCurrentFigureLeft, ref lRaeady, CrayonElementLforPuzzleLine);
                                }
                                else if (_Configuration.HandsState == 1)
                                {
                                    if (_currentHand == false)
                                        TrackPuzzle(skeleton.Joints[JointType.HandRight], ref _PuzzleDotIndexR, _ListPuzzleR, CrayonElementR, PuzzleBoardElementR, ref indexOfCurrentFigureRight, ref rRaeady);

                                    else if (_currentHand == true)
                                        TrackPuzzle(skeleton.Joints[JointType.HandLeft], ref _PuzzleDotIndexL, _ListPuzzleL, CrayonElementL, PuzzleBoardElementL, ref indexOfCurrentFigureLeft, ref lRaeady);

                                }
                            }



                        }


                    }

                }
            }
        }


        private static Skeleton GetPrimarySkeleton(Skeleton[] skeletons)
        {
            Skeleton skeleton = null;

            if (skeletons != null)
            {

                for (int i = 0; i < skeletons.Length; i++)
                {
                    if (skeletons[i].TrackingState == SkeletonTrackingState.Tracked)
                    {
                        if (skeleton == null)
                        {
                            skeleton = skeletons[i];
                        }
                        else
                        {
                            if (skeleton.Position.Z > skeletons[i].Position.Z)
                            {
                                skeleton = skeletons[i];
                            }
                        }
                    }
                }
            }

            return skeleton;
        }



        #endregion Methods

        #region Properties
        public KinectSensor KinectDevice
        {
            get { return this._KinectDevice; }
            set
            {
                if (this._KinectDevice != value)
                {

                    if (this._KinectDevice != null)
                    {
                        this._KinectDevice.Stop();
                        this._KinectDevice.SkeletonFrameReady -= SkeletonsReady;
                        this._KinectDevice.SkeletonStream.Disable();
                        this._FrameSkeletons = null;
                    }

                    this._KinectDevice = value;


                    if (this._KinectDevice != null)
                    {
                        if (this._KinectDevice.Status == KinectStatus.Connected)
                        {
                            this._KinectDevice.SkeletonStream.Enable();
                            this._KinectDevice.ColorStream.Enable();
                            this._FrameSkeletons = new
        Skeleton[this._KinectDevice.SkeletonStream.FrameSkeletonArrayLength];
                            this.KinectDevice.SkeletonFrameReady +=
        SkeletonsReady;
                            this._KinectDevice.Start();
                        }
                    }
                }
            }
        }
        #endregion Properties


        #region Closing window

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {

                Close();
            }
        }
        #endregion




        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
            //this.SymetrialElement.Points.Add(new Point(SkeletonBoardElement.ActualWidth / 2, SkeletonBoardElement.ActualHeight * (4 / 6)));
            //this.SymetrialElement.Points.Add(new Point(SkeletonBoardElement.ActualWidth / 2, SkeletonBoardElement.ActualHeight));
            _playerBounds.X = 0;
            _playerBounds.Width = this.Width;
            _playerBounds.Y = 0;
            _playerBounds.Height = this.Height;
            PrepareGame();
            this.KinectDevice = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);

        }
    }

}





