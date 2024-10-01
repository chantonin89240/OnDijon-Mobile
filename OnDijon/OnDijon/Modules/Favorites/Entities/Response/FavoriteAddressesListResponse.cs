using OnDijon.Common.Entities.Model;
using OnDijon.Modules.Favorites.Entities.Dto;
using OnDijon.Modules.Favorites.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Favorites.Entities.Response
{
    public class FavoriteAddressesListResponse : Common.Entities.Response.Response
    {
        public List<FavoriteModel> FavAdressesList { get; set; }

    }
}
