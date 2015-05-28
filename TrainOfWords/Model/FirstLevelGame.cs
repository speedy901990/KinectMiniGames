using System;
using System.Collections.Generic;
using System.Drawing;
using TrainOfWords.Resources;

namespace TrainOfWords.Model
{
    /// <summary>
    /// Prepares data from resorces: 
    /// </summary>
    public class FirstLevelGame : Game
    {
        public FirstLevelGame(TrainOfWordsGameConfig config) : base(config)
        {
            var random = new Random();
            Config.AllLettersCount = 0;
            
            //3 chars word
            var number = random.Next(WordsContainer.Words3Chars.Count);
            var wordStr = WordsContainer.Words3Chars[number];
            Words.Add(new Word(wordStr));

            //4 chars word
            number = random.Next(WordsContainer.Words4Chars.Count);
            wordStr = WordsContainer.Words4Chars[number];
            Words.Add(new Word(wordStr));

            //5 chars word
            number = random.Next(WordsContainer.Words5Chars.Count);
            wordStr = WordsContainer.Words5Chars[number];
            Words.Add(new Word(wordStr));

            foreach (var word in Words)
            {
                Letters.Add(word.Name, new List<string>(word.Letters));
                Config.AllLettersCount += word.Letters.Count;
                while (Letters[word.Name].Count < Config.NuberOfLettersOnScreen)
                {
                    var index = random.Next(WordsContainer.Alphabet.Count);
                    Letters[word.Name].Add(WordsContainer.Alphabet[index]);
                }
                var rm = Properties.Resources.ResourceManager;
                var image = (Bitmap)rm.GetObject(word.Name);
                word.Bitmap = image;
            }
            foreach (var letter in Letters)
                letter.Value.Sort();

            StartTime = DateTime.Now;
        }

        public override void Run()
        {
            StartTime = DateTime.Now;
        }
    }
}
