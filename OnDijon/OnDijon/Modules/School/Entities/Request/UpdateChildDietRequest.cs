using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.School.Entities.Request
{
    public class UpdateChildDietRequest
    {
        [JsonProperty("key")]
        public string Key;

        [JsonProperty("childEditId")]
        public string ChildEditId;

        [JsonProperty("diet")]
        public string StandardDiet;

        [JsonProperty("option")]
        public bool Option;

        [JsonProperty("CityContext")]
        public string CityContext;
    }
}
