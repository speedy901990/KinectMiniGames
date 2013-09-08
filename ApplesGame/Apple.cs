using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Kinect.Toolkit.Controls;

namespace ApplesGame
{
    public class Apple
    {
        #region private fields
        
        private int size;
        private Point pos; 
        private Color color;
        private KinectCircleButton figure;
        private int number;
        private int treeNumber;

        #endregion private fields

        #region accessors
        public int TreeNumber
        {
            get { return this.treeNumber; }
            set { this.treeNumber = value; }
        }

        public int Number
        {
            get { return this.number; }
            set { this.number = value; }
        }

        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        public Point Pos
        {
            get { return this.pos; }
            set { this.pos = value; }
        }

        public Color Color
        {
            get {return this.color;}
            set {this.color = value;}
        }

        public KinectCircleButton Figure
        {
            get { return this.figure;}
            set {this.figure = value;}
        }
        #endregion accessors

        //Constructor take parameters that describe the range
        //to generate apples
        public Apple(Point rangeMin, Point rangeMax, int appleSize, int appleNumber, int tree)
        {
            System.Random rand = new Random(Guid.NewGuid().GetHashCode());

            //Setting random position and size of the apple
            Pos = new Point(rand.Next((int)rangeMin.X, (int)rangeMax.X),
                            rand.Next((int)rangeMin.Y, (int)rangeMax.Y));
            Size = appleSize;

            Number = appleNumber;
            TreeNumber = tree;

            //Creating Ellipse filled with image
            Figure = new KinectCircleButton();
            //Coloring (1 - red, 2 - green, 3 - yellow)
            int colorTemp = rand.Next(1, 4);
            setAppleGraphics(colorTemp);

            // Set the width and height of the Ellipse.
            figure.Width = Size;
            figure.Height = Size;

            // Set position of figure
            figure.Margin = new Thickness(Pos.X, Pos.Y, 0, 0);
            figure.HorizontalAlignment = HorizontalAlignment.Left;
            figure.Name = "Apple" + Number;
            figure.Content = int.Parse(figure.Name.Substring(5));
            //figure.BorderThickness =
        }

        public Apple(Apple target, double x, double y)
        {

            //Setting point 
            Pos = new Point(x, y);
            Size = target.Size;

            //Creating Ellipse filled with image
            Figure = new KinectCircleButton();
            

            if (target.Color == Colors.Red)
                setAppleGraphics(1); 
            if (target.Color == Colors.Green)
                setAppleGraphics(2); 
            if (target.Color == Colors.Yellow)
                setAppleGraphics(3);

            // Set the width and height of the Ellipse.
            figure.Width = Size;
            figure.Height = Size;

            // Set position of figure
            figure.Margin = new Thickness(Pos.X, Pos.Y, 0, 0);
            figure.HorizontalAlignment = HorizontalAlignment.Left;
            figure.Content = target.Number;
        }

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
                    figure.Background = appleImage;
                    break;
                
                //Green
                case 2:
                    Color = Colors.Green;
                    appleImage = new ImageBrush();
                    appleImage.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/green_apple.png", UriKind.Relative));
                    figure.Background = appleImage;
                    break;
                
                //Yellow
                case 3:
                    Color = Colors.Yellow;
                    appleImage = new ImageBrush();
                    appleImage.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/yellow_apple.png", UriKind.Relative));
                    figure.Background = appleImage;
                    break;
                
                default:
                    Color = Colors.Red;
                    appleImage = new ImageBrush();
                    appleImage.ImageSource =
                        new BitmapImage(new Uri(@"/../Graphics/ApplesGame/red_apple.png", UriKind.Relative));
                    figure.Background = appleImage;
                    break;
            }
        }
    }
}
