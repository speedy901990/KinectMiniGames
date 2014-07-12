using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement.Params
{
    public class LettersGameParams
    {
        public LettersGameParams()
        {

        }
        #region params
        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        private int lettersCount;

        public int LettersCount
        {
            get { return lettersCount; }
            set { lettersCount = value; }
        }
        #endregion

        #region history results
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

        #endregion



    }
}
