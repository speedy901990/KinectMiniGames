using DatabaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameManagement.ViewModel
{
    public class GameResultViewModel
    {
        public string Name { get; set; }
        public IList<HistoryResultViewModel> historyResultViewModelList;
        public GameResultViewModel(GameResult gameResult)
        {
            Name = gameResult.Name;
            historyResultViewModelList = gameResult.HistoryResults.ToList().ConvertAll(new Converter<HistoryResult, HistoryResultViewModel>(x =>
            {
                return new HistoryResultViewModel()
                {

                    Value = x.Value


                };
            }));
                
        }
    }
}
