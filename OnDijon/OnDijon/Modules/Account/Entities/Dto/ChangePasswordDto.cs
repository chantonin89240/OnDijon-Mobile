using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.Account.Entities.Dto
{
    public class ChangePasswordDto : WsDMDto
    {
        [JsonProperty(PropertyName = "ProfileEditId")]
        public string Guid { get; set; }

        [JsonProperty(PropertyName = "AuthenticationUrl")]
        public string AuthenticationUrl { get; set; }
    }
}
