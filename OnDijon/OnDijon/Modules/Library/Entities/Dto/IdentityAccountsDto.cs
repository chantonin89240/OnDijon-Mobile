using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class IdentityAccountsDto : WsDMDto
    {
        [JsonProperty("Accounts")]
        public List<UserIdentityInformationDto> Accounts { get; set; }
    }
}
