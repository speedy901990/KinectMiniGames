using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TrainOfWords.Model
{
    public class Word
    {
        public string Name { get; private set; }

        public List<string> Letters { get; private set; }

        public Word(string name)
        {
            Name = name;
            Letters = Name.ToList().ConvertAll(input => input.ToString(CultureInfo.InvariantCulture));
        }
    }
}
