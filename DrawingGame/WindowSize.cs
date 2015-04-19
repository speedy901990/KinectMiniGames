using System.ComponentModel;

namespace DrawingGame
{
    public class WindowSize
    {
        public event PropertyChangedEventHandler PropertyChanged;
        double width { get; set; }
        double height { get; set; }

        public double Width
        {
            get { return width; }
            set
            {
                width = value;

                OnPropertyChanged("Width");
            }
        }
        public double Height
        {
            get { return height; }
            set
            {
                height = value;

                OnPropertyChanged("Height");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
