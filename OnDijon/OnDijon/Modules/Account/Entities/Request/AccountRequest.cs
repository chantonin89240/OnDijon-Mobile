using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;

namespace OnDijon.Modules.Account.Entities.Request
{
    public class AccountRequest : DtoRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "UserEditId")]
        public string UserEditId { get; set; }
    }
}
