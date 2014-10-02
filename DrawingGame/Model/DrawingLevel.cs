using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DrawingGame.Model
{
    public class DrawingLevel
    {
        public List<Point> Points { get; set; }
        public int Side;
        public int Difficulty;
    }
}
