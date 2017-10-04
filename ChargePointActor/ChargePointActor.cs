using System;
using System.Linq;
using System.Threading.Tasks;
using ChargePointActor.Interfaces;
using DashboardActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ChargePointActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class ChargePointActor : Actor, IChargePointActor
    {
        public ChargePointActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task RegisterDashboardAsync(string dashboardId, int position)
        {
            await StateManager.SetStateAsync($"Dashboard:{dashboardId}", position);
        }

        public async Task NotifyChargeSessionStartedAsync()
        {
            // The chargePoint is clearly occupied when the chargeSessionStarted is received.
            var occupied = true;

            // The chargePoint is not yet charging when the chargeSessionStarted is received.
            // We have to wait for a ChargeSessionUpdated.
            var charging = false;

            await UpdateDashboardsAsync(occupied, charging);
        }

        public async Task NotifyChargeSessionEndedAsync()
        {
            // The chargePoint is no longer occupied, and thus not charging.
            var charging = false;
            var occupied = false;

            // Clean up state.
            await StateManager.TryRemoveStateAsync("LastSessionCharge");
            await StateManager.TryRemoveStateAsync("LastSessionUpdate");

            await UpdateDashboardsAsync(occupied, charging);
        }

        public async Task NotifyChargeSessionUpdatedAsync(long sessionCharge)
        {
            // The ChargePoint is occupied.
            var occupied = true;

            // Get and update the time of the last session update.
            var lastUpdateTime = await StateManager.TryGetStateAsync<DateTime>("LastSessionUpdate");
            await StateManager.SetStateAsync("LastSessionUpdate", DateTime.UtcNow);

            // Get and update the charge value of the last session update.
            var lastSessionCharge = await StateManager.TryGetStateAsync<long>("LastSessionCharge");
            await StateManager.SetStateAsync("LastSessionCharge", sessionCharge);

            // Calculate the time that has passed since the last update.
            var timeSinceLastUpdate = lastUpdateTime.HasValue ? DateTime.UtcNow - lastUpdateTime.Value : TimeSpan.Zero;

            // Calculate the charge progress.
            var chargeProgress = sessionCharge - (lastSessionCharge.HasValue ? lastSessionCharge.Value : 0);

            // We're still charging if there's been some progress.
            var charging = (chargeProgress > 0);

            await UpdateDashboardsAsync(occupied, charging);
        }

        private async Task UpdateDashboardsAsync(bool occupied, bool charging)
        {
            var status = charging ? "charging" : (occupied ? "occupied" : "available");

            // Distribute charge record to all interested dashboards.
            var tasks = (await StateManager.GetStateNamesAsync())
                .Where(name => name.StartsWith("Dashboard:"))
                .Select(async name =>
                {
                    var dashboardId = name.Substring("Dashboard:".Length);
                    var position = await StateManager.GetStateAsync<int>(name);

                    var proxy = ActorProxy.Create<IDashboardActor>(new ActorId(dashboardId));
                    return proxy.UpdateChargePointAsync(position, status);
                });

            await Task.WhenAll(tasks);
        }
    }
}
