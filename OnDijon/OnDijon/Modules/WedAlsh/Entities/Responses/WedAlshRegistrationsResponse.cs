using OnDijon.Modules.WedAlsh.Entities.Models;
using System.Collections.Generic;

namespace OnDijon.Modules.WedAlsh.Entities.Response
{
    public class WedAlshRegistrationsResponse : Common.Entities.Response.Response
    {
        public List<WedAlshChildModel> Childs { get; set; }
    }
}
