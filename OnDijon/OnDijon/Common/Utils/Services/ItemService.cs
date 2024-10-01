using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnDijon.Common.Utils.Services
{
    public class ItemService
    {
        public static async Task<T> GetItemAsync<T>(string path)
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(path);

                return JsonConvert.DeserializeObject<T>(content);
            }
        }
    }
}
