using OnDijon.Modules.Abris.Entities.Models;
using OnDijon.Modules.Favorites.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Abris.Entities.Response
{
    public class AbrisListResponse : Common.Entities.Response.Response
    {
        public List<AbrisModel> AbrisList { get; set; }
    }
}
