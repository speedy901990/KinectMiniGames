using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubblesGame
{
    public class BubblesGameConfig
    {
        private string username;
        private int bubbleSize;
        private int bubbleFallSpeed;

        public BubblesGameConfig()
        {

        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public int BubbleFallSpeed
        {
            get { return this.bubbleFallSpeed; }
            set { this.bubbleFallSpeed = value; }
        }
    }
}
