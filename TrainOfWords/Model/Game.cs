using System;
using System.Collections.Generic;
using System.Threading;
using DatabaseManagement.Managers;
using DatabaseManagement.Params;
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

        public virtual void SaveResult()
        {
            SaveResultsThread = new Thread(SaveToDatabase);
            SaveResultsThread.Start();
        }

        protected virtual void SaveToDatabase()
        {
            Score.Time = DateTime.Now - StartTime;
            var results = new TrainOfWordsParams
            {
                Level = Config.Level,
                CorrectTrials = Score.CorrectTrials,
                Failures = Score.Failures,
                Time = (int)Score.Time.TotalMilliseconds
            };

            var manager = new TrainOfWordsManager(Config.Player);
            manager.SaveGameResult(results);
        }

        protected DateTime StartTime;
    }
}
