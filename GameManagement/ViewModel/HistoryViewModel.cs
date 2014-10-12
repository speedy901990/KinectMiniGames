using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.ViewModel
{
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public string   GameName { get; set; }
        public DateTime Date { get; set; }

        //public ICollection<HistoryResult> HistoryResults { get; set; }
        //public ICollection<HistoryParam> HistoryParams { get; set; }
    }
}
