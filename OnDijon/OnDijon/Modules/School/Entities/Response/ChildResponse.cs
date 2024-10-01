using OnDijon.Modules.School.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.School.Entities.Response
{
    public class ChildResponse : Common.Entities.Response.Response
    {
        public IList<ChildCardModel> SchoolCardList { get; set; }
        public string SessionScheduledHelper { get; set; }
    }
}
