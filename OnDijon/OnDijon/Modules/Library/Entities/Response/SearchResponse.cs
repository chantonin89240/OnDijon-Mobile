using OnDijon.Modules.Library.Entities.Model;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Response
{
    public class SearchResponse : Common.Entities.Response.Response
    {
        public List<Resource> Resources { get; set; }
        public int Page { get; set; }
        public int PageMax { get; set; }
    }
}
