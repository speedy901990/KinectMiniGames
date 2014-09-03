using System.Collections.Generic;
using System.Runtime.Serialization;
using TrainOfWords.Model;

namespace TrainOfWords.Resources
{
    [DataContract]
    public class WordsContainer
    {
        [DataMember]
        public List<Word> threeChars { get; set; }

        [DataMember]
        public List<Word> fourChars { get; set; }

        [DataMember]
        public List<Word> fiveChars { get; set; }
    }
}
