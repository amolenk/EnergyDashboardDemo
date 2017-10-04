using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DashboardUI
{
    public static class HttpClientExtensions
    {
        public static async Task PostJsonAsync(this HttpClient client, string requestUri, JToken json)
        {
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            await client.PostAsync(requestUri, content);
        }

        public static async Task PutJsonAsync(this HttpClient client, string requestUri, JToken json)
        {
            var content = new StringContent(json.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PutAsync(requestUri, content);
        }
    }
}
