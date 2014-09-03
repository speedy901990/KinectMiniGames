using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainOfWords.View
{
    public class TrainOfWordsGameConfig
    {
        private const int NumberOfLetters = 12;

        //public Player Player { get; set; }

        public int NuberOfLetters { get { return NumberOfLetters; } }

        public int Level { get; set; }
    }
}
