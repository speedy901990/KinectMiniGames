//------------------------------------------------------------------------------
// <copyright file="FlyingText.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace ShapeGame
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    // FlyingText creates text that flys out from a given point, and fades as it gets bigger.
    // NewFlyingText() can be called as often as necessary, and there can be many texts flying out at once.
    public class FlyingText
    {
        private static readonly List<FlyingText> FlyingTexts = new List<FlyingText>();
        private readonly double fontGrow;
        private readonly string text;
        private Point center;
        private Brush brush;
        private double fontSize;
        private double alpha;
        private Label label;

        public FlyingText(string s, double size, Point center)
        {
            text = s;
            fontSize = Math.Max(1, size);
            fontGrow = Math.Sqrt(size) * 0.4;
            this.center = center;
            alpha = 1.0;
            label = null;
            brush = null;
        }

        public static void NewFlyingText(double size, Point center, string s)
        {
            FlyingTexts.Add(new FlyingText(s, size, center));
        }

        public static void Draw(UIElementCollection children)
        {
            for (int i = 0; i < FlyingTexts.Count; i++)
            {
                FlyingText flyout = FlyingTexts[i];
                if (flyout.alpha <= 0)
                {
                    FlyingTexts.Remove(flyout);
                    i--;
                }
            }

            foreach (var flyout in FlyingTexts)
            {
                flyout.Advance();
                children.Add(flyout.label);
            }
        }

        private void Advance()
        {
            alpha -= 0.01;
            if (alpha < 0)
            {
                alpha = 0;
            }

            if (brush == null)
            {
                brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }

            if (label == null)
            {
                //label = FallingThings.MakeSimpleLabel(text, new Rect(0, 0, 0, 0), brush);
            }

            brush.Opacity = Math.Pow(alpha, 1.5);
            label.Foreground = brush;
            fontSize += fontGrow;
            label.FontSize = Math.Max(1, fontSize);
            Rect renderRect = new Rect(label.RenderSize);
            label.SetValue(Canvas.LeftProperty, center.X - (renderRect.Width / 2));
            label.SetValue(Canvas.TopProperty, center.Y - (renderRect.Height / 2));
        }
    }
}
