using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

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
            set
            {
                this.color = value;
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
            Size = rand.Next(60, 80);
            
            //Creating Ellipse filled with image
            Figure = new Ellipse();
            //Coloring (1 - red, 2 - green, 3 - yellow)
            int colorTemp = rand.Next(1, 4);
            setAppleGraphics(colorTemp); 

            // Set the width and height of the Ellipse.
            figure.Width = Size;
            figure.Height = Size;

            // Set position of figure
            figure.Margin = new Thickness(Pos.X, Pos.Y, 0, 0);
        }

        #region setters
        private void setAppleGraphics(int colourParam)
        {
            ImageBrush appleImage;
            switch (colourParam)
            {
                //Red
                case 1:
                    Color = Colors.Red;
                    appleImage = new ImageBrush();
                    appleImage.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/red_apple.png", UriKind.Relative));
                    figure.Fill = appleImage;
                    break;
                
                //Green
                case 2:
                    Color = Colors.Green;
                    appleImage = new ImageBrush();
                    appleImage.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/green_apple.png", UriKind.Relative));
                    figure.Fill = appleImage;
                    break;
                
                //Yellow
                case 3:
                    Color = Colors.Yellow;
                    appleImage = new ImageBrush();
                    appleImage.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/yellow_apple.png", UriKind.Relative));
                    figure.Fill = appleImage;
                    break;
                
                default:
                    Color = Colors.Red;
                    appleImage = new ImageBrush();
                    appleImage.ImageSource =
                        new BitmapImage(new Uri(@"/../Graphics/ApplesGame/red_apple.png", UriKind.Relative));
                    figure.Fill = appleImage;
                    break;
            }
        }
        #endregion setters


    }
}