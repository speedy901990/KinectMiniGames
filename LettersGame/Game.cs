using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Collections;

namespace LettersGame
{
    class Game
    {
        private List<Letter> letters;

        internal List<Letter> Letters
        {
            get { return letters; }
            set { letters = value; }
        }


        public Game(int numOfLetters = 8)
        {
            ResourceSet resourceSet = Resources.Letters.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            List<Letter> allLetters = new List<Letter>();
            foreach (DictionaryEntry item in resourceSet)
            {
                allLetters.Add(new Letter((string)item.Value));
            }
            Random rand = new Random();
            this.letters = new List<Letter>();
            for (int i = 0; i < numOfLetters; i++)
            {
                int index = rand.Next(allLetters.Count);
                this.letters.Add(allLetters[index]);
                allLetters.RemoveAt(index);
            }
        }
    }
}
