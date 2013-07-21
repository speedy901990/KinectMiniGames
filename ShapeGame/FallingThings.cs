//------------------------------------------------------------------------------
// <copyright file="FallingThings.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

// This module contains code to do display falling shapes, and do
// hit testing against a set of segments provided by the Kinect NUI, and
// have shapes react accordingly.

using System.Security.AccessControl;

namespace BubblesGame
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Microsoft.Kinect;
    using Utils;

    // FallingThings is the main class to draw and maintain positions of falling shapes.  It also does hit testing
    // and appropriate bouncing.
    public class FallingThings
    {
        private const double BaseGravity = 0.017;
        private const double BaseAirFriction = 0.994;

        private readonly Dictionary<PolyType, PolyDef> _polyDefs = new Dictionary<PolyType, PolyDef>
            {
                /*{ PolyType.Triangle, new PolyDef { Sides = 3, Skip = 1 } },
                { PolyType.Star, new PolyDef { Sides = 5, Skip = 2 } },
                { PolyType.Pentagon, new PolyDef { Sides = 5, Skip = 1 } },
                { PolyType.Square, new PolyDef { Sides = 4, Skip = 1 } },
                { PolyType.Hex, new PolyDef { Sides = 6, Skip = 1 } },
                { PolyType.Star7, new PolyDef { Sides = 7, Skip = 3 } },
                */{ PolyType.Circle, new PolyDef { Sides = 1, Skip = 1 } },
                //{ PolyType.Bubble, new PolyDef { Sides = 0, Skip = 1 } }
            };

        private readonly List<Thing> _things = new List<Thing>();
        private readonly Random _rnd = new Random();
        private readonly int _maxThings;
        private readonly int _intraFrames = 1;
        private readonly Dictionary<int, int> _scores = new Dictionary<int, int>();
        private const double DissolveTime = 0.4;
        private Rect _sceneRect;
        private double _targetFrameRate = 60;
        private double _dropRate = 2.0;
        private double _shapeSize = 1.0;
        private double _baseShapeSize = 20;
        private GameMode _gameMode = GameMode.Off;
        private double _gravity = BaseGravity;
        private double _gravityFactor = 1.0;
        private double _airFriction = BaseAirFriction;
        private int _frameCount;
        private bool _doRandomColors = true;
        private double _expandingRate = 1.0;
        private Color _baseColor = Color.FromRgb(0, 0, 0);
        private PolyType _polyTypes = PolyType.All;
        private DateTime _gameStartTime;

        public FallingThings(int maxThings, double framerate, int intraFrames)
        {
            _maxThings = maxThings;
            _intraFrames = intraFrames;
            _targetFrameRate = framerate * intraFrames;
            SetGravity(_gravityFactor);
            _sceneRect.X = _sceneRect.Y = 0;
            _sceneRect.Width = _sceneRect.Height = 100;
            _shapeSize = _sceneRect.Height * _baseShapeSize / 1000.0;
            _expandingRate = Math.Exp(Math.Log(6.0) / (_targetFrameRate * DissolveTime));
        }

        public enum ThingState
        {
            Falling = 0,
            Bouncing = 1,
            Dissolving = 2,
            Remove = 3
        }

        public void SetFramerate(double actualFramerate)
        {
            _targetFrameRate = actualFramerate * _intraFrames;
            _expandingRate = Math.Exp(Math.Log(6.0) / (_targetFrameRate * DissolveTime));
            if (_gravityFactor != 0)
            {
                SetGravity(_gravityFactor);
            }
        }

        public void SetBoundaries(Rect r)
        {
            _sceneRect = r;
            _shapeSize = r.Height * _baseShapeSize / 1000.0;
        }

        public void SetDropRate(double f)
        {
            _dropRate = f;
        }

        public void SetSize(double f)
        {
            _baseShapeSize = f;
            _shapeSize = _sceneRect.Height * _baseShapeSize / 1000.0;
        }

        public void SetShapesColor(Color color, bool doRandom)
        {
            _doRandomColors = doRandom;
            _baseColor = color;
        }

        public void Reset()
        {
            for (int i = 0; i < _things.Count; i++)
            {
                Thing thing = _things[i];
                if ((thing.State == ThingState.Bouncing) || (thing.State == ThingState.Falling))
                {
                    thing.State = ThingState.Dissolving;
                    thing.Dissolve = 0;
                    _things[i] = thing;
                }
            }

            _gameStartTime = DateTime.Now;
            _scores.Clear();
        }
        public void SetGameMode(GameMode mode)
        {
            _gameMode = mode;
            _gameStartTime = DateTime.Now;
            _scores.Clear();
        }

        public void SetGravity(double f)
        {
            _gravityFactor = f;
            _gravity = f * BaseGravity / _targetFrameRate / Math.Sqrt(_targetFrameRate) / Math.Sqrt(_intraFrames);
            _airFriction = f == 0 ? 0.997 : Math.Exp(Math.Log(1.0 - ((1.0 - BaseAirFriction) / f)) / _intraFrames);

            if (f == 0)
            {
                // Stop all movement as well!
                for (var i = 0; i < _things.Count; i++)
                {
                    Thing thing = _things[i];
                    thing.XVelocity = thing.YVelocity = 0;
                    _things[i] = thing;
                }
            }
        }

        public void SetPolies(PolyType polies)
        {
            _polyTypes = polies;
        }

        public HitType LookForHits(Dictionary<Bone, BoneData> segments, int playerId)
        {
            var cur = DateTime.Now;
            var allHits = HitType.None;

            // Zero out score if necessary
            if (!_scores.ContainsKey(playerId))
            {
                _scores.Add(playerId, 0);
            }

            foreach (var pair in segments)
            {
                for (int i = 0; i < _things.Count; i++)
                {
                    var hit = HitType.None;
                    Thing thing = _things[i];
                    switch (thing.State)
                    {
                        //case ThingState.Bouncing:
                        case ThingState.Falling:
                            {
                                var hitCenter = new Point(0, 0);
                                double lineHitLocation = 0;
                                Segment seg = pair.Value.GetEstimatedSegment(cur);
                                if (thing.Hit(seg, ref hitCenter, ref lineHitLocation))
                                {
                                    double fMs = 1000;
                                    if (thing.TimeLastHit != DateTime.MinValue)
                                    {
                                        fMs = cur.Subtract(thing.TimeLastHit).TotalMilliseconds;
                                        thing.AvgTimeBetweenHits = (thing.AvgTimeBetweenHits * 0.8) + (0.2 * fMs);
                                    }

                                    thing.TimeLastHit = cur;

                                    // Bounce off head and hands
                                    /*if (seg.IsCircle())
                                    {
                                        if (fMs > 100.0)
                                        {
                                            hit |= HitType.Hand;
                                        }
                                    }
                                    else
                                    {
                                        if (fMs > 100.0)
                                        {
                                    */        
                                    hit |= HitType.Hand;
                                    //    }
                                    //}
                                    if (seg.IsCircle())
                                    {
                                        if (thing.State == ThingState.Falling)
                                        {
                                            thing.State = ThingState.Bouncing;
                                            thing.Hotness = 1;
                                            thing.FlashCount = 0;
                                        }
                                     }
                                    _things[i] = thing;

                                    hit |= HitType.Popped;
                                }
                            }

                            break;
                    }

                    if ((hit & HitType.Popped) != 0)
                    {
                        thing.State = ThingState.Dissolving;
                        thing.Dissolve = 0;
                        thing.XVelocity = thing.YVelocity = 0;
                        _things[i] = thing;
                    }

                    allHits |= hit;
                }
            }

            return allHits;
        }

        public void AdvanceFrame()
        {
            // Move all things by one step, accounting for gravity
            for (int thingIndex = 0; thingIndex < _things.Count; thingIndex++)
            {
                Thing thing = _things[thingIndex];
                thing.Center.Offset(thing.XVelocity, thing.YVelocity);
                thing.YVelocity += _gravity * _sceneRect.Height;
                thing.YVelocity *= _airFriction;
                thing.XVelocity *= _airFriction;

                if ((thing.Center.X - thing.Size < 0) || (thing.Center.X + thing.Size > _sceneRect.Width))
                {
                    thing.XVelocity = -thing.XVelocity;
                    thing.Center.X += thing.XVelocity;
                }

                // Then get rid of one if any that fall off the bottom
                if (thing.Center.Y - thing.Size > _sceneRect.Bottom)
                {
                    thing.State = ThingState.Remove;
                }

                // Get rid of after dissolving.
                if (thing.State == ThingState.Dissolving)
                {
                    thing.Dissolve += 1 / (_targetFrameRate * DissolveTime);
                    thing.Size *= _expandingRate;
                    if (thing.Dissolve >= 1.0)
                    {
                        thing.State = ThingState.Remove;
                    }
                }

                _things[thingIndex] = thing;
            }

            // Then remove any that should go away now
            for (int i = 0; i < _things.Count; i++)
            {
                Thing thing = _things[i];
                if (thing.State == ThingState.Remove)
                {
                    _things.Remove(thing);
                    i--;
                }
            }

            // Create any new things to drop based on dropRate
            if ((_things.Count < _maxThings) && (_rnd.NextDouble() < _dropRate / _targetFrameRate) && (_polyTypes != PolyType.None))
            {
                PolyType[] alltypes = 
                {
                    //PolyType.Triangle, PolyType.Square, PolyType.Star, PolyType.Pentagon,
                    //PolyType.Hex, PolyType.Star7, 
                    PolyType.Circle, 
                    //PolyType.Bubble
                };
                byte r;
                byte g;
                byte b;

                if (_doRandomColors)
                {
                    r = (byte)(_rnd.Next(215) + 40);
                    g = (byte)(_rnd.Next(215) + 40);
                    b = (byte)(_rnd.Next(215) + 40);
                }
                else
                {
                    r = (byte)Math.Min(255.0, _baseColor.R * (0.7 + (_rnd.NextDouble() * 0.7)));
                    g = (byte)Math.Min(255.0, _baseColor.G * (0.7 + (_rnd.NextDouble() * 0.7)));
                    b = (byte)Math.Min(255.0, _baseColor.B * (0.7 + (_rnd.NextDouble() * 0.7)));
                }

                PolyType tryType;
                do
                {
                    tryType = alltypes[_rnd.Next(alltypes.Length)];
                }
                while ((_polyTypes & tryType) == 0);

                DropNewThing(tryType, _shapeSize, Color.FromRgb(r, g, b));
            }
        }

        public void DrawFrame(UIElementCollection children)
        {
            _frameCount++;

            // Draw all shapes in the scene
            for (var i = 0; i < _things.Count; i++)
            {
                Thing thing = _things[i];
                if (thing.Brush == null)
                {
                    thing.Brush = new SolidColorBrush(thing.Color);
                    double factor = 0.4 + (((double)thing.Color.R + thing.Color.G + thing.Color.B) / 1600);
                    thing.Brush2 =
                        new SolidColorBrush(
                            Color.FromRgb(
                                (byte)(255 - ((255 - thing.Color.R) * factor)),
                                (byte)(255 - ((255 - thing.Color.G) * factor)),
                                (byte)(255 - ((255 - thing.Color.B) * factor))));
                    thing.BrushPulse = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }

                    if (thing.State == ThingState.Dissolving)
                    {
                        thing.Brush.Opacity = 1.0 - (thing.Dissolve * thing.Dissolve);
                    }

                    children.Add(
                        MakeSimpleShape(
                            _polyDefs[thing.Shape].Sides,
                            _polyDefs[thing.Shape].Skip,
                            thing.Size,
                            thing.Center,
                            thing.Brush,
                            (thing.State == ThingState.Dissolving) ? null : thing.Brush2,
                            1,
                            0.2));
                
            }
        }

        private static double SquaredDistance(double x1, double y1, double x2, double y2)
        {
            return ((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1));
        }

        private void DropNewThing(PolyType newShape, double newSize, Color newColor)
        {
            // Only drop within the center "square" area 
            double dropWidth = _sceneRect.Bottom - _sceneRect.Top;
            if (dropWidth > _sceneRect.Right - _sceneRect.Left)
            {
                dropWidth = _sceneRect.Right - _sceneRect.Left;
            }

            var newThing = new Thing
            {
                Size = newSize,
                YVelocity = ((0.5 * _rnd.NextDouble()) - 0.25) / _targetFrameRate,
                XVelocity = 0,
                Shape = newShape,
                Center = new Point((_rnd.NextDouble() * dropWidth) + ((_sceneRect.Left + _sceneRect.Right - dropWidth) / 2), _sceneRect.Top - newSize),
                TimeLastHit = DateTime.MinValue,
                AvgTimeBetweenHits = 100,
                Color = newColor,
                Brush = null,
                Brush2 = null,
                BrushPulse = null,
                Dissolve = 0,
                State = ThingState.Falling,
                Hotness = 0,
                FlashCount = 0
            };

            _things.Add(newThing);
        }

        private Shape MakeSimpleShape(
            int numSides,
            int skip,
            double size,
            Point center,
            Brush brush,
            Brush brushStroke,
            double strokeThickness,
            double opacity)
        {
            
                var circle = new Ellipse { Width = size * 2, Height = size * 2, Stroke = brushStroke };
                if (circle.Stroke != null)
                {
                    circle.Stroke.Opacity = opacity;
                }

                circle.StrokeThickness = strokeThickness * ((numSides == 1) ? 1 : 2);
                circle.Fill = (numSides == 1) ? brush : null;
                circle.SetValue(Canvas.LeftProperty, center.X - size);
                circle.SetValue(Canvas.TopProperty, center.Y - size);
                return circle;
            
        }

        internal struct PolyDef
        {
            public int Sides;
            public int Skip;
        }

        // The Thing struct represents a single object that is flying through the air, and
        // all of its properties.
        private struct Thing
        {
            public Point Center;
            public double Size;
            public double YVelocity;
            public double XVelocity;
            public PolyType Shape;
            public Color Color;
            public Brush Brush;
            public Brush Brush2;
            public Brush BrushPulse;
            public double Dissolve;
            public ThingState State;
            public DateTime TimeLastHit;
            public double AvgTimeBetweenHits;
            public int Hotness;                 // Score level
            public int FlashCount;

            // Hit testing between this thing and a single segment.  If hit, the center point on
            // the segment being hit is returned, along with the spot on the line from 0 to 1 if
            // a line segment was hit.
            public bool Hit(Segment seg, ref Point hitCenter, ref double lineHitLocation)
            {
                double minDxSquared = Size + seg.Radius;
                minDxSquared *= minDxSquared;

                // See if falling thing hit this body segment
                if (seg.IsCircle())
                {
                    if (SquaredDistance(Center.X, Center.Y, seg.X1, seg.Y1) <= minDxSquared)
                    {
                        hitCenter.X = seg.X1;
                        hitCenter.Y = seg.Y1;
                        lineHitLocation = 0;
                        return true;
                    }
                }
                else
                {
                    double sqrLineSize = SquaredDistance(seg.X1, seg.Y1, seg.X2, seg.Y2);
                    if (sqrLineSize < 0.5)
                    {
                        // if less than 1/2 pixel apart, just check dx to an endpoint
                        return SquaredDistance(Center.X, Center.Y, seg.X1, seg.Y1) < minDxSquared;
                    }

                    // Find dx from center to line
                    double u = ((Center.X - seg.X1) * (seg.X2 - seg.X1)) + (((Center.Y - seg.Y1) * (seg.Y2 - seg.Y1)) / sqrLineSize);
                    if ((u >= 0) && (u <= 1.0))
                    {   // Tangent within line endpoints, see if we're close enough
                        double intersectX = seg.X1 + ((seg.X2 - seg.X1) * u);
                        double intersectY = seg.Y1 + ((seg.Y2 - seg.Y1) * u);

                        if (SquaredDistance(Center.X, Center.Y, intersectX, intersectY) < minDxSquared)
                        {
                            lineHitLocation = u;
                            hitCenter.X = intersectX;
                            hitCenter.Y = intersectY;
                            return true;
                        }
                    }
                    else
                    {
                        // See how close we are to an endpoint
                        if (u < 0)
                        {
                            if (SquaredDistance(Center.X, Center.Y, seg.X1, seg.Y1) < minDxSquared)
                            {
                                lineHitLocation = 0;
                                hitCenter.X = seg.X1;
                                hitCenter.Y = seg.Y1;
                                return true;
                            }
                        }
                        else
                        {
                            if (SquaredDistance(Center.X, Center.Y, seg.X2, seg.Y2) < minDxSquared)
                            {
                                lineHitLocation = 1;
                                hitCenter.X = seg.X2;
                                hitCenter.Y = seg.Y2;
                                return true;
                            }
                        }
                    }
                    return false;
                }
                return false;
            }
        }
    }
}
