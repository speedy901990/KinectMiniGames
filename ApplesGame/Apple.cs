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

        private Ellipse shape;
        #endregion private fields

        //Constructor take parameters that describe the range
        //to generate apples
        public Apple(int xMin, int xMax, int yMin, int yMax)
        {
            //System.Random rand = new Random(System.DateTime.Now.Millisecond);
            System.Random rand = new Random(Guid.NewGuid().GetHashCode());
            rand.Next(xMin, xMax);
            pos.X = rand.Next(xMin, xMax);
            pos.Y = rand.Next(yMin, yMax);
            size = rand.Next(40, 60);
            //Creating Ellipse colored with SolidColorBrush
            shape = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            //Coloring (1 - red, 2 - green, 3 - yellow)
            int colorTemp = rand.Next(1, 4);
            setColor(colorTemp); 
            mySolidColorBrush.Color = getColor();
            shape.Fill = mySolidColorBrush;
            shape.StrokeThickness = 2;
            shape.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            shape.Width = size;
            shape.Height = size;

            // Set position of shape
            shape.Margin = new Thickness(pos.X, pos.Y, 0, 0);
        }

        #region getters
        public Ellipse getShape()
        {
            return this.shape;
        }

        public Point getPosition()
        {
            return this.pos;
        }

        public int getSize()
        {
            return this.size;
        }

        public Color getColor()
        {
            return this.color;
        }
        #endregion getters


        #region setters
        private void setSize(int sizeParam)
        {
            this.size = sizeParam;
        }

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
