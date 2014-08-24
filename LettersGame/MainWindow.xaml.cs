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
using LettersGame.View;
using Microsoft.Kinect.Toolkit;

namespace LettersGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KinectSensorChooser SensorChooser { get; set; }
        public LettersGameConfig Config { get; set; }

        public MainWindow()
            : this(new LettersGameConfig(){ CurrentLevel = 3, LettersCount = 3, TrolleysCount = 2})
        {
        }

        public MainWindow(LettersGameConfig config)
        {
            InitializeComponent();
            Loaded += OnLoaded;
            KeyDown += OnKeyDown;
            Config = config;
            Config.WindowHeight = Height;
            Config.WindowWidth = Width;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Escape)
            {
                Close();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            switch (Config.CurrentLevel)
            {
                case 1:
                    MainGrid.Children.Add(new FirstLevelView(Config));
                    break;
                case 2:
                    MainGrid.Children.Add(new SecondLevelView(Config));
                    break;
                case 3:
                    MainGrid.Children.Add(new ThirdLevelView(Config));
                    break;
            }
        }
    }
}
