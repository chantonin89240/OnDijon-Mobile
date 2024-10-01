using System.Collections.Generic;

namespace OnDijon.Modules.Diary.Entities.Response
{
    public class DiarySuggestionResponse : Common.Entities.Response.Response
    {
        public IEnumerable<string> Results { get; set; }
    }
}
