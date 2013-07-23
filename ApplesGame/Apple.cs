using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplesGame
{
    public class Apple
    {
        //pozycja jablka
        private int posX;
        private int posY;

        //rozmiar
        private int size;

        //konstruktor tworzacy jablko
        public Apple(int xMin, int xMax, int yMin, int yMax)
        {
            System.Random rand = new Random(Seed);
            rand.Next(xMin, xMax);
            setPosition(rand.Next(xMin, xMax), rand.Next(yMin, yMax));
        }
        public void setPosition(int x, int y)
        {
            this.posX = x;
            this.posY = y;
        }

        public int Seed { get; set; }
    }
}
