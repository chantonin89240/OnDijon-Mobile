using OnDijon.Common.Entities.Dto;
using System;
using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Dto
{
    public class WedAlshChildDto : WsDMDto
    {
        public string PersonNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Civility { get; set; }
        public DateTime? BirthDate { get; set; }
        public WedAlshSchoolDto School { get; set; }
        public string SchoolLevel { get; set; }
        public string Meal { get; set; }
        public bool? Handicap { get; set; }
        public bool? EducatedInPrivateSchool { get; set; }
        public List<WedAlshRegistrationDetailsDto> Registrations { get; set; }
    }
}
