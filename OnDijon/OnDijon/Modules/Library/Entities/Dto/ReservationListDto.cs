using Newtonsoft.Json;
using System.Collections.Generic;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class ReservationListDto : WsDMDto
    {
        [JsonProperty("Reservations")]
        public List<ReservationDto> Reservations { get; set; }
    }
}
