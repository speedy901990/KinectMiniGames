using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainOfWords.Model
{
    public class Score
    {
        public int CorrectTrials { get; set; }

        public int Failures { get; set; }

        public DateTime Time { get; set; }
    }
}
