using Newtonsoft.Json;
using OnDijon.Common.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnDijon.Modules.Report.Entities.Request
{
    public class ReportContentRequest
    {
        [JsonProperty(PropertyName = "RegistrationToken")]
        public string RegistrationToken { get; set; }

        [JsonProperty(PropertyName = "ReportTypeCode")]
        public string ReportTypeCode { get; set; }

        [JsonIgnore]
        public string ReportTypeName { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Position")]
        public CoordinatesRequest Position { get; set; }

        [JsonIgnore]
        public string Address { get; set; }

        [JsonIgnore]
        public IList<byte[]> Photos { get; set; }

        [JsonProperty(PropertyName = "Photos")]
        public IList<string> PhotosBase64
        {
            get
            {
                return PhotosToBase64();
            }
        }

        /// <summary>
        /// Convert photos to base64 with prefix "photo1Base64="
        /// </summary>
        private IList<string> PhotosToBase64()
        {
            return Photos?.Select((bytes, index) => $"photo{index + 1}Base64={Convert.ToBase64String(bytes)}").ToList();
        }
    }

    public class ReportRequest : DtoRequest
    {
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }
        
        [JsonProperty(PropertyName = "ProfileEditId")]
        public string ProfileEditId { get; set; }

        [JsonProperty(PropertyName = "ReportRequest")]
        public ReportContentRequest ReportContent { get; set; } = new ReportContentRequest();
    }
}
