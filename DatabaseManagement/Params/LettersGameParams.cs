using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagement.Params
{
    public class LettersGameParams
    {
        public int Level { get; set; }

        public int LettersCount { get; set; }

        public int Failures { get; set; }

        public int Time { get; set; }

        public int CorrectTrials { get; set; }
    }
}
