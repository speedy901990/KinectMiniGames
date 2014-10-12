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

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HistoryGrid.DataContext = historyList;
            HistoryResultGrid.DataContext = historyResultList;
            HistoryGrid.DataContext = historyList;
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
    }
}
