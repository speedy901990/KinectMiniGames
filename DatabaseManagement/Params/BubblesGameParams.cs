using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement.Params
{
    public class BubblesGameParams
    {
        public BubblesGameParams()
        {

        }

        private int time;

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        private int success;

        public int Success
        {
            get { return success; }
            set { success = value; }
        }

        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        
        
    }
}
