using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrawingGame
{
    public class Points
    {
        public Points()
        {
            this.score = 0;
            this.rate = "";
        }
        public int score { get; set; }
        public string rate { get; set; }
    }
}
