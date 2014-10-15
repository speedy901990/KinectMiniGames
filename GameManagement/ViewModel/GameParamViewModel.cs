using DatabaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameManagement.ViewModel
{
    public class GameParamViewModel
    {
        public string Name { get; set; }
        public IList<HistoryParamViewModel> historyResultViewModelList;
        public GameParamViewModel(GameParam gameParam)
        {
            Name = gameParam.Name;
            historyResultViewModelList = gameParam.HistoryParams.ToList().ConvertAll(new Converter<HistoryParam, HistoryParamViewModel>(x =>
            {
                return new HistoryParamViewModel()
                {
                    Value = x.Value


                };
            }));
                
        }
    }
}
