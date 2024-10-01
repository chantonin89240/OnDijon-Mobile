using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.Favorites.Entities.Models
{
    public class ShelterUserModel
    {
        public int Id { get; set; }
        public int IdProfil { get; set; }
        public Boolean HasFamily { get; set; }
        public string Deplacement { get; set; }

    }
}
