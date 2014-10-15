using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DatabaseManagement.Managers;
using DatabaseManagement.Params;

namespace LettersGame
{
    public class Game
    {
        private readonly LettersGameConfig _config;
        private readonly DateTime _startTime;

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
                var allLetters = new List<Letter>();
                var imagesBoys = new List<ImageBrush>
                {
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesBoys.ch1)),
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesBoys.ch2)),
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesBoys.ch3)),
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesBoys.ch4))
                };
                var imagesGirls = new List<ImageBrush>
                {
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesGirls.dz1)),
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesGirls.dz2)),
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesGirls.dz3)),
                    new ImageBrush(ConvertBitmapToBitmapSource(Resources.ImagesGirls.dz4))
                };
                using (ResourceSet resourceSet = Resources.LettersAndNamesBoys.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                {
                    var i = 0;
                    foreach (DictionaryEntry item in resourceSet)
                    {
                        allLetters.Add(new Letter((string) item.Key, (string) item.Value, imagesBoys[i%4]));
                        i++;
                    }
                    Resources.LettersAndNamesBoys.ResourceManager.ReleaseAllResources();
                }
                using (ResourceSet resourceSet = Resources.LettersAndNamesGirls.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
                {
                    var i = 0;
                    foreach (DictionaryEntry item in resourceSet)
                    {
                        allLetters.Add(new Letter((string) item.Key, (string) item.Value, imagesGirls[i%4]));
                        i++;
                    }
                    Resources.LettersAndNamesGirls.ResourceManager.ReleaseAllResources();
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
                LettersLeft = numOfLetters;
            }
            CorrectTrials = 0;
            Fails = 0;
            _startTime = DateTime.Now;
        }

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bm)
        {
            var bitmap = bm;
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmap.Dispose();
            return bitmapSource;
        }

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
    }
}