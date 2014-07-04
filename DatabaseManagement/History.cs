//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseManagement
{
    using System;
    using System.Collections.Generic;
    
    public partial class History
    {
        public History()
        {
            this.HistoryParams = new HashSet<HistoryParam>();
            this.HistoryResults = new HashSet<HistoryResult>();
        }
    
        public long id { get; set; }
        public long id_game { get; set; }
        public long id_player { get; set; }
        public System.DateTime date { get; set; }
    
        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }
        public virtual ICollection<HistoryParam> HistoryParams { get; set; }
        public virtual ICollection<HistoryResult> HistoryResults { get; set; }
    }
}