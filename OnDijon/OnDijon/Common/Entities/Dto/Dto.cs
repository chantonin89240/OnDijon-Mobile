using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDijon.Common.Entities.Dto
{
    public class Dto
    {
        private static readonly JsonSerializer _serializer = new JsonSerializer();

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public static async Task<T> FromHttpContentAsync<T>(HttpContent content) where T : Dto
        {
            using (var reader = new StreamReader(await content.ReadAsStreamAsync()))
            using (var json = new JsonTextReader(new StringReader(reader.ReadToEnd())))
            {
                return _serializer.Deserialize<T>(json);
            }
        }

        public static async Task<List<T>> ListFromHttpContentAsync<T>(HttpContent content) where T : Dto
        {
            using (var reader = new StreamReader(await content.ReadAsStreamAsync()))
            using (var json = new JsonTextReader(reader))
            {
                return _serializer.Deserialize<List<T>>(json);
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
