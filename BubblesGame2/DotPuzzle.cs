using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KinectBookSkeleton
{
    public class DotPuzzle
    {
        public DotPuzzle()
        {
            this.Dots = new List<Point>();
        }

        public List<Point> Dots { get; set; }
    }
}
