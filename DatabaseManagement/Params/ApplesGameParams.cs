using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement.Params
{
    public class ApplesGameParams
    {
        private int failures;

        public int Failures
        {
            get { return failures; }
            set { failures = value; }
        }

        private int time;

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        private int correctTrials;

        public int CorrectTrials
        {
            get { return correctTrials; }
            set { correctTrials = value; }
        }

        private int apples;

        public int Apples
        {
            get { return apples; }
            set { apples = value; }
        }

        private int colors;

        public int Colors
        {
            get { return colors; }
            set { colors = value; }
        }

        private int baskets;

        public int Baskets
        {
            get { return baskets; }
            set { baskets = value; }
        }
        
        
        


        public ApplesGameParams()
        {

        }
    }
}
