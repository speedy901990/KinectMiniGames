using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainOfWords.View
{
    public class TrainOfWordsGameConfig
    {
        private const int LettersOnScreen = 12;

        //public Player Player { get; set; }

        public int NuberOfLettersOnScreen { get { return LettersOnScreen; } }

        public int Level { get; set; }

        public int AllLettersCount { get; set; }
    }
}
