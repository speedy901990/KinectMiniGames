using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace DrawingGame
{
    public partial class MainWindow : Window
    {

        #region Member Variables
        WindowSize _windowSize;
        public Stopwatch StopwatchOfGame;
        public Stopwatch StopwatchOfOutOfField;

        public bool GameOver = false;
        public bool CurrentHand;
        public DrawingGameConfig Configuration;

        public KinectSensor _kinectDevice;
        public readonly Brush[] SkeletonBrushes;
        public Skeleton[] FrameSkeletons;
        public List<Polyline> Figures = new List<Polyline>();
        public List<DotPuzzle> ListPuzzleL = new List<DotPuzzle>();
        public List<DotPuzzle> ListPuzzleR = new List<DotPuzzle>();
        public int PuzzleDotIndexL;
        public int PuzzleDotIndexR;
        public Player Player;
        public Rect PlayerBounds = new Rect();
        private readonly Dictionary<int, Player> _players = new Dictionary<int, Player>();
        public int Precision;
        private Thread _saveResults;
        public int IndexOfCurrentFigureLeft = 0;
        public int IndexOfCurrentFigureRight = 0;
        public bool RRaeady, LRaeady;
        private readonly Draftsman _draftsman;
        private readonly ResultSaver _resultSaver;
        private readonly KinectManager _kinectManager;
        private readonly Tracker _tracker;

        #endregion Member Variables

        #region Constructor
        public MainWindow(DrawingGameConfig config)
        {
            _windowSize = new WindowSize();
            _windowSize.Width = SystemParameters.PrimaryScreenWidth;
            _windowSize.Height = SystemParameters.PrimaryScreenHeight;

            StopwatchOfGame = new Stopwatch();
            StopwatchOfOutOfField = new Stopwatch();

            Configuration = config;
            InitializeComponent();

            SkeletonBrushes = new Brush[] { Brushes.Green, Brushes.Blue, Brushes.Red, Brushes.Orange };
            _draftsman = new Draftsman(this);
            _resultSaver = new ResultSaver(this);
            _kinectManager = new KinectManager(this);
            _tracker = new Tracker(this);
        }


        #endregion Constructor

        #region Methods

        public void EndGame()
        {


            StopwatchOfGame.Stop();


            _saveResults = new Thread(ResultSaver.SaveResults);
            _saveResults.Start();
            GameOver = true;
            GameOverPopup popup = new GameOverPopup(this);
            popup.VerticalAlignment = VerticalAlignment.Center;
            popup.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SetTop(popup, Height / 2);
            Canvas.SetLeft(popup, Width / 2);
            EndPopUpCanvas.Children.Add(popup);
        }



        private void PrepareGame()
        {

            LoadLevels();

            PuzzleDotIndexL = -1;
            PuzzleDotIndexR = -1;
            CurrentHand = false;

            if (Configuration.Precision == 2)
            {
                Precision = 30;
            }
            else if (Configuration.Precision == 1)
            {
                Precision = 50;
            }



            if (Configuration.HandsState == 2)
            {
                Draftsman.DrawPuzzle(CurrentHand);
                Draftsman.DrawPuzzle(!CurrentHand);

            }
            if (Configuration.HandsState == 1)
            {
                Draftsman.DrawPuzzle(CurrentHand);

            }

            StopwatchOfGame.Start();
        }

        private void LoadLevels()
        {
            double centerX = (double)Width / 2;
            double centerY = (double)Height / 2;


            DotPuzzle firstLeftHand = new DotPuzzle();
            DotPuzzle secondLeftHand = new DotPuzzle();
            DotPuzzle thirdLeftHand = new DotPuzzle();
            DotPuzzle fourthLeftHand = new DotPuzzle();
            DotPuzzle fifthLeftHand = new DotPuzzle();
            DotPuzzle sixthLeftHand = new DotPuzzle();
            DotPuzzle seventhLeftHand = new DotPuzzle();
            DotPuzzle eignthLeftHand = new DotPuzzle();
            DotPuzzle ninthLeftHand = new DotPuzzle();
            DotPuzzle tenthLeftHand = new DotPuzzle();

            if (Configuration.Difficulty == 1)//easy
            {
                #region Easy Levels Data For Left Hand

                firstLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (38.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (44.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (46.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (51.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (54.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (58.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (64.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (68.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (72.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (76.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (83.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (91.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (97.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (104.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (110.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (116.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (123.0 / 100.0)));

                secondLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (38.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (38.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (44.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (46.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (50.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (53.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (56.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (62.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (67.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (74.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (81.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (87.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (94.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (101.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (109.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (117.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (121.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (124.0 / 100.0)));




                thirdLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (38.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (38.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (42.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (46.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (50.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (52.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (55.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (60.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (65.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (72.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (81.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (88.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (93.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (100.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (104.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (112.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (117.0 / 100.0)));




                fourthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (42.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (45.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (50.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (57.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (63.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (71.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (75.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (78.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (82.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (87.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (93.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (103.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (112.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (118.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (119.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (118.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (116.0 / 100.0)));




                fifthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (54.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (54.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (53.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (54.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (54.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (58.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (65.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (76.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (91.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (97.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (108.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (114.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (122.0 / 100.0)));




                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (51.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (51.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (55.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (61.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (69.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (75.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (81.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (96.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (102.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (110.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (118.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (122.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (123.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (124.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (123.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (124.0 / 100.0)));




                seventhLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (51.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (53.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (57.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (60.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (65.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (71.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (77.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (83.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (86.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (90.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (97.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (99.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (103.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (107.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (112.0 / 100.0)));


                eignthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (51.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (55.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (58.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (64.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (66.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (73.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (76.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (84.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (91.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (98.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (104.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (108.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (111.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (115.0 / 100.0)));




                ninthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (45.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (47.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (52.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (59.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (65.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (67.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (68.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (68.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (71.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (74.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (80.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (84.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (90.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (102.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (105.0 / 100.0)));




                tenthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (114.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (110.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (101.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (86.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (81.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (67.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (58.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (51.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (50.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (50.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (51.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (57.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (66.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (68.0 / 100.0), centerY * (73.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (78.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (86.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (98.0 / 100.0)));


                #endregion

            }
            else if (Configuration.Difficulty == 2)
            {
                #region Medium Levels Data For Left Hand

                firstLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (52.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (51.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (48.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (47.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (47.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (49.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (56.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (64.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (75.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (83.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (87.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (86.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (80.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (73.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (73.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (77.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (68.0 / 100.0), centerY * (85.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (99.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (68.0 / 100.0), centerY * (108.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (115.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (120.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (122.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (119.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (115.0 / 100.0)));
                firstLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (110.0 / 100.0)));




                secondLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (64.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (72.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (82.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (88.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (91.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (85.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (70.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (63.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (48.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (46.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (45.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (49.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (56.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (64.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (77.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (87.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (98.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (110.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (113.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (114.0 / 100.0)));
                secondLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (111.0 / 100.0)));



                thirdLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (48.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (54.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (64.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (70.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (81.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (91.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (105.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (119.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (129.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (134.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (132.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (122.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (114.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (108.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (95.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (81.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (72.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (60.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (51.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (49.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (47.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (50.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (57.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (66.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (72.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (83.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (95.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (108.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (114.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (129.0 / 100.0)));
                thirdLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (137.0 / 100.0)));




                fourthLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (42.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (41.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (41.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (42.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (41.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (42.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (44.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (46.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (54.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (60.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (62.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (63.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (64.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (65.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (67.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (75.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (83.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (85.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (84.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (83.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (82.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (82.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (65.0 / 100.0), centerY * (86.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (64.0 / 100.0), centerY * (94.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (68.0 / 100.0), centerY * (101.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (105.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (103.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (102.0 / 100.0)));
                fourthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (104.0 / 100.0)));



                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (42.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (41.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (41.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (49.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (60.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (66.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (72.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (70.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (64.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (55.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (46.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (42.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (68.0 / 100.0), centerY * (49.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (63.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (71.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (79.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (89.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (93.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (100.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (109.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (116.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (112.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (111.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (104.0 / 100.0)));
                fifthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (90.0 / 100.0)));




                sixthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (57.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (63.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (72.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (88.0 / 100.0), centerY * (73.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (69.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (59.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (49.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (42.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (39.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (38.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (40.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (49.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (62.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (76.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (88.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (104.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (117.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (125.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (131.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (134.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (131.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (115.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (93.0 / 100.0), centerY * (105.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (93.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (90.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (92.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (99.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (110.0 / 100.0)));
                sixthLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (114.0 / 100.0)));




                seventhLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (48.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (48.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (49.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (47.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (49.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (48.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (52.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (63.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (72.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (85.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (98.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (114.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (120.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (128.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (129.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (130.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (130.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (118.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (105.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (90.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (81.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (70.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (63.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (66.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (67.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (67.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (67.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (77.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (86.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (98.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (107.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (114.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (111.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (98.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (89.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (83.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (83.0 / 100.0)));
                seventhLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (86.0 / 100.0)));




                eignthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (64.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (61.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (52.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (49.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (50.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (57.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (69.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (76.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (80.0 / 100.0), centerY * (76.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (73.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (74.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (82.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (93.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (95.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (95.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (92.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (92.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (101.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (96.0 / 100.0), centerY * (110.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (121.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (128.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (83.0 / 100.0), centerY * (130.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (77.0 / 100.0), centerY * (132.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (128.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (124.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (112.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (110.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (111.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (112.0 / 100.0)));
                eignthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (110.0 / 100.0)));




                ninthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (66.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (66.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (66.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (86.0 / 100.0), centerY * (66.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (65.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (65.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (55.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (47.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (45.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (45.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (46.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (46.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (74.0 / 100.0), centerY * (47.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (46.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (68.0 / 100.0), centerY * (48.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (53.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (63.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (71.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (83.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (93.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (67.0 / 100.0), centerY * (106.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (70.0 / 100.0), centerY * (113.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (113.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (113.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (87.0 / 100.0), centerY * (112.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (90.0 / 100.0), centerY * (111.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (111.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (110.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (95.0 / 100.0), centerY * (101.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (93.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (94.0 / 100.0), centerY * (88.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (84.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (89.0 / 100.0), centerY * (85.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (84.0 / 100.0), centerY * (86.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (85.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (78.0 / 100.0), centerY * (85.0 / 100.0)));
                ninthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (86.0 / 100.0)));




                tenthLeftHand.Dots.Add(new Point(centerX * (65.0 / 100.0), centerY * (130.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (128.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (75.0 / 100.0), centerY * (131.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (131.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (132.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (116.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (96.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (91.0 / 100.0), centerY * (83.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (68.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (92.0 / 100.0), centerY * (58.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (85.0 / 100.0), centerY * (57.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (79.0 / 100.0), centerY * (57.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (76.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (88.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (103.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (82.0 / 100.0), centerY * (110.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (81.0 / 100.0), centerY * (115.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (76.0 / 100.0), centerY * (114.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (71.0 / 100.0), centerY * (112.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (96.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (72.0 / 100.0), centerY * (84.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (68.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (73.0 / 100.0), centerY * (57.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (69.0 / 100.0), centerY * (50.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (66.0 / 100.0), centerY * (51.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (63.0 / 100.0), centerY * (59.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (64.0 / 100.0), centerY * (72.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (64.0 / 100.0), centerY * (83.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (62.0 / 100.0), centerY * (96.0 / 100.0)));
                tenthLeftHand.Dots.Add(new Point(centerX * (63.0 / 100.0), centerY * (113.0 / 100.0)));


                #endregion

            }


            ListPuzzleL.Add(firstLeftHand);
            ListPuzzleL.Add(secondLeftHand);
            ListPuzzleL.Add(thirdLeftHand);
            ListPuzzleL.Add(fourthLeftHand);
            ListPuzzleL.Add(fifthLeftHand);
            ListPuzzleL.Add(sixthLeftHand);
            ListPuzzleL.Add(seventhLeftHand);
            ListPuzzleL.Add(eignthLeftHand);
            ListPuzzleL.Add(ninthLeftHand);
            ListPuzzleL.Add(tenthLeftHand);

            {
                DotPuzzle firstRightHand = new DotPuzzle();
                DotPuzzle secondRightHand = new DotPuzzle();
                DotPuzzle thirdRightHand = new DotPuzzle();
                DotPuzzle fourthRightHand = new DotPuzzle();
                DotPuzzle fifthRightHand = new DotPuzzle();
                DotPuzzle sixthRightHand = new DotPuzzle();
                DotPuzzle seventhRightHand = new DotPuzzle();
                DotPuzzle eignthRightHand = new DotPuzzle();
                DotPuzzle ninthRightHand = new DotPuzzle();
                DotPuzzle tenthRightHand = new DotPuzzle();


                if (Configuration.Difficulty == 1)//easy
                {
                    #region Easy Levels Data For Right Hand

                    firstRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (38.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (44.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (46.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (51.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (54.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (58.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (64.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (68.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (72.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (76.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (83.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (91.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (97.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (104.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (110.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (116.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (123.0 / 100.0)));





                    secondRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (38.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (38.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (44.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (46.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (50.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (53.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (56.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (62.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (67.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (74.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (81.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (87.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (94.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (101.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (109.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (117.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (121.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (124.0 / 100.0)));





                    thirdRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (38.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (38.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (42.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (46.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (50.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (52.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (55.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (60.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (65.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (72.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (81.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (88.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (93.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (100.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (104.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (112.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (117.0 / 100.0)));




                    fourthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (42.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (45.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (50.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (57.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (63.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (71.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (75.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (78.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (82.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (87.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (93.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (103.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (112.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (118.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (119.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (118.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (116.0 / 100.0)));




                    fifthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (54.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (54.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (53.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (54.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (54.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (58.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (65.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (76.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (91.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (97.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (108.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (114.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (122.0 / 100.0)));




                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (51.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (51.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (55.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (61.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (69.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (75.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (81.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (96.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (102.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (110.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (118.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (122.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (123.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (124.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (123.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (124.0 / 100.0)));





                    seventhRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (51.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (53.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (57.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (60.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (65.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (71.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (77.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (83.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (86.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (90.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (97.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (99.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (103.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (107.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (112.0 / 100.0)));





                    eignthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (51.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (55.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (58.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (64.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (66.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (73.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (76.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (84.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (91.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (98.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (104.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (108.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (111.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (115.0 / 100.0)));




                    ninthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (45.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (47.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (52.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (59.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (65.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (67.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (68.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (68.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (71.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (74.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (80.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (84.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (90.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (102.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (105.0 / 100.0)));



                    tenthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (114.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (110.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (101.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (86.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (81.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (67.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (58.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (51.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (50.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (50.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (51.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (57.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (66.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (132.0 / 100.0), centerY * (73.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (78.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (86.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (98.0 / 100.0)));


                    #endregion

                }
                else if (Configuration.Difficulty == 2)
                {
                    #region Medium Levels Data For Right Hand

                    firstRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (52.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (51.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (48.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (47.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (47.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (49.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (56.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (64.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (75.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (83.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (87.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (86.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (80.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (73.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (73.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (77.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (132.0 / 100.0), centerY * (85.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (99.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (132.0 / 100.0), centerY * (108.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (115.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (120.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (122.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (119.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (115.0 / 100.0)));
                    firstRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (110.0 / 100.0)));





                    secondRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (64.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (72.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (82.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (88.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (91.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (85.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (70.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (63.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (48.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (46.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (45.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (49.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (56.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (64.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (77.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (87.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (98.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (110.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (113.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (114.0 / 100.0)));
                    secondRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (111.0 / 100.0)));





                    thirdRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (48.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (54.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (64.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (70.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (81.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (91.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (105.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (119.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (129.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (134.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (132.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (122.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (114.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (108.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (95.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (81.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (72.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (60.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (51.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (49.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (47.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (50.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (57.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (66.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (72.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (83.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (95.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (108.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (114.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (129.0 / 100.0)));
                    thirdRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (137.0 / 100.0)));




                    fourthRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (42.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (41.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (41.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (42.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (41.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (42.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (44.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (46.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (54.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (60.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (62.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (63.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (64.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (65.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (67.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (75.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (83.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (85.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (84.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (83.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (82.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (82.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (135.0 / 100.0), centerY * (86.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (136.0 / 100.0), centerY * (94.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (132.0 / 100.0), centerY * (101.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (105.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (103.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (102.0 / 100.0)));
                    fourthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (104.0 / 100.0)));





                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (42.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (41.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (41.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (49.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (60.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (66.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (72.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (70.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (64.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (55.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (46.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (42.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (132.0 / 100.0), centerY * (49.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (63.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (71.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (79.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (89.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (93.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (100.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (109.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (116.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (112.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (111.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (104.0 / 100.0)));
                    fifthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (90.0 / 100.0)));




                    sixthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (57.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (63.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (72.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (112.0 / 100.0), centerY * (73.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (69.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (59.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (49.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (42.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (39.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (38.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (40.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (49.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (62.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (76.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (88.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (104.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (117.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (125.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (131.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (134.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (131.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (115.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (107.0 / 100.0), centerY * (105.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (93.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (90.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (92.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (99.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (110.0 / 100.0)));
                    sixthRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (114.0 / 100.0)));




                    seventhRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (48.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (48.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (49.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (47.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (49.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (48.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (52.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (63.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (72.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (85.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (98.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (114.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (120.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (128.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (129.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (130.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (130.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (118.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (105.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (90.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (81.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (70.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (63.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (66.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (67.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (67.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (67.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (77.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (86.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (98.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (107.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (114.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (111.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (98.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (89.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (83.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (83.0 / 100.0)));
                    seventhRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (86.0 / 100.0)));




                    eignthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (64.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (61.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (52.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (49.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (50.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (57.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (69.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (76.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (120.0 / 100.0), centerY * (76.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (73.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (74.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (82.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (93.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (95.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (95.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (92.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (92.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (101.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (104.0 / 100.0), centerY * (110.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (121.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (128.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (117.0 / 100.0), centerY * (130.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (123.0 / 100.0), centerY * (132.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (128.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (124.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (112.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (110.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (111.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (112.0 / 100.0)));
                    eignthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (110.0 / 100.0)));




                    ninthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (66.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (66.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (66.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (114.0 / 100.0), centerY * (66.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (65.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (65.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (55.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (47.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (45.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (45.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (46.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (46.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (126.0 / 100.0), centerY * (47.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (46.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (132.0 / 100.0), centerY * (48.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (53.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (63.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (71.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (83.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (93.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (133.0 / 100.0), centerY * (106.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (130.0 / 100.0), centerY * (113.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (113.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (113.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (113.0 / 100.0), centerY * (112.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (110.0 / 100.0), centerY * (111.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (111.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (110.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (105.0 / 100.0), centerY * (101.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (93.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (106.0 / 100.0), centerY * (88.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (84.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (111.0 / 100.0), centerY * (85.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (116.0 / 100.0), centerY * (86.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (85.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (122.0 / 100.0), centerY * (85.0 / 100.0)));
                    ninthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (86.0 / 100.0)));




                    tenthRightHand.Dots.Add(new Point(centerX * (135.0 / 100.0), centerY * (130.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (128.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (125.0 / 100.0), centerY * (131.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (131.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (132.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (116.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (96.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (109.0 / 100.0), centerY * (83.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (68.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (108.0 / 100.0), centerY * (58.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (115.0 / 100.0), centerY * (57.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (121.0 / 100.0), centerY * (57.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (76.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (88.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (103.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (118.0 / 100.0), centerY * (110.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (119.0 / 100.0), centerY * (115.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (124.0 / 100.0), centerY * (114.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (129.0 / 100.0), centerY * (112.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (96.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (128.0 / 100.0), centerY * (84.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (68.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (127.0 / 100.0), centerY * (57.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (131.0 / 100.0), centerY * (50.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (134.0 / 100.0), centerY * (51.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (137.0 / 100.0), centerY * (59.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (136.0 / 100.0), centerY * (72.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (136.0 / 100.0), centerY * (83.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (138.0 / 100.0), centerY * (96.0 / 100.0)));
                    tenthRightHand.Dots.Add(new Point(centerX * (137.0 / 100.0), centerY * (113.0 / 100.0)));


                    #endregion
                }


                ListPuzzleR.Add(firstRightHand);
                ListPuzzleR.Add(secondRightHand);
                ListPuzzleR.Add(thirdRightHand);
                ListPuzzleR.Add(fourthRightHand);
                ListPuzzleR.Add(fifthRightHand);
                ListPuzzleR.Add(sixthRightHand);
                ListPuzzleR.Add(seventhRightHand);
                ListPuzzleR.Add(eignthRightHand);
                ListPuzzleR.Add(ninthRightHand);
                ListPuzzleR.Add(tenthRightHand);


            }
        }

        #endregion Methods

        #region Properties
        public KinectSensor KinectDevice
        {
            get { return _kinectDevice; }
            set
            {
                if (_kinectDevice != value)
                {

                    if (_kinectDevice != null)
                    {
                        _kinectDevice.Stop();
                        _kinectDevice.SkeletonFrameReady -= KinectManager.SkeletonsReady;
                        _kinectDevice.SkeletonStream.Disable();
                        FrameSkeletons = null;
                    }

                    _kinectDevice = value;


                    if (_kinectDevice != null)
                    {
                        if (_kinectDevice.Status == KinectStatus.Connected)
                        {
                            _kinectDevice.SkeletonStream.Enable();
                            _kinectDevice.ColorStream.Enable();
                            FrameSkeletons = new
        Skeleton[_kinectDevice.SkeletonStream.FrameSkeletonArrayLength];
                            KinectDevice.SkeletonFrameReady += KinectManager.SkeletonsReady;
                            _kinectDevice.Start();
                        }
                    }
                }
            }
        }

        public Draftsman Draftsman
        {
            get { return _draftsman; }
        }

        public ResultSaver ResultSaver
        {
            get { return _resultSaver; }
        }

        public KinectManager KinectManager
        {
            get { return _kinectManager; }
        }

        public Tracker Tracker
        {
            get { return _tracker; }
        }

        #endregion Properties


        #region Events Hanldlers

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {

                Close();
            }
        }


        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
            //this.SymetrialElement.Points.Add(new Point(SkeletonBoardElement.ActualWidth / 2, SkeletonBoardElement.ActualHeight * (4 / 6)));
            //this.SymetrialElement.Points.Add(new Point(SkeletonBoardElement.ActualWidth / 2, SkeletonBoardElement.ActualHeight));
            PlayerBounds.X = 0;
            PlayerBounds.Width = Width;
            PlayerBounds.Y = 0;
            PlayerBounds.Height = Height;
            PrepareGame();
            KinectDevice = KinectSensor.KinectSensors.FirstOrDefault(x => x.Status == KinectStatus.Connected);

        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutRoot.DataContext = _windowSize;
        }
        #endregion

        public Draftsman Draftsman1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public KinectManager KinectManager1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public ResultSaver ResultSaver1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        internal DrawingGame.Properties.Resources Resources
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public GameOverPopup GameOverPopup
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public DrawingGameConfig DrawingGameConfig
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Player Player1
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public WindowSize WindowSize
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public DotPuzzle DotPuzzle
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


    }

}





