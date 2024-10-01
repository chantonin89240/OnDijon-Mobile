using System.Collections.Generic;

namespace OnDijon.Modules.Report.Entities.Response
{
    public class AddressesListResponse : Common.Entities.Response.Response
    {
        public IList<string> Suggestions { get; set; }
    }
}
