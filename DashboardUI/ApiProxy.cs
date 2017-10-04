using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DashboardUI
{
    class ApiProxy
    {
        private readonly HttpClient _client;

        public ApiProxy()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:9061/");
        }

        public async Task RegisterTopologyAsync()
        {
            await _client.PostJsonAsync("api/dashboard/demo", JObject.FromObject(new
            {
                producedEnergyMeterId = "meter1",
                consumedEnergyMeterId = "meter2",
                chargeEnergyMeterId = "meter3",
                chargePointIds = new Dictionary<int, string>
                {
                    [1] = "chargePoint1",
                    [2] = "chargePoint2",
                    [3] = "chargePoint3",
                    [4] = "chargePoint4",
                    [5] = "chargePoint5",
                    [6] = "chargePoint6",
                    [7] = "chargePoint7",
                    [8] = "chargePoint8",
                    [9] = "chargePoint9",
                    [10] = "chargePoint10"
                }
            }));
        }

        public async Task StartSessionAsync(string chargePointId)
        {
            await _client.PutJsonAsync($"api/chargepoint/{chargePointId}", JObject.FromObject(new
            {
                eventType = "sessionStarted"
            }));
        }

        public async Task UpdateSessionAsync(string chargePointId, long charge)
        {
            await _client.PutJsonAsync($"api/chargepoint/{chargePointId}", JObject.FromObject(new
            {
                eventType = "sessionUpdated",
                eventPayload = charge
            }));
        }

        public async Task EndSessionAsync(string chargePointId)
        {
            await _client.PutJsonAsync($"api/chargepoint/{chargePointId}", JObject.FromObject(new
            {
                eventType = "sessionEnded"
            }));
        }

        public async Task UpdateMeterReadingAsync(string meterId, int reading)
        {
            await _client.PutJsonAsync($"api/meter/{meterId}", reading);
        }
    }
}
