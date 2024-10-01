using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace OnDijon.Modules.Favorites.Entities.Models
{
    public class FavoriteModel
    {
        public int? Id { get; set; }
        public int ProfilId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CodePostal { get; set; }
        public string Ville { get; set; }
        public string Rue { get; set; }
        public string Pays { get; set; }
    }
}
