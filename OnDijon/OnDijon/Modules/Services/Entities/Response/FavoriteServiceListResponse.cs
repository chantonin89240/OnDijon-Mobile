using OnDijon.Modules.Services.Entities.Dto;
using OnDijon.Common.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Services.Entities.Response
{
    public class FavoriteServiceListResponse : Common.Entities.Response.Response
    {
        public IList<ServiceDto> Services { get; set; }
        public List<CheckboxModel> Scopes { get; set; }
        public bool HasAlertIdentity { get; set; }
    }
}
