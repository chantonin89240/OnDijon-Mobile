using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using Xamarin.Forms;

namespace OnDijon.Modules.Report.Entities.Dto
{
    public class ReportDto : Common.Entities.Dto.Dto
    {
        private static readonly Dictionary<string, Color> StatusColors = new Dictionary<string, Color>()
        {
            { "Créé", (Color)App.Current.Resources["BadgeColorRed"] },
            { "En cours de traitement", (Color)App.Current.Resources["BadgeColorYellow"] },
            { "Clôture",(Color)App.Current.Resources["BadgeColorGreen"] },
            { "Annulation", (Color)App.Current.Resources["BadgeColorLightGray"] }
        };

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "typeCode")]
        public string TypeCode { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "position")]
        public ReportPositionDto Position { get; set; }

        [JsonProperty(PropertyName = "photoUrl")]
        public string PhotoUrl { get; set; }

        [JsonProperty(PropertyName = "isOwner")]
        public bool? IsOwner { get; set; }

        [JsonProperty(PropertyName = "historyList")]
        public IList<ReportHistoryDto> HistoryList { get; set; }

        [JsonIgnore]
        public string TypeIconUrl { get; set; }

        [JsonIgnore]
        public string TypeName { get; set; }

        [JsonIgnore]
        public Color StatusColor
        {
            get
            {
                if (StatusColors.TryGetValue(Status, out Color color))
                {
                    return color;
                }
                else
                {
                    return (Color)App.Current.Resources["BadgeColorLightGray"];
                }
            }
        }
    }

    public class ReportsListDto : WsDMDto
    {
        [JsonProperty(PropertyName = "Reports")]
        public IList<ReportDto> ReportsList { get; set; }
    }

}
