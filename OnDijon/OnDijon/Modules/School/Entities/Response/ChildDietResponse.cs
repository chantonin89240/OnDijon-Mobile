using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.School.Entities.Response
{
    public class ChildDietResponse : Common.Entities.Response.Response
    {
        public bool HasOption { get; set; }
        public bool OptionDiet { get; set; }
        public List<string> PossibleStandardDiets { get; set; }
        public string StandardDiet { get; set; }
    }
}
