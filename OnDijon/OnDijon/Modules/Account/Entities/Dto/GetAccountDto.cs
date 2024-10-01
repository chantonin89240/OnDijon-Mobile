using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Account.Entities.Dto
{
    public class GetAccountDto : WsDMDto
    {
        [JsonProperty(PropertyName = "Profile")]
        public ProfileDto Profile { get; set; }

        [JsonProperty(PropertyName = "MobileRegistration")]
        public IList<MobileRegistrationDto> MobileRegistrations { get; set; }
    }
}
