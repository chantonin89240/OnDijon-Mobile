using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace OnDijon.Modules.Dashboard.Entities.Dto.Card
{
    public class CardDto : Common.Entities.Dto.Dto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "CardType")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "Image")]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "Color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "ServiceId")]
        public string ServiceId { get; set; }

        [JsonProperty(PropertyName = "Tags")]
        public ObservableCollection<CardTagDto> Tags { get; set; }

        [JsonProperty(PropertyName = "Actions")]
        public ObservableCollection<CardActionDto> Actions { get; set; }

        [JsonProperty(PropertyName = "ImagePosition")]
        public string ImagePosition { get; set; }

        [JsonIgnore]
        public bool ImagePositionRight
        {
            get
            {
                return ImagePosition != null && ImagePosition.Equals("right");
            }
        }
    }
}
