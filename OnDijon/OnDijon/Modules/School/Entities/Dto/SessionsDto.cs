using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.School.Entities.Dto
{
    public class SessionsDto : WsDMDto
    {
        public IList<Dictionary> SessionEditIdByCityContext { get; set; }
    }


    public class Dictionary
    {
        public string Key;
        public string Value;
    }
}
