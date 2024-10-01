using System;
using System.Globalization;

namespace OnDijon.Modules.School.Models
{
    public class SchoolRestaurantCalendar
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public DateTime Date { get; set; }
        public bool Ferie { get; set; }
        public bool Ouvert { get { return !Ferie; } }
        public Plat Entree { get; set; }
        public Plat Proteine { get; set; }
        public Plat Legume { get; set; }
        public Plat Fromage { get; set; }
        public Plat Dessert { get; set; }
        public string Day { get { return Date.ToString("dddd", CultureInfo.CreateSpecificCulture("fr-FR")); } }
    }

    public class Plat
    {
        public string Nom { get; set; }
        public bool Bio { get; set; }
        public bool Porc { get; set; }
        public bool ProduitLocaux { get; set; }
        public bool CommerceEquitable { get; set; }
    }

    public class Week
    {
        public DateTime BeginningDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Title { get { return $"Semaine du " + BeginningDate.ToString("dd/MM") + " au " + EndingDate.ToString("dd/MM"); } }
    }
}
