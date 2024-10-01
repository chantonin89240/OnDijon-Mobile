using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Response
{
    public class SuggestionResponse : Common.Entities.Response.Response
    {
        public List<string> Suggestions { get; set; }
    }
}