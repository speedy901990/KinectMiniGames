using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace ApplesGame
{
    public class Apple
    {
        #region private fields
        
        private int size;
        private Point pos; 
        private Color color;
        private Ellipse figure;
        #endregion private fields

        #region accessors

        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        public Point Pos
        {
            get
            {
                return this.pos;
            }
            set
            {
                pos = value;
            }
        }

        public Color Color
        {
            get
            {
                return this.color;
            }
        }

        public Ellipse Figure
        {
            get
            {
                return this.figure;
            }
            set
            {
                this.figure = value;
            }
        }
        #endregion accessors

        //Constructor take parameters that describe the range
        //to generate apples
        public Apple(int xMin, int xMax, int yMin, int yMax)
        {
            System.Random rand = new Random(Guid.NewGuid().GetHashCode());
            
            //Setting random position and size of the apple
            Pos = new Point(rand.Next(xMin, xMax),rand.Next(yMin, yMax));
            Size = rand.Next(40, 60);
            
            //Creating Ellipse colored with SolidColorBrush
            Figure = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            //Coloring (1 - red, 2 - green, 3 - yellow)
            int colorTemp = rand.Next(1, 4);
            setColor(colorTemp); 
            mySolidColorBrush.Color = Color;
            figure.Fill = mySolidColorBrush;
            figure.StrokeThickness = 2;
            figure.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            figure.Width = Size;
            figure.Height = Size;

            // Set position of figure
            figure.Margin = new Thickness(Pos.X, Pos.Y, 0, 0);
        }

        #region setters
        private void setColor(int colourParam)
        {
            switch (colourParam)
            {
                //Red
                case 1:
                    color = Color.FromArgb(255, 255, 0, 0);
                    break;
                
                //Green
                case 2:
                    color = Color.FromArgb(255, 0, 255, 0);
                    break;
                
                //Yellow
                case 3:
                    color = Color.FromArgb(255, 255, 255, 0);
                    break;
                
                default:
                    color = Color.FromArgb(255, 255, 0, 0);
                    break;
            }
        }
        #endregion setters


    }
}