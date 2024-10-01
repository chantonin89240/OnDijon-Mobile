using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Simulator.Entities.Dto
{
    public class CityContextListDto : WsDMDto
    {
        public string Title;
        public string WarningMessage;
        public List<CityContextDto> Cities;
    }
}