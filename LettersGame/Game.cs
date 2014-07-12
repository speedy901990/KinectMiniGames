﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Collections;
using System.Threading;
using DatabaseManagement.Managers;
using DatabaseManagement.Params;

namespace LettersGame
{
    class Game
    {
        #region private state
        private LettersGameConfig config;
        private List<Letter> smallLetters;
        private List<Letter> bigLetters;
        private int correctTrials;
        private int fails;
        private int lettersLeft;
        private DateTime startTime;
        private TimeSpan time;
        private Thread saveResultsThread;
        #endregion


        #region constructor
        public Game(LettersGameConfig config)
        {
            this.config = config;
            int numOfLetters = config.LettersCount;
            using (ResourceSet resourceSet = Resources.Letters.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
            {
                List<Letter> allLetters = new List<Letter>();
                foreach (DictionaryEntry item in resourceSet)
                {
                    allLetters.Add(new Letter((string)item.Value));
                }
                Random rand = new Random();
                this.smallLetters = new List<Letter>();
                this.bigLetters = new List<Letter>();
                for (int i = 0; i < numOfLetters; i++)
                {
                    int index = rand.Next(allLetters.Count);
                    var letter = allLetters[index];
                    this.SmallLetters.Add(letter);
                    this.BigLetters.Add(letter);
                    allLetters.RemoveAt(index);
                }
            }
            this.CorrectTrials = 0;
            this.Fails = 0;
            this.LettersLeft = numOfLetters;
            this.startTime = DateTime.Now;
        }
        #endregion

        #region public state
        internal List<Letter> BigLetters
        {
            get { return bigLetters; }
            set { bigLetters = value; }
        }

        internal List<Letter> SmallLetters
        {
            get { return smallLetters; }
            set { smallLetters = value; }
        }

        public int CorrectTrials
        {
            get { return correctTrials; }
            set { correctTrials = value; }
        }

        public int Fails
        {
            get { return fails; }
            set { fails = value; }
        }

        public TimeSpan Time
        {
            get { return time; }
            set { time = value; }
        }

        public int LettersLeft
        {
            get { return lettersLeft; }
            set { lettersLeft = value; }
        }

        public Thread SaveResultsThread
        {
            get { return saveResultsThread; }
            private set { saveResultsThread = value; }
        }

        public void CalculateTime(DateTime endTime)
        {
            this.Time = endTime - this.startTime;
        }

        public void SaveResults()
        {
            this.SaveResultsThread = new Thread(SaveToDatabase);
            this.SaveResultsThread.Start();
        }
        #endregion

        private void SaveToDatabase()
        {
            LettersGameManager manager = new LettersGameManager(this.config.Player);
            LettersGameParams gameParams = new LettersGameParams
            {
                Level = this.config.CurrentLevel,
                LettersCount = this.config.LettersCount,
                CorrectTrials = this.correctTrials,
                Failures = this.Fails,
                Time = (int)this.Time.TotalMilliseconds
            };
            manager.SaveGameResult(gameParams);
        }
    }
}
