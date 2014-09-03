using System;
using System.Collections.Generic;
using TrainOfWords.View;

namespace TrainOfWords.Model
{
    public abstract class Game
    {
        public Dictionary<string, List<string>> Letters { get; protected set; }

        public List<Word> Words { get; protected set; }

        public TrainOfWordsGameConfig Config { get; protected set; }

        public Score Score { get; protected set; }

        protected Game(TrainOfWordsGameConfig config)
        {
            Config = config;
            Words = new List<Word>();
            Letters = new Dictionary<string, List<string>>();
        }

        public abstract void Run();

        public virtual void SaveResult(){}

        protected DateTime StartTime;
    }
}
