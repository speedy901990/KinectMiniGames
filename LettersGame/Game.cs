using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Threading;
using DatabaseManagement.Managers;
using DatabaseManagement.Params;

namespace LettersGame
{
    public class Game
    {
        #region private state
        private readonly LettersGameConfig _config;
        private readonly DateTime _startTime;

        #endregion

        #region constructor
        public Game(LettersGameConfig config)
        {
            _config = config;
            int numOfLetters = config.LettersCount;
            if (config.CurrentLevel == 1)
            {
                using (ResourceSet resourceSet = Resources.Letters.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                {
                    var allLetters = new List<Letter>();
                    foreach (DictionaryEntry item in resourceSet)
                    {
                        allLetters.Add(new Letter((string)item.Value));
                    }
                    var rand = new Random();
                    SmallLetters = new List<Letter>();
                    BigLetters = new List<Letter>();
                    for (var i = 0; i < numOfLetters; i++)
                    {
                        var index = rand.Next(allLetters.Count);
                        var letter = allLetters[index];
                        SmallLetters.Add(letter);
                        BigLetters.Add(letter);
                        allLetters.RemoveAt(index);
                    }
                    Resources.Letters.ResourceManager.ReleaseAllResources();
                    LettersLeft = numOfLetters;
                }
            }
            if (config.CurrentLevel == 2)
            {
                using (ResourceSet resourceSet = Resources.Letters.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                {
                    var allLetters = new List<Letter>();
                    foreach (DictionaryEntry item in resourceSet)
                    {
                        allLetters.Add(new Letter((string)item.Value));
                    }
                    var rand = new Random();
                    SmallLetters = new List<Letter>();
                    Trolleys = new List<Letter>();

                    for (var i = 0; i < config.TrolleysCount; i++)
                    {
                        var index = rand.Next(allLetters.Count);
                        var letter = allLetters[index];
                        SmallLetters.Add(letter);
                        Trolleys.Add(letter);
                        allLetters.Remove(letter);
                    }
                    numOfLetters -= config.TrolleysCount;

                    for (var i = 0; i < numOfLetters; i++)
                    {
                        var letter = allLetters[rand.Next(allLetters.Count)];
                        SmallLetters.Add(letter);
                        allLetters.Remove(letter);
                    }
                    SmallLetters.Sort();
                    Resources.Letters.ResourceManager.ReleaseAllResources();
                    LettersLeft = config.TrolleysCount;
                }
            }
            if (config.CurrentLevel == 3)
            {
                using (ResourceSet resourceSet = Resources.LettersAndNames.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                {
                    var allLetters = new List<Letter>();
                    foreach (DictionaryEntry item in resourceSet)
                    {
                        allLetters.Add(new Letter((string)item.Key, (string)item.Value));
                    }
                    var rand = new Random();
                    SmallLetters = new List<Letter>();
                    BigLetters = new List<Letter>();
                    for (var i = 0; i < numOfLetters; i++)
                    {
                        var index = rand.Next(allLetters.Count);
                        var letter = allLetters[index];
                        SmallLetters.Add(letter);
                        BigLetters.Add(letter);
                        allLetters.RemoveAt(index);
                    }
                    Resources.LettersAndNames.ResourceManager.ReleaseAllResources();
                    LettersLeft = numOfLetters;
                }
            }
            CorrectTrials = 0;
            Fails = 0;
            _startTime = DateTime.Now;
        }
        #endregion

        #region public state

        internal List<Letter> BigLetters { get; set; }

        internal List<Letter> SmallLetters { get; set; }

        internal List<Letter> Trolleys { get; set; }

        public int CorrectTrials { get; set; }

        public int Fails { get; set; }

        public TimeSpan Time { get; set; }

        public int LettersLeft { get; set; }

        public Thread SaveResultsThread { get; private set; }

        public void CalculateTime(DateTime endTime)
        {
            Time = endTime - _startTime;
        }

        #endregion

        #region saving results
        public void SaveResults()
        {
            SaveResultsThread = new Thread(SaveToDatabase);
            SaveResultsThread.Start();
        }

        private void SaveToDatabase()
        {
            var manager = new LettersGameManager(_config.Player);
            var gameParams = new LettersGameParams
            {
                Level = _config.CurrentLevel,
                LettersCount = _config.LettersCount,
                CorrectTrials = CorrectTrials,
                Failures = Fails,
                Time = (int)Time.TotalMilliseconds
            };
            manager.SaveGameResult(gameParams);
        }
        #endregion
    }
}