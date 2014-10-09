using DatabaseManagement;

namespace TrainOfWords.View
{
    public class TrainOfWordsGameConfig
    {
        public const int LetterWidth = 250;

        public Player Player { get; set; }

        public int NuberOfLettersOnScreen
        {
            get { return 2 * WindowWidth/LetterWidth; }
        }

        public int WindowWidth { get; set; }

        public int Level { get; set; }

        public int AllLettersCount { get; set; }
    }
}
