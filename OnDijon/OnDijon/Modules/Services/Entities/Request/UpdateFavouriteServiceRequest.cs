using System.Collections.Generic;
using Newtonsoft.Json;
using OnDijon.Modules.Services.Entities.Dto;
using OnDijon.Modules.Account.Entities.Request;

namespace OnDijon.Modules.Services.Entities.Request
{
    public class UpdateFavouriteServiceRequest : AccountRequest
    {
        [JsonProperty("UserEditId")]
        public string UserEditId { get; set; }

        [JsonProperty("MenuItems")]
        public IEnumerable<ServiceDto> NewFavouriteServices { get; set; }

        [JsonProperty("ScopeFavorites")]
        public IEnumerable<ScopeRequest> NewScopeFavorites { get; set; }
    }

    public class ScopeRequest
    {
        public string Title;
        public bool Favorite;
    }

}
