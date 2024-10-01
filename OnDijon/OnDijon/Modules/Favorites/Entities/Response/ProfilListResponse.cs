using OnDijon.Modules.Favorites.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Favorites.Entities.Response
{
    public class ProfilListResponse : Common.Entities.Response.Response
    {
        public List<ProfilModel> ProfilList { get; set; }

    }
}
