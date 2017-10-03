using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LoadGenerator
{
    public static class HttpClientExtensions
    {
        public static async Task PostJsonAsync(this HttpClient client, string requestUri, JObject json)
        {
            var content = new StringContent(json.ToString());

            await client.PostAsync(requestUri, content);
        }
    }
}
