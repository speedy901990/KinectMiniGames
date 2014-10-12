using DatabaseManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.ViewModel
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private string _name;
            private string _surname;
        private int _age;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                   
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    
                    OnPropertyChanged("Surname");
                }
            }
        }
        public int Age
        {
            get { return _age; }
            set
            {
                if (_age != value)
                {
                    _age = value;
                    
                    OnPropertyChanged("Age");
                }
            }
        }

        //public ICollection<HistoryViewModel> Histories;

        //public PlayerViewModel(Player player)
        //{
        //    ICollection<HistoryViewModel> historyViewModelCollection;

        //    HistoryViewModel historiesViewModel = new HistoryViewModel();
        //    Name = player.Name;
        //    Surname = player.Surname;
        //    Age = player.Age;
        //    //Histories = player.Histories.ToList().ConvertAll(new Converter<History,HistoryViewModel(x => {return new HistoryViewModel(){
        //    //date = x.Date,
        //    //game = new GameViewModel(){
        //    //    Name = x.Game.Name,
                
            
        //    //},
        //    //player = x.Player,
            
        //    //};}
        //    //));
        //}



        void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
