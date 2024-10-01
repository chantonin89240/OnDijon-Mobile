using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.School.Entities.Dto
{
    public class ChildDietDto : WsDMDto
    {
        public bool HasOption { get; set; }
        public bool OptionDiet { get; set; }
        public List<string> PossibleStandardDiets { get; set; }
        public string StandardDiet { get; set; }
        public string CityContext { get; set; }
    }
}
