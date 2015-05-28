using System.Collections.Generic;

namespace TrainOfWords.Resources
{
    public static class WordsContainer
    {
        public static List<string> Words3Chars { get; private set; }
        public static List<string> Words4Chars { get; private set; }
        public static List<string> Words5Chars { get; private set; }
        public static List<string> Alphabet { get; private set; } 

        static WordsContainer()
        {
            Words3Chars = new List<string>
            {
                "kot",
                "oko"
            };
            Words4Chars = new List<string>
            {
                "noga",
                "ryba"
            };
            Words5Chars = new List<string>
            {
                "ekran",
                "ławka",
                "lizak",
                "małpa",
                "okręt",
                "torba",
                "wózek",
                "zegar"
            };

            Alphabet = new List<string>
            {
                "a",
                "ą",
                "b",
                "c",
                "ć",
                "d",
                "e",
                "ę",
                "f",
                "g",
                "h",
                "i",
                "j",
                "k",
                "l",
                "ł",
                "m",
                "n",
                "ń",
                "o",
                "ó",
                "p",
                "r",
                "s",
                "ś",
                "t",
                "u",
                "v",
                "w",
                "x",
                "y",
                "z",
                "ź",
                "ż"
            };
        }
    }
}