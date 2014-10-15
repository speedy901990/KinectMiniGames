using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.ViewModel
{
    public class GameViewModel
    {
        public string Name { get; set; }
        public IList<GameParamViewModel> GameParams { get; set; }
        public IList<GameResultViewModel> GameResults { get; set; }
        public IList<HistoryViewModel> Histories { get; set; }
    }
}
