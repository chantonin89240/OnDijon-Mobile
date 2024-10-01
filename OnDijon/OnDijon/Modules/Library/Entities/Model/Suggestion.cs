using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnDijon.Modules.Library.Entities.Model
{
    public class Suggestion
    {
        [JsonProperty("Suggestions")]
        public List<string> Suggestions { get; set; }
    }
}
