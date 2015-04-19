using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DrawingGame.Properties;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

namespace DrawingGame
{
    public class Draftsman
    {
        private MainWindow _mainWindow;

        public Draftsman(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void DrawPuzzle(DotPuzzle puzzle, Canvas colorPointCanvas, Polyline figurePolyline)
        {

            try
            {
                if (puzzle != null)
                {
                    colorPointCanvas.Children.Clear();
                    figurePolyline.Points.Clear();
                    for (int i = 0; i < puzzle.Dots.Count; i++)
                    {
                        Grid dotContainer = new Grid();
                        dotContainer.Width = 80;
                        dotContainer.Height = 80;
                        if (i == 0 || i == puzzle.Dots.Count - 1)
                        {
                            if (i == 0)
                                dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Resources.greenBall))) }); // do punktu startowego i koncowego potrzebna grafika, najlepiej jakas animacja
                            if (i == puzzle.Dots.Count - 1)
                                dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Resources.redBall))) });
                            

                            TextBlock dotLabel = new TextBlock();
                            //dotLabel.Text = (i + 1).ToString();
                            dotLabel.Foreground = Brushes.White;
                            dotLabel.FontSize = 35;
                            dotLabel.HorizontalAlignment = HorizontalAlignment.Center;
                            dotLabel.VerticalAlignment = VerticalAlignment.Center;
                            dotContainer.Children.Add(dotLabel);

                            Canvas.SetTop(dotContainer, puzzle.Dots[i].Y - (dotContainer.Height / 2));
                            Canvas.SetLeft(dotContainer, puzzle.Dots[i].X - (dotContainer.Width / 2));
                            colorPointCanvas.Children.Add(dotContainer);

                        }
                        else
                            dotContainer.Children.Add(new Ellipse() { Fill = figurePolyline.Stroke });

                        //TextBlock dotLabel = new TextBlock();
                        ////dotLabel.Text = (i + 1).ToString();
                        //dotLabel.Foreground = Brushes.White;
                        //dotLabel.FontSize = 35;
                        //dotLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        //dotLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                        //dotContainer.Children.Add(dotLabel);

                        //Canvas.SetTop(dotContainer, puzzle.Dots[i].Y - (dotContainer.Height / 2));
                        //Canvas.SetLeft(dotContainer, puzzle.Dots[i].X - (dotContainer.Width / 2));
                        //ColorPointCanvas.Children.Add(dotContainer);
                        figurePolyline.Points.Add(new Point(puzzle.Dots[i].X, puzzle.Dots[i].Y));

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        public void ClearFigureBoards(bool side)
        {
            if (side)
            {
                _mainWindow.PuzzleBoardElementL.Children.Clear();
                _mainWindow.CrayonElementLforPuzzleLine.Points.Clear();
            }
            else
            {
                _mainWindow.PuzzleBoardElementR.Children.Clear();
                _mainWindow.CrayonElementRforPuzzleLine.Points.Clear();
            }

        }

        public void DrawPuzzle(bool side)
        {

            try
            {
                int currentFigureIndex;
                List<DotPuzzle> figuresList;
                Canvas startFinishPointBoard;
                Polyline figurePolyline;
                if (side)
                {

                    currentFigureIndex = _mainWindow.IndexOfCurrentFigureLeft;
                    figuresList = _mainWindow.ListPuzzleL;
                    startFinishPointBoard = _mainWindow.PuzzleBoardElementL;
                    figurePolyline = _mainWindow.CrayonElementLforPuzzleLine;
                }
                else
                {
                    currentFigureIndex = _mainWindow.IndexOfCurrentFigureRight;
                    figuresList = _mainWindow.ListPuzzleR;
                    startFinishPointBoard = _mainWindow.PuzzleBoardElementR;
                    figurePolyline = _mainWindow.CrayonElementRforPuzzleLine;


                }

                if (figuresList[currentFigureIndex] != null)
                {
                    startFinishPointBoard.Children.Clear();
                    figurePolyline.Points.Clear();



                    if (_mainWindow.Configuration.HandsState == 1)
                    {
                        ClearFigureBoards(!side);
                    }
                    else
                    {
                        ClearFigureBoards(side);
                    }






                    for (int i = 0; i < figuresList[currentFigureIndex].Dots.Count; i++)
                    {
                        Grid dotContainer = new Grid();
                        dotContainer.Width = 80;
                        dotContainer.Height = 80;
                        if (i == 0 || i == figuresList[currentFigureIndex].Dots.Count - 1)
                        {
                            if(i ==0)
                                dotContainer.Children.Add(new Canvas() {  Background = new ImageBrush(ConvertBitmapToBitmapSource((Resources.greenBall))) }); // do punktu startowego i koncowego potrzebna grafika, najlepiej jakas animacja
                            if(i == figuresList[currentFigureIndex].Dots.Count - 1)
                                dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Resources.redBall))) });
                            
                            
                            //TextBlock dotLabel = new TextBlock();
                            ////dotLabel.Text = (i + 1).ToString();
                            //dotLabel.Foreground = Brushes.White;
                            //dotLabel.FontSize = 35;
                            //dotLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            //dotLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            //dotContainer.Children.Add(dotLabel);

                            Canvas.SetTop(dotContainer, figuresList[currentFigureIndex].Dots[i].Y - (dotContainer.Height / 2));
                            Canvas.SetLeft(dotContainer, figuresList[currentFigureIndex].Dots[i].X - (dotContainer.Width / 2));
                            startFinishPointBoard.Children.Add(dotContainer);

                        }
                        else
                            dotContainer.Children.Add(new Ellipse() { Fill = figurePolyline.Stroke });

                        figurePolyline.Points.Add(new Point(figuresList[currentFigureIndex].Dots[i].X, figuresList[currentFigureIndex].Dots[i].Y));

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }

        public void SetNewFigure(ref int currentPointIndex, Polyline statusPolilyline, Canvas startFinishPointBoard, ref int currentFigureIndex)// czyœci ekran, ustala index nowej figury i jej bie¿¹cego punktu
        {


            startFinishPointBoard.Children.Clear();
            statusPolilyline.Points.Clear();
            currentPointIndex = -1;
            currentFigureIndex++;
        }
    }
}