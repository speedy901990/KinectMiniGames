using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace DrawingGame
{
    public class Tracker
    {
        private MainWindow _mainWindow;

        public Tracker(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public Point GetCoordinatesFromJoint(Joint joint)
        {
            Point point = new Point();
            point.X = (joint.Position.X *_mainWindow.Player.PlayerScale) + _mainWindow.Player.PlayerCenter.X;
            point.Y = _mainWindow.Player.PlayerCenter.Y - (joint.Position.Y *_mainWindow.Player.PlayerScale);

            return point;

        }

        public void TrackPuzzle(Joint joint, ref int currentPointIndex, List<DotPuzzle> figuresList, Polyline statusPolilyline, Canvas startFinishPointBoard, ref int currentFigureIndex, ref bool sideIsDone)
        {
            Grid dotContainer = new Grid();
            dotContainer.Width = 80;
            dotContainer.Height = 80;


            if (!sideIsDone)
            {
                if (currentPointIndex == figuresList[currentFigureIndex].Dots.Count - 1)
                {
                    _mainWindow.Draftsman.SetNewFigure(ref currentPointIndex, statusPolilyline, startFinishPointBoard, ref currentFigureIndex);

                    if (figuresList.Count == currentFigureIndex) // bie¿¹ca figura jest ostatnia dla danej strony
                    {
                        currentFigureIndex = 0;
                        sideIsDone = true;

                        _mainWindow.Draftsman.ClearFigureBoards(_mainWindow.CurrentHand);

                        if (_mainWindow.LRaeady == true && _mainWindow.RRaeady == true)
                        {
                            _mainWindow.Draftsman.ClearFigureBoards(!_mainWindow.CurrentHand);
                            _mainWindow.Draftsman.ClearFigureBoards(_mainWindow.CurrentHand);
                            _mainWindow.EndGame();
                        }
                        if (!_mainWindow.GameOver)
                        {
                            if (_mainWindow.Configuration.HandsState == 1) // dzia³a w trypie naprzemiennym
                            {
                                _mainWindow.CurrentHand = !_mainWindow.CurrentHand;
                                _mainWindow.Draftsman.DrawPuzzle(_mainWindow.CurrentHand);
                            }
                        }
                    }
                    else
                    {
                        if (_mainWindow.Configuration.HandsState == 2)
                        {
                            //  DrawPuzzle(figuresList[currentFigureIndex], startFinishPointBoard, figurePolyline);
                            _mainWindow.Draftsman.DrawPuzzle(_mainWindow.CurrentHand);
                            _mainWindow.Draftsman.DrawPuzzle(!_mainWindow.CurrentHand);
                        }
                        else if (_mainWindow.Configuration.HandsState == 1)
                        {
                            _mainWindow.Draftsman.ClearFigureBoards(_mainWindow.CurrentHand);
                            _mainWindow.CurrentHand = !_mainWindow.CurrentHand;
                            _mainWindow.Draftsman.DrawPuzzle(_mainWindow.CurrentHand);
                        }
                    }
                }
                else
                {
                    Point dot;

                    if (currentPointIndex + 1 < figuresList[currentFigureIndex].Dots.Count)
                    {
                        dot = figuresList[currentFigureIndex].Dots[currentPointIndex + 1];
                    }
                    else
                    {
                        dot = figuresList[currentFigureIndex].Dots[0];
                    }


                    Point handPoint = GetCoordinatesFromJoint(joint);


                    Point dotDiff = new Point(dot.X - handPoint.X, dot.Y - handPoint.Y);
                    double length = Math.Sqrt(dotDiff.X * dotDiff.X + dotDiff.Y * dotDiff.Y);

                    int lastPoint = statusPolilyline.Points.Count - 1;


                    if (length < _mainWindow.Precision)
                    {
                        _mainWindow.StopwatchOfOutOfField.Stop();
                        if (lastPoint > 0)
                        {

                            statusPolilyline.Points.RemoveAt(lastPoint);
                            //startFinishPointBoard.Children.Remove(dotContainer);
                        }


                        //dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.greenBall))) });
                        //Canvas.SetTop(dotContainer, dot.X + 40);
                        //Canvas.SetLeft(dotContainer, dot.X - 40);
                        //startFinishPointBoard.Children.Add(dotContainer);
                        
                        statusPolilyline.Points.Add(new Point(dot.X, dot.Y));


                        statusPolilyline.Points.Add(new Point(dot.X, dot.Y));


                        currentPointIndex++;

                    }
                    else
                    {

                        if (lastPoint > 0)
                        {
                            //startFinishPointBoard.Children.Remove(dotContainer);
                            //dotContainer.Children.Add(new Canvas() { Background = new ImageBrush(ConvertBitmapToBitmapSource((Properties.Resources.yellowBall))) });
                            //Canvas.SetTop(dotContainer, dot.X + 40);
                            //Canvas.SetLeft(dotContainer, dot.X - 40);
                            //startFinishPointBoard.Children.Add(dotContainer);

                            _mainWindow.StopwatchOfOutOfField.Start();

                            Point lineEndpoint = statusPolilyline.Points[lastPoint];
                            statusPolilyline.Points.RemoveAt(lastPoint);
                            lineEndpoint.X = handPoint.X;
                            lineEndpoint.Y = handPoint.Y;
                            statusPolilyline.Points.Add(lineEndpoint);
                        }
                    }
                }
            }
        }

        public void TrackPuzzle(Joint joint, ref int puzzleDotCurrentIndex, List<DotPuzzle> listOfPuzzle, Polyline crayonElement, Canvas puzzleBoardElement, ref int indexOfCurrentFigure, ref bool sideIsDone, Polyline crayonElementForPuzzleLine)
        {
            if (!sideIsDone)
            {

                if (puzzleDotCurrentIndex == listOfPuzzle[indexOfCurrentFigure].Dots.Count - 1)
                {
                    puzzleBoardElement.Children.Clear();
                    crayonElement.Points.Clear();
                    puzzleDotCurrentIndex = -1;
                    indexOfCurrentFigure++;
                    //if (_Configuration.HandsState == 1)
                    //{
                    //    _currentHand = !_currentHand;
                    //}

                    if (listOfPuzzle.Count == indexOfCurrentFigure)
                    {
                        indexOfCurrentFigure = 0;
                        sideIsDone = true;
                        crayonElementForPuzzleLine.Points.Clear();
                        if (_mainWindow.LRaeady == true && _mainWindow.RRaeady == true)
                        {
                            _mainWindow.Draftsman.ClearFigureBoards(!_mainWindow.CurrentHand);
                            _mainWindow.Draftsman.ClearFigureBoards(_mainWindow.CurrentHand);
                            _mainWindow.EndGame();
                        }

                    }
                    else
                    {
                        if (_mainWindow.Configuration.HandsState == 2)
                        {
                            _mainWindow.Draftsman.DrawPuzzle(listOfPuzzle[indexOfCurrentFigure], puzzleBoardElement, crayonElementForPuzzleLine);
                        }
                        else if (_mainWindow.Configuration.HandsState == 1)
                        {
                            puzzleBoardElement.Children.Clear();
                            _mainWindow.CurrentHand = !_mainWindow.CurrentHand;
                            if (!_mainWindow.CurrentHand)
                            {
                                _mainWindow.Draftsman.DrawPuzzle(_mainWindow.ListPuzzleR[indexOfCurrentFigure], _mainWindow.PuzzleBoardElementR, _mainWindow.CrayonElementRforPuzzleLine);
                                _mainWindow.PuzzleBoardElementL.Children.Clear();
                                _mainWindow.CrayonElementLforPuzzleLine.Points.Clear();
                            }
                            else
                            {
                                _mainWindow.Draftsman.DrawPuzzle(_mainWindow.ListPuzzleL[indexOfCurrentFigure - 1], _mainWindow.PuzzleBoardElementL, _mainWindow.CrayonElementLforPuzzleLine);
                                _mainWindow.PuzzleBoardElementR.Children.Clear();
                                _mainWindow.CrayonElementRforPuzzleLine.Points.Clear();
                            }
                        }
                    }
                }
                else
                {
                    Point dot;

                    if (puzzleDotCurrentIndex + 1 < listOfPuzzle[indexOfCurrentFigure].Dots.Count)
                    {
                        dot = listOfPuzzle[indexOfCurrentFigure].Dots[puzzleDotCurrentIndex + 1];
                    }
                    else
                    {
                        dot = listOfPuzzle[indexOfCurrentFigure].Dots[0];
                    }


                    Point handPoint = GetCoordinatesFromJoint(joint);


                    Point dotDiff = new Point(dot.X - handPoint.X, dot.Y - handPoint.Y);
                    double length = Math.Sqrt(dotDiff.X * dotDiff.X + dotDiff.Y * dotDiff.Y);

                    int lastPoint = crayonElement.Points.Count - 1;


                    if (length < _mainWindow.Precision)
                    {
                        _mainWindow.StopwatchOfOutOfField.Stop();
                        if (lastPoint > 0)
                        {

                            crayonElement.Points.RemoveAt(lastPoint);
                        }


                        crayonElement.Points.Add(new Point(dot.X, dot.Y));


                        crayonElement.Points.Add(new Point(dot.X, dot.Y));


                        puzzleDotCurrentIndex++;

                    }
                    else
                    {

                        if (lastPoint > 0)
                        {
                            _mainWindow.StopwatchOfOutOfField.Start();
                            Point lineEndpoint = crayonElement.Points[lastPoint];
                            crayonElement.Points.RemoveAt(lastPoint);
                            lineEndpoint.X = handPoint.X;
                            lineEndpoint.Y = handPoint.Y;
                            crayonElement.Points.Add(lineEndpoint);
                        }
                    }
                }
            }
        }
    }
}