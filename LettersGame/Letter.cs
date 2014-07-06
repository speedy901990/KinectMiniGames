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

        public Letter(string letter)
        {
            this.smallLetter = letter.ToLower();
            this.bigLetter = this.smallLetter.ToUpper();
        }
    }
}
