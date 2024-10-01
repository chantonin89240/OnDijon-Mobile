using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnDijon.Modules.School.Entities.Request
{
    public class SessionRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
    }
}
