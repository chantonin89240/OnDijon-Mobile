using OnDijon.Modules.Library.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Response
{
    public class HoldingResponse : Common.Entities.Response.Response
    {
        public List<Holding> Holdings { get; set; }
    }
}
