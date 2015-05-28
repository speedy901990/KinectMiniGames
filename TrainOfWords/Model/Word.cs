using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace TrainOfWords.Model
{
    public class Word
    {
        public string Name { get; private set; }

        public Bitmap Bitmap { get; set; }

        public bool ShowTrain { get; set; }

        public List<char> Letters { get; private set; }


        public Word(string name)
        {
            Name = name;
            Letters = Name.ToCharArray().ToList();
            //Letters = Name.ToList().ConvertAll(input => input.ToString(CultureInfo.InvariantCulture));
        }
    }
}
