using DatabaseManagement;
using GameManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Data.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.Collections.Specialized;
using System.ComponentModel;



namespace GameManagement
{
    /// <summary>
    /// Interaction logic for ShowDetailsWindow.xaml
    /// </summary>
    public partial class ShowDetailsWindow : Window
    {
        public ObservableCollection<HistoryViewModel> historyList = new ObservableCollection<HistoryViewModel>();
        public ObservableCollection<GameHistoryResultViewModel> historyResultList = new ObservableCollection<GameHistoryResultViewModel>();
        public ObservableCollection<GameHistroyParamViewModel> historyParamList = new ObservableCollection<GameHistroyParamViewModel>();


    //    private ObservableCollection<GameHistoryResultViewModel> _historyResultList;

    //    public ObservableCollection<GameHistoryResultViewModel> HistoryResultList
    //    {
    //        get{ return _historyResultList;}
    //        set { _historyResultList = value; OnPropertyChanged("HistoryResultList"); }
    //    }

        
    //        public event PropertyChangedEventHandler PropertyChanged;
    //protected virtual void OnPropertyChanged(string propertyName)
    //{
    //    PropertyChangedEventHandler handler = PropertyChanged;
    //    if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
   // }
         


        public ShowDetailsWindow()
        {
            GetHistoryList();
            
            InitializeComponent();

        }
        public void GetHistoryList(){

            using (var context = new GameModelContainer())
            {

                historyList = new ObservableCollection<HistoryViewModel>(context.Histories.ToList().ConvertAll(new Converter<History, HistoryViewModel>(x =>
            {
                return new HistoryViewModel()
                {
                    Date = x.Date,
                    GameName = x.Game.Name,
                    Id = x.Id,
                    PlayerName = x.Player.Name

                };
            })));
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowPlayerDetailsButton_Click(object sender, RoutedEventArgs e)
        {   
           
            int index = -1;
            if (HistoryGrid.SelectedCells.Count != 0)
            {
                try
                {
                    var cellInfo = HistoryGrid.SelectedCells[0];

                    var content = cellInfo.Column.GetCellContent(cellInfo.Item);
                    HistoryViewModel row = (HistoryViewModel)content.DataContext;
                    index = row.Id;
                }
                catch (Exception ex)
                {
                    //zaznaczono pusty rekord
                }
                
            }
            if (index>-1)
            {
                using (var context = new GameModelContainer())
                {
                    
                    historyResultList = new ObservableCollection<GameHistoryResultViewModel>(context.HistoryResults.Where(x => x.History.Id == index).ToList().ConvertAll(new Converter<HistoryResult, GameHistoryResultViewModel>(x =>
                    {
                        return new GameHistoryResultViewModel()
                        {
                            Value = x.Value,
                            Name = x.GameResult.Name

                        };
                    })));

                    historyParamList = new ObservableCollection<GameHistroyParamViewModel>(context.HistoryParams1.Where(x => x.History.Id == index).ToList().ConvertAll(new Converter<HistoryParam, GameHistroyParamViewModel>(x =>
                    {
                        return new GameHistroyParamViewModel()
                        {
                            Value = x.Value,
                            Name = x.GameParam.Name
                            

                        };
                    })));

                }

                HistoryResultGrid.DataContext = historyParamList;
                GameResultGrid.DataContext = historyResultList;
            }

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //historyList.CollectionChanged +=ObservableCollection_CollectionChanged;
            HistoryGrid.DataContext = historyList;
            //historyResultList.CollectionChanged += ObservableCollection_CollectionChanged;
            HistoryResultGrid.DataContext = historyResultList;
            //historyList.CollectionChanged += ObservableCollection_CollectionChanged;
            GameResultGrid.DataContext = historyResultList;
            
        }

        private void GameResultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new GameModelContainer())
            {

                historyResultList = new ObservableCollection<GameHistoryResultViewModel>(context.HistoryResults.Where(x => x.Id == Convert.ToInt32(((DataRowView)HistoryGrid.SelectedItems[0])["Id"].ToString())).ToList().ConvertAll(new Converter<HistoryResult, GameHistoryResultViewModel>(x =>
                {
                    return new GameHistoryResultViewModel()
                    {
                       Value = x.Value,
                       Name = x.GameResult.Name

                    };
                })));
            }
            

        }

        private void HistoryGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new GameModelContainer())
            {
                object item = HistoryGrid.SelectedItem;
                int index = Convert.ToInt32((HistoryGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);// Convert.ToInt32(((DataRowView)HistoryGrid.SelectedItems[0])["Id"].ToString());
                historyResultList = new ObservableCollection<GameHistoryResultViewModel>(context.HistoryResults.Where(x => x.Id == index).ToList().ConvertAll(new Converter<HistoryResult, GameHistoryResultViewModel>(x =>
                {
                    return new GameHistoryResultViewModel()
                    {
                        Value = x.Value,
                        Name = x.GameResult.Name

                    };
                })));
            }
            

        }
        //private void ObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //    {
        //        foreach (var item in e.NewItems)
        //        {
        //            item.PropertyChanged += this.Item_PropertyChanged;
        //        }
        //    }

        //    if (e.OldItems != null)
        //    {
        //        foreach (var item in e.OldItems)
        //        {
        //            item.PropertyChanged -= this.Item_PropertyChanged;
        //        }
        //    }
        //}
        //private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    
        //}

       
    }
}
