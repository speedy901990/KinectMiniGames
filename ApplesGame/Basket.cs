using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ApplesGame
{
    class Basket
    {
        public static int basketCount;
        private int width;
        private int height;
        private Color color;
        private int colorNumber;
        private Point position;
        private Point endPosition;
        private int basketNumber;
        private Canvas figure;

        #region accessors
        public Point Position
        {
            get { return this.position; }
            set { this.position = value; }
        }
        public Point EndPosition
        {
            get { return this.endPosition; }
            set { this.endPosition = value; }
        }
        public Color Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
        public Canvas Figure
        {
            get { return this.figure; }
            set { this.figure = value; }
        }
        public int ColorNumber
        {
            get { return this.colorNumber; }
            set { this.colorNumber = value; }
        }
        #endregion accessors

        public Basket(int parWidth, int parHeight, Point parPosition, int parColor)
        {
            basketCount++;
            width = parWidth;
            height = parHeight;
            Position = parPosition;
            EndPosition = new Point(Position.X + width, Position.Y + height);
            basketNumber = basketCount;
            ColorNumber = parColor;
            Figure = new Canvas();

            Figure.Height = height;
            Figure.Width = width;
            Figure.Margin = new Thickness(Position.X, Position.Y, 0, 0);
            Figure.HorizontalAlignment = HorizontalAlignment.Left;
            
            setGraphics(parColor);
        }

        private BitmapSource convertBitmapToBitmapSource(System.Drawing.Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }

        private void setGraphics(int colourParam)
        {
            ImageBrush basketBg;
            switch (colourParam)
            {
                //case 0:
                //    color = Colors.Red;
                //    basketBg = new ImageBrush();
                //    basketBg.ImageSource = convertBitmapToBitmapSource(Properties.Resources.red_basket);
                //    Figure.Background = basketBg;
                //    break;
                //case 1:
                //    color = Colors.Green;
                //    basketBg = new ImageBrush();
                //    basketBg.ImageSource = convertBitmapToBitmapSource(Properties.Resources.green_basket);
                //    Figure.Background = basketBg;
                //    break;
                //case 2:
                //    color = Colors.Yellow;
                //    basketBg = new ImageBrush();
                //    basketBg.ImageSource = convertBitmapToBitmapSource(Properties.Resources.yellow_basket);
                //    Figure.Background = basketBg;
                //    break;
                //case 3:
                //    Color = Colors.Orange;
                //    basketBg = new ImageBrush();
                //    basketBg.ImageSource = convertBitmapToBitmapSource(Properties.Resources.orange_basket);
                //    Figure.Background = basketBg;
                //    break;
                //case 4:
                //    Color = Colors.Brown;
                //    basketBg = new ImageBrush();
                //    basketBg.ImageSource = convertBitmapToBitmapSource(Properties.Resources.brown_basket);
                //    Figure.Background = basketBg;
                //    break;
                //default:
                //    color = Colors.Red;
                //    basketBg = new ImageBrush();
                //    basketBg.ImageSource = convertBitmapToBitmapSource(Properties.Resources.basket);
                //    Figure.Background = basketBg;
                //    break;
                case 0:
                    color = Colors.Red;
                    basketBg = new ImageBrush();
                    basketBg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/red_basket.png", UriKind.Relative));
                    Figure.Background = basketBg;
                    break;
                case 1:
                    color = Colors.Green;
                    basketBg = new ImageBrush();
                    basketBg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/green_basket.png", UriKind.Relative));
                    Figure.Background = basketBg;
                    break;
                case 2:
                    color = Colors.Yellow;
                    basketBg = new ImageBrush();
                    basketBg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/yellow_basket.png", UriKind.Relative));
                    Figure.Background = basketBg;
                    break;
                case 3:
                    Color = Colors.Orange;
                    basketBg = new ImageBrush();
                    basketBg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/orange_basket.png", UriKind.Relative));
                    Figure.Background = basketBg;
                    break;
                case 4:
                    Color = Colors.Brown;
                    basketBg = new ImageBrush();
                    basketBg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/brown_basket.png", UriKind.Relative));
                    Figure.Background = basketBg;
                    break;
                default:
                    color = Colors.Red;
                    basketBg = new ImageBrush();
                    basketBg.ImageSource =
                        new BitmapImage(new Uri(@"../../../Graphics/ApplesGame/basket.png", UriKind.Relative));
                    Figure.Background = basketBg;
                    break;
            }
        }
    }
}
