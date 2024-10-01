using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Strike.Entities.Model
{
    public class SessionStrikeModel
    {
        public string EditId { get; set; }
        public DateTime DateStrike { get; set; }
        public List<SchoolStrikeInfoModel> Strikes { get; set; }
    }
}
