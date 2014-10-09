using System;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Media;

namespace LettersGame
{
    public class Letter : IComparable
    {
        protected bool Equals(Letter other)
        {
            return string.Equals(SmallLetter, other.SmallLetter);
        }

        public override int GetHashCode()
        {
            return (SmallLetter != null ? SmallLetter.GetHashCode() : 0);
        }

        public int CompareTo(object obj)
        {
            var other = obj as Letter;
            if (other != null) return String.Compare(other.SmallLetter, SmallLetter, StringComparison.Ordinal);
            return obj.GetHashCode() - GetHashCode();
        }

        public string SmallLetter { get; set; }

        public string BigLetter { get; set; }

        public string Word { get; set; }

        public ImageBrush Image { get; set; }

        public Letter(string letter)
        {
            SmallLetter = letter.ToLower();
            BigLetter = SmallLetter.ToUpper();
        }

        public Letter(string letter, string word, ImageBrush image = null)
        {
            SmallLetter = letter.ToLower();
            BigLetter = letter.ToUpper();
            Word = word;
            Image = image;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Letter) obj);
        }
    }
}