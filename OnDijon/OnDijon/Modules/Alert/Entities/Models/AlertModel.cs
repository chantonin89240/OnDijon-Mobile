using Newtonsoft.Json;
using OnDijon.Common.Utils.Converters;
using System;

namespace OnDijon.Modules.Alert.Entities.Models
{
    public class AlertModel
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool? IsRead { get; set; }
        [JsonConverter(typeof(DateTimeJsonConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime? SendingDate { get; set; }
        public string EditId { get; set; }
        public string Scope { get; set; }
        public string ExternalLink { get; set; }
        public string NavigationLink { get; set; }
    }
}
