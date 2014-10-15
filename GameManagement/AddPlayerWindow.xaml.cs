using DatabaseManagement;
using GameManagement.ViewModel;
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
using System.Windows.Shapes;

namespace GameManagement
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddPlayerWindow : Window
    {
        public PlayerViewModel player = new PlayerViewModel()
        {
        Name = "",
        Surname = "",
        Age=0
        };
        public AddPlayerWindow()
        {
            this.DataContext = player;
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.Name.Length > 0 && player.Surname.Length > 0)
            {


                using (var context = new GameModelContainer())
                {
                    context.Players.Add(new Player()
                    {
                        Name = player.Name,
                        Surname = player.Surname,
                        Age = player.Age
                    });
                    context.SaveChanges();
                }
                _lPlayerAdded.Visibility = Visibility.Visible;

            }
            

        }
    }
}
