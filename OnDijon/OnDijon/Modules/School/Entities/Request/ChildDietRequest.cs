using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.School.Entities.Request
{
    public class ChildDietRequest
    {
        [JsonProperty("Key")]
        public string Key;

        [JsonProperty("ChildEditId")]
        public string ChildEditId;

        [JsonProperty("CityContext")]
        public string CityContext;
    }
}
