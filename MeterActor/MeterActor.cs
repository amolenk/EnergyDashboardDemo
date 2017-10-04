using System.Linq;
using System.Threading.Tasks;
using DashboardActor.Interfaces;
using MeterActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace MeterActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class MeterActor : Actor, IMeterActor
    {
        public MeterActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task RegisterDashboardAsync(string dashboardId, MeterReadingType meterReadingType)
        {
            await StateManager.SetStateAsync($"Dashboard:{dashboardId}", meterReadingType);
        }

        public async Task ProcessReadingAsync(int reading)
        {
            // Distribute reading to all interested dashboards.
            var tasks = (await StateManager.GetStateNamesAsync())
                .Where(name => name.StartsWith("Dashboard:"))
                .Select(async name =>
                {
                    var dashboardId = name.Substring("Dashboard:".Length);
                    var meterReadingType = await StateManager.GetStateAsync<MeterReadingType>(name);

                    var proxy = ActorProxy.Create<IDashboardActor>(new ActorId(dashboardId));
                    return proxy.UpdateMeterStatus(meterReadingType, reading);
                });

            await Task.WhenAll(tasks);
        }
    }
}
