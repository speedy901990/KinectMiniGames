using System.Collections.Generic;
using System.Windows;

namespace DrawingGame
{
    public class DotPuzzle
    {
        public DotPuzzle()
        {
            Dots = new List<Point>();
        }

        public List<Point> Dots { get; set; }
    }
}
