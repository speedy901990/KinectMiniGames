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
        private int bubblesFallSpeed;
        private int bubblesCount;

        public BubblesGameConfig()
        {

        }

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public int BubblesFallSpeed
        {
            get { return this.bubblesFallSpeed; }
            set { this.bubblesFallSpeed = value; }
        }

        public int BubblesCount
        {
            get { return this.bubblesCount; }
            set { this.bubblesCount = value; }
        }
    }
}
