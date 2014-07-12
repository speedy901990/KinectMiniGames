using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LettersGame
{
    class Letter
    {
        private string smallLetter;

        public string SmallLetter
        {
            get { return smallLetter; }
            set { smallLetter = value; }
        }

        private string bigLetter;

        public string BigLetter
        {
            get { return bigLetter; }
            set { bigLetter = value; }
        }

        private string word;

        public string Word
        {
            get { return word; }
            set { word = value; }
        }

        public Letter(string letter)
        {
            this.smallLetter = letter.ToLower();
            this.bigLetter = this.smallLetter.ToUpper();
        }

        public Letter(string letter, string word)
        {
            this.smallLetter = letter.ToLower();
            this.bigLetter = letter.ToUpper();
            this.word = word;
        }
    }
}
