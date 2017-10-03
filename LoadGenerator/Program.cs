using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LoadGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9061/");

            await client.PostJsonAsync("api/dashboard/demo", JObject.FromObject(new
            {
                producedEnergyMeterId = "meter1",
                consumedEnergyMeterId = "meter2",
                chargeEnergyMeterId = "meter3"
            }));
        }
    }
}
