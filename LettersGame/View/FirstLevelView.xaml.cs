using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for FirstLevelView.xaml
    /// </summary>
    public partial class FirstLevelView : UserControl
    {
        private LettersGameConfig config;
        private Game game;
        private int letterWidth;
        private int letterHeight;

        public FirstLevelView(LettersGameConfig config = null)
        {
            InitializeComponent();
            this.config = config;
            this.game = new Game(this.config.FirstLevelLettersCount);
            this.letterHeight = (int)(this.config.WindowHeight / 3);
            this.letterWidth = (int)(this.config.WindowWidth / this.config.FirstLevelLettersCount);
        }

        private void setDisplay()
        {

        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            //this.rect1.DataContext = this.config;
        }
    }
}
