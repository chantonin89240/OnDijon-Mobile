using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace OnDijon.Modules.WedAlsh.Entities.Models
{
    public class WedAlshChildModel
    {
        public string PersonNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Civility { get; set; }
        public DateTime? BirthDate { get; set; }
        public WedAlshSchoolModel School { get; set; }
        public string SchoolLevel { get; set; }
        public string Meal { get; set; }
        public bool? Handicap { get; set; }
        public bool? EducatedInPrivateSchool { get; set; }
        public List<WedAshRegistrationDetailsModel> Registrations { get; set; }

        //visuel
        public string Color { get; set; }
        public ImageSource ImageSource { get; set; }
        public string Title { get { return "Les mercredis d" + ("aeiouAEIOU".IndexOf(FirstName[0]) >= 0 ? "'" : "e ")  + FirstName.Substring(0,1).ToUpper() + FirstName.Substring(1).ToLower(); } set { } }
    }
}
