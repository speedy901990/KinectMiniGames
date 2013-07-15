//------------------------------------------------------------------------------
// <copyright file="FallingShapes.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

// This module contains code to do display falling shapes, and do
// hit testing against a set of segments provided by the Kinect NUI, and
// have shapes react accordingly.

namespace ShapeGame.Utils
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.Kinect;

    [Flags]
    public enum PolyType
    {
        None = 0x00,
        Triangle = 0x01,
        Square = 0x02,
        Star = 0x04,
        Pentagon = 0x08,
        Hex = 0x10,
        Star7 = 0x20,
        Circle = 0x40,
        Bubble = 0x80,
        All = 0x7f
    }

    [Flags]
    public enum HitType
    {
        None = 0x00,
        Hand = 0x01,
        Arm = 0x02,
        Squeezed = 0x04,
        Popped = 0x08
    }

    public enum GameMode
    {
        Off = 0,
        Solo = 1,
        //TwoPlayer = 2
    }

    // For hit testing, a dictionary of BoneData items, keyed off the endpoints
    // of a segment (Bone) is used.  The velocity of these endpoints is estimated
    // and used during hit testing and updating velocity vectors after a hit.
    public struct Bone
    {
        public JointType Joint1;
        public JointType Joint2;

        public Bone(JointType j1, JointType j2)
        {
            Joint1 = j1;
            Joint2 = j2;
        }
    }

    public struct Segment
    {
        public double X1;
        public double Y1;
        public double X2;
        public double Y2;
        public double Radius;

        public Segment(double x, double y)
        {
            Radius = 1;
            X1 = X2 = x;
            Y1 = Y2 = y;
        }

        public Segment(double x1, double y1, double x2, double y2)
        {
            Radius = 1;
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public bool IsCircle()
        {
            return (X1 == X2) && (Y1 == Y2);
        }
    }

    public struct BoneData
    {
        public Segment Segment;
        public Segment LastSegment;
        public double XVelocity;
        public double YVelocity;
        public double XVelocity2;
        public double YVelocity2;
        public DateTime TimeLastUpdated;

        private const double Smoothing = 0.8;

        public BoneData(Segment s)
        {
            Segment = LastSegment = s;
            XVelocity = YVelocity = 0;
            XVelocity2 = YVelocity2 = 0;
            TimeLastUpdated = DateTime.Now;
        }

        // Update the segment's position and compute a smoothed velocity for the circle or the
        // endpoints of the segment based on  the time it took it to move from the last position
        // to the current one.  The velocity is in pixels per second.
        public void UpdateSegment(Segment s)
        {
            LastSegment = Segment;
            Segment = s;
            
            DateTime cur = DateTime.Now;
            double fMs = cur.Subtract(TimeLastUpdated).TotalMilliseconds;
            if (fMs < 10.0)
            {
                fMs = 10.0;
            }

            double fps = 1000.0 / fMs;
            TimeLastUpdated = cur;

            if (Segment.IsCircle())
            {
                XVelocity = (XVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.X1 - LastSegment.X1) * fps);
                YVelocity = (YVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.Y1 - LastSegment.Y1) * fps);
            }
            else
            {
                XVelocity = (XVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.X1 - LastSegment.X1) * fps);
                YVelocity = (YVelocity * Smoothing) + ((1.0 - Smoothing) * (Segment.Y1 - LastSegment.Y1) * fps);
                XVelocity2 = (XVelocity2 * Smoothing) + ((1.0 - Smoothing) * (Segment.X2 - LastSegment.X2) * fps);
                YVelocity2 = (YVelocity2 * Smoothing) + ((1.0 - Smoothing) * (Segment.Y2 - LastSegment.Y2) * fps);
            }
        }

        // Using the velocity calculated above, estimate where the segment is right now.
        public Segment GetEstimatedSegment(DateTime cur)
        {
            Segment estimate = Segment;
            double fMs = cur.Subtract(TimeLastUpdated).TotalMilliseconds;
            estimate.X1 += fMs * XVelocity / 1000.0;
            estimate.Y1 += fMs * YVelocity / 1000.0;
            if (Segment.IsCircle())
            {
                estimate.X2 = estimate.X1;
                estimate.Y2 = estimate.Y1;
            }
            else
            {
                estimate.X2 += fMs * XVelocity2 / 1000.0;
                estimate.Y2 += fMs * YVelocity2 / 1000.0;
            }

            return estimate;
        }
    }

    // BannerText generates a scrolling or still banner of text (along the bottom of the screen).
    // Only one banner exists at a time.  Calling NewBanner() will erase the old one and start the new one.
    /*public class BannerText
    {
        private readonly Color color;
        private readonly string text;
        private readonly bool doScroll;
        private static BannerText myBannerText;
        private Brush brush;
        private Label label;
        private Rect boundsRect;
        private Rect renderedRect;
        private double offset;

        public BannerText(string s, Rect rect, bool scroll, Color col)
        {
            text = s;
            boundsRect = rect;
            doScroll = scroll;
            brush = null;
            label = null;
            color = col;
            offset = doScroll ? 1.0 : 0.0;
        }

        public static void NewBanner(string s, Rect rect, bool scroll, Color col)
        {
            myBannerText = (s != null) ? new BannerText(s, rect, scroll, col) : null;
        }

        public static void UpdateBounds(Rect rect)
        {
            if (myBannerText == null)
            {
                return;
            }

            myBannerText.boundsRect = rect;
            myBannerText.label = null;
        }

        public static void Draw(UIElementCollection children)
        {
            if (myBannerText == null)
            {
                return;
            }

            Label text = myBannerText.GetLabel();
            if (text == null)
            {
                myBannerText = null;
                return;
            }

            children.Add(text);
        }

        private Label GetLabel()
        {
            if (brush == null)
            {
                brush = new SolidColorBrush(color);
            }

            if (label == null)
            {
                //label = FallingThings.MakeSimpleLabel(text, boundsRect, brush);
                if (doScroll)
                {
                    label.FontSize = Math.Max(20, boundsRect.Height / 30);
                    label.Width = 10000;
                }
                else
                {
                    label.FontSize = Math.Min(
                        Math.Max(10, boundsRect.Width * 2 / text.Length), Math.Max(10, boundsRect.Height / 20));
                }

                label.VerticalContentAlignment = VerticalAlignment.Bottom;
                label.HorizontalContentAlignment = doScroll
                                                            ? HorizontalAlignment.Left
                                                            : HorizontalAlignment.Center;
                label.SetValue(Canvas.LeftProperty, offset * boundsRect.Width);
            }

            renderedRect = new Rect(label.RenderSize);

            if (doScroll)
            {
                offset -= 0.0015;
                if (offset * boundsRect.Width < boundsRect.Left - 10000)
                {
                    return null;
                }

                label.SetValue(Canvas.LeftProperty, (offset * boundsRect.Width) + boundsRect.Left);
            }

            return label;
        }
    }*/
}
