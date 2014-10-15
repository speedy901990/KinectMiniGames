using DatabaseManagement;

namespace TrainOfWords.Model
{
    public class TrainOfWordsGameConfig
    {
        public TrainOfWordsGameConfig()
        {
            WindowWidth = 1280;
            WindowHeight = 1024;
            LetterWidth = 250;
        }
        public int WindowWidth { get; set; }

        public int WindowHeight { get; set; }

        public int LetterWidth { get; set; }

        public int LetterHeight
        {
            get
            {
                var a = 4.0 / 9.0;
                var b = WindowHeight - 100;
                var result = a*b/3;
                return (int) result;
            }
        }

        public Player Player { get; set; }

        public int NuberOfLettersOnScreen
        {
            get { return 2 * WindowWidth / LetterWidth; }
        }

        public int Level { get; set; }

        public int AllLettersCount { get; set; }
    }
}
