using System;
using System.Collections.Generic;

namespace OnDijon.Modules.Strike.Entities.Model
{
    public class NurserySessionStrikeModel
    {
        public string EditId { get; set; }
        public DateTime DateStrike { get; set; }
        public List<NurseryStrikeInfoModel> Strikes { get; set; }
    }
}
