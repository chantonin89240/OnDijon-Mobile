using OnDijon.Modules.Abris.Entities.Dto;
using OnDijon.Modules.Abris.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Abris.Entities.Response
{
    public class ShelterStatesListResponse : Common.Entities.Response.Response
    {
        public List<ShelterStateDto> SheltersList { get; set; }
    }
}
