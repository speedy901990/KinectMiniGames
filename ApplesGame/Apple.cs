using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace ApplesGame
{
    public class Apple
    {
        #region private fields
        
        private int size;
        private ApplePosition pos;
        private Color color;

        private Ellipse shape;
        #endregion private fields

        //Constructor take parameters that describe the range
        //to generate apples
        public Apple(int xMin, int xMax, int yMin, int yMax)
        {
            System.Random rand = new Random(Seed);
            rand.Next(xMin, xMax);
            pos = new ApplePosition(rand.Next(xMin, xMax), rand.Next(yMin, yMax));
            size = rand.Next(50, 75);

            //Creating Ellipse colored with SolidColorBrush
            shape = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            //Coloring (1 - red, 2 - green, 3 - yellow)
            int colorTemp = rand.Next(1, 3);
            setColor(colorTemp); 
            mySolidColorBrush.Color = getColor();
            shape.Fill = mySolidColorBrush;
            shape.StrokeThickness = 2;
            shape.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            shape.Width = size;
            shape.Height = size;
        }

        public int Seed { get; set; }

        #region getters
        public Ellipse getShape()
        {
            return this.shape;
        }

        public ApplePosition getPosition()
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

    public class ApplePosition
    {
        private int posX;
        private int posY;

        public ApplePosition(int x, int y)
        {
            this.posX = x;
            this.posY = y;
        }
    }
}
