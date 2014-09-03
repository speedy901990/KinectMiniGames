using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TrainOfWords.View;

namespace TrainOfWords.Model
{
    public class FirstLevelGame : Game
    {
        public FirstLevelGame(TrainOfWordsGameConfig config) : base(config)
        {
            var random = new Random();
            var alphabet = new List<string>();
            using (var resourceSet = Resources.Alphabet.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
            {
                var letters = resourceSet.Cast<DictionaryEntry>().Select(entry => entry.Value.ToString());
                alphabet.AddRange(letters);
                Resources.Alphabet.ResourceManager.ReleaseAllResources();
            }
            using (var resourceSet = Resources.Words3Chars.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
            {
                var words = resourceSet.Cast<DictionaryEntry>().ToList();
                var number = random.Next(words.Count);
                var word = words[number].Value.ToString();
                Words.Add(new Word(word));

                Resources.Words3Chars.ResourceManager.ReleaseAllResources();
            }
            using (var resourceSet = Resources.Words4Chars.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
            {
                var words = resourceSet.Cast<DictionaryEntry>().ToList();
                var number = random.Next(words.Count);
                var word = words[number].Value.ToString();
                Words.Add(new Word(word));

                Resources.Words4Chars.ResourceManager.ReleaseAllResources();
            }
            using (var resourceSet = Resources.Words5Chars.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true))
            {
                var words = resourceSet.Cast<DictionaryEntry>().ToList();
                var number = random.Next(words.Count);
                var word = words[number].Value.ToString();
                Words.Add(new Word(word));

                Resources.Words5Chars.ResourceManager.ReleaseAllResources();
            }
            foreach (var word in Words)
            {
                Letters.Add(word.Name, new List<string>(word.Letters));
                while (Letters[word.Name].Count < Config.NuberOfLetters)
                {
                    var index = random.Next(alphabet.Count);
                    Letters[word.Name].Add(alphabet[index]);
                }
            }
            foreach (var letter in Letters)
                letter.Value.Sort();
        }

        public override void Run()
        {
            StartTime = DateTime.Now;
        }
    }
}
