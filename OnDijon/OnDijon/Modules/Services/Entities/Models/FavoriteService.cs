using OnDijon.Modules.Services.Entities.Dto;
using System.Collections.Generic;

namespace OnDijon.Modules.Services.Entities.Models
{
    public class FavoriteService
    {
        public List<ServiceLayout> Services { get; set; }
        public List<Scope> Scopes { get; set; }
        public bool HasAlertIdentity { get; set; }
    }
}
