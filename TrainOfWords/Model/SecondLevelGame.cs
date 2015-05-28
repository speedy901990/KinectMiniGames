using System;
using System.Collections.Generic;
using System.Drawing;
using TrainOfWords.Resources;

namespace TrainOfWords.Model
{
    public sealed class SecondLevelGame : Game
    {
        public SecondLevelGame(TrainOfWordsGameConfig config) : base(config)
        {
            var random = new Random();

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
            
            PrepareLettersAndImages();
        }

        protected override void PrepareLettersAndImages()
        {
            base.PrepareLettersAndImages();
            foreach (var word in Words)
            {
                word.ShowTrain = true;
            }
        }

        public override void Run()
        {
            StartTime = DateTime.Now;
        }
    }
}