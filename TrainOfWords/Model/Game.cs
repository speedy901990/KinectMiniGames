using System;
using System.Collections.Generic;
using System.Threading;
using TrainOfWords.View;

namespace TrainOfWords.Model
{
    public abstract class Game
    {
        public Dictionary<string, List<string>> Letters { get; protected set; }

        public List<Word> Words { get; protected set; }

        public TrainOfWordsGameConfig Config { get; protected set; }

        public Score Score { get; protected set; }

        public bool FirstStepFinished { get; set; }

        public bool SecondStepFinished { get; set; }

        public bool ThirdStepFinished { get; set; }

        public Thread SaveResultsThread { get; set; }

        protected Game(TrainOfWordsGameConfig config)
        {
            Config = config;
            Words = new List<Word>();
            Letters = new Dictionary<string, List<string>>();
            Score = new Score {CorrectTrials = 0, Failures = 0};
            FirstStepFinished = false;
            SecondStepFinished = false;
            ThirdStepFinished = false;
        }

        public abstract void Run();

        public virtual void SaveResult(){}

        protected DateTime StartTime;
    }
}
