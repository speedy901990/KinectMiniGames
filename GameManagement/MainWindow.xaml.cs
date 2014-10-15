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

namespace GameManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            GameManagement.ShowDetailsWindow window = new GameManagement.ShowDetailsWindow();
            window.Show();
        }

        private void AddPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            GameManagement.AddPlayerWindow window = new GameManagement.AddPlayerWindow();
            window.Show();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
