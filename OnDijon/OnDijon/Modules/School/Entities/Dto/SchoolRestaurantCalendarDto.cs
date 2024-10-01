using System;
using OnDijon.Common.Entities.Dto;

namespace OnDijon.Modules.School.Entities.Dto
{
    public class SchoolRestaurantCalendarDto : WsDMDto
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public DateTime Date { get; set; }
        public bool Ferie { get; set; }
        public Plat Entree { get; set; }
        public Plat Proteine { get; set; }
        public Plat Legume { get; set; }
        public Plat Fromage { get; set; }
        public Plat Dessert { get; set; }
    }

    public class Plat
    {
        public string Nom { get; set; }
        public bool Bio { get; set; }
        public bool Porc { get; set; }
        public bool ProduitLocaux { get; set; }
        public bool CommerceEquitable { get; set; }
    }

}
