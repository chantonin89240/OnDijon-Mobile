namespace OnDijon.Modules.School.Entities.Models
{
    

    public class ChildCardModel
    {

    
        public SchoolCardType Type { get; set; }
        public ChildByCityModel ChildModel { get; set; }

        public string Title { get { return "La journée de " + ChildModel.Prenom; } set { } }

        public string Color { get; set; }

      
    }

    public enum SchoolCardType
    {
        Child,
        Restaurant
       
    }
}
