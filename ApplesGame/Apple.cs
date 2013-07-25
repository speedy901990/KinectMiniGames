using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesGame
{
    public class Apple
    {
        #region private fields
        
        private int size;
        private ApplePosition pos;
        private string  color;
        
        #endregion private fields

        public Apple(int xMin, int xMax, int yMin, int yMax)
        {
            System.Random rand = new Random(Seed);
            rand.Next(xMin, xMax);
            pos = new ApplePosition(rand.Next(xMin, xMax), rand.Next(yMin, yMax));
        }

        public int Seed { get; set; }

        #region getters
        public ApplePosition getPosition()
        {
            return this.pos;
        }

        public int getSize()
        {
            return this.size;
        }

        public string getColor()
        {
            return this.color;
        }
        #endregion getters


        #region setters
        private void setSize(int sizeParam)
        {
            this.size = sizeParam;
        }

        private void setColor(string colourParam)
        {
            this.color = colourParam;
        }
        #endregion setters


    }

    public class ApplePosition
    {
        private int posX;
        private int posY;

        public ApplePosition(int x, int y)
        {
            this.posX = x;
            this.posY = y;
        }
    }
}
