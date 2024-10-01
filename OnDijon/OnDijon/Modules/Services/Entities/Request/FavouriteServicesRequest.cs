using Newtonsoft.Json;
using OnDijon.Modules.Account.Entities.Request;

namespace OnDijon.Modules.Services.Entities.Request
{
    public class FavouriteServicesRequest : AccountRequest
    {
        [JsonProperty(PropertyName = "UserEditId")]
        public string UserEditId { get; set; }


    }
}
