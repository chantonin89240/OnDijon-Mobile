using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace OnDijon.Modules.Report.Entities.Dto
{
    public class ReportHistoryDto : Common.Entities.Dto.Dto
    {
        // refacto, ce code ne devrait pas être là dans un DTO 
        private static readonly Dictionary<string, Color> StatusColors = new Dictionary<string, Color>()
        {
            { "Créé", (Color)App.Current.Resources["BadgeColorRed"] },
            { "En cours de traitement", (Color)App.Current.Resources["BadgeColorYellow"] },
            { "Clôture",(Color)App.Current.Resources["BadgeColorGreen"] },
            { "Annulation", (Color)App.Current.Resources["BadgeColorLightGray"] }
        };


        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        
        // Refacto, Color ne devrait pas être utiliser dans une classe qui n'est pas lié à la vue 
        // ici on devrait utiliser un Value Converter 
        // regarder : ReportHistoryDtoStatusToColorValueConverter
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
}
