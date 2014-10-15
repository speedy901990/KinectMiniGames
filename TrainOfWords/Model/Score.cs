using System;

namespace TrainOfWords.Model
{
    public class Score
    {
        public int CorrectTrials { get; set; }

        public int Failures { get; set; }

        public int LettersLeft { get; set; }

        public TimeSpan Time { get; set; }
    }
}
