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
using Microsoft.Kinect.Toolkit.Controls;
using DatabaseManagement;

namespace KinectMiniGames.ConfigPages
{
    /// <summary>
    /// Interaction logic for PlayerSelection.xaml
    /// </summary>
    public partial class PlayerSelection : UserControl
    {
        public Player SelectedPlayer { get; private set; }

        public PlayerSelection()
        {
            InitializeComponent();
            GenerateButtons();
        }

        private void GenerateButtons()
        {
            MainWindow.PlayersThread.Join();
            mainStackPanel.Children.Clear();
            for (var i = 0; i < MainWindow.PlayerList.Count; i++)
			{
                var item = MainWindow.PlayerList[i];
                var button = new KinectTileButton 
                { 
                    Content = item.Name + "\n" + item.Surname,
                    Tag = i,
                    Foreground = new SolidColorBrush(Colors.White),
                    Width = 300,
                    Height = 300
                };
                button.Click += button_Click;
                mainStackPanel.Children.Add(button);			 
			}
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as KinectTileButton;
            if (button != null)
            {
                var id = (int)button.Tag;
                SelectedPlayer = MainWindow.PlayerList[id];
            }
            MainWindow.SelectedPlayer = SelectedPlayer;

            var parent = (Panel)Parent;
            parent.Children.Remove(this);
        }
    }
}
