using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ApplesGame
{
    class Score
    {
        private int success;
        private int fail;
        private int applesLeft;
        private Canvas scoreboard;
        private Label actualScore;

        #region accessors
        public int Success
        {
            get { return this.success; }
            set { this.success = value; }
        }
        public int Fail
        {
            get { return this.fail; }
            set { this.fail = value; }
        }
        public int ApplesLeft
        {
            get { return this.applesLeft; }
            set { this.applesLeft = value; }
        }
        public Canvas Scoreboard
        {
            get { return this.scoreboard; }
            set { this.scoreboard = value; }
        }
        public Label ActualScore
        {
            get { return this.actualScore; }
            set { this.actualScore = value; }
        }
        #endregion accessors
        

        public Score(int applesCountPar = 0)
        {
            ApplesLeft = applesCountPar;
            Success = 0;
            Fail = 0;
            Scoreboard = new Canvas();
            Scoreboard.Width = 200;
            Scoreboard.Height = 150;
            Scoreboard.HorizontalAlignment = HorizontalAlignment.Right;
            Scoreboard.VerticalAlignment = VerticalAlignment.Top;
            Scoreboard.Margin = new Thickness(0, 200, 0, 0);
            Scoreboard.Background = new SolidColorBrush(Colors.Blue);

            ActualScore = new Label();
            ActualScore.Width = 200;
            ActualScore.Height = 200;
            ActualScore.HorizontalAlignment = HorizontalAlignment.Right;
            ActualScore.VerticalAlignment = VerticalAlignment.Top;
            ActualScore.Margin = new Thickness(0, 30, 0, 0);
            ActualScore.FontSize = 90;
            ActualScore.HorizontalContentAlignment = HorizontalAlignment.Center;
            ActualScore.Foreground = new SolidColorBrush(Colors.White);
            ActualScore.Content = ApplesLeft;

            Label description = new Label();
            description.Width = 200;
            description.Height = 100;
            description.HorizontalAlignment = HorizontalAlignment.Right;
            description.VerticalAlignment = VerticalAlignment.Top;
            description.Margin = new Thickness(0, 10, 0, 0);
            description.FontSize = 40;
            description.Foreground = new SolidColorBrush(Colors.White);
            description.Content = "Pozostało:";

            Scoreboard.Children.Add(description);
            Scoreboard.Children.Add(ActualScore);
        }

        public void collectSuccess()
        {
            Success++;
            ApplesLeft--;
            ActualScore.Content = Success;
        }

        public void collectFail()
        {
            Fail++;
        }
    }
}
