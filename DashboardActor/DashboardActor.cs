using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChargePointActor.Interfaces;
using DashboardActor.Interfaces;
using MeterActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace DashboardActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class DashboardActor : Actor, IDashboardActor
    {
        public DashboardActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public async Task RegisterAsync(Topology topology)
        {
            var dashboardId = Id.GetStringId();

            await RegisterTopologyWithChargePointsAsync(dashboardId, topology);
            //await RegisterTopologyWithMetersAsync(dashboardId, topology);

            await PublishDashboardUpdatedEventAsync();
        }

        public async Task UpdateChargePointAsync(int position, string status)
        {
            var stateName = $"chargePoint:{position}";
            await StateManager.SetStateAsync(stateName, status);

            await PublishDashboardUpdatedEventAsync();
        }

        public async Task UpdateMeterStatus(MeterReadingType readingType, int reading)
        {
            var stateName = $"reading:{readingType}";
            await StateManager.SetStateAsync(stateName, reading);

            await PublishDashboardUpdatedEventAsync();
        }

        private async Task PublishDashboardUpdatedEventAsync()
        {
            var status = await GetDashboardStatusAsync();

            var ev = GetEvent<IDashboardEvents>();
            ev.DashboardUpdated(status);
        }

        private static Task RegisterTopologyWithChargePointsAsync(string dashboardId, Topology topology)
        {
            var registerTasks = topology.ChargePointIds.Select(chargePoint =>
            {
                var proxy = ActorProxy.Create<IChargePointActor>(new ActorId(chargePoint.Value));
                return proxy.RegisterDashboardAsync(dashboardId, chargePoint.Key);
            });  

            return Task.WhenAll(registerTasks);
        }

        private static Task RegisterTopologyWithMetersAsync(string dashboardId, Topology topology)
        {
            return Task.WhenAll(
                RegisterTopologyWithMeterAsync(dashboardId, topology.ProducedEnergyMeterId),
                RegisterTopologyWithMeterAsync(dashboardId, topology.ConsumedEnergyMeterId),
                RegisterTopologyWithMeterAsync(dashboardId, topology.ChargeEnergyMeterId));
        }

        private static Task RegisterTopologyWithMeterAsync(string dashboardId, string meterId)
        {
            var proxy = ActorProxy.Create<IMeterActor>(new ActorId(meterId));
            return proxy.RegisterDashboardAsync(dashboardId);
        }

        private async Task<DashboardStatus> GetDashboardStatusAsync()
        {
            var producedEnergy = await StateManager.TryGetStateAsync<int>("reading:ProducedEnergy");
            var consumedEnergy = await StateManager.TryGetStateAsync<int>("reading:ConsumedEnergy");
            var chargeEnergy = await StateManager.TryGetStateAsync<int>("reading:ChargeEnergy");

            var chargePoints = new List<ChargePointStatus>();
            var chargePointPositions = (await StateManager.GetStateNamesAsync())
                .Where(name => name.StartsWith("chargePoint:"))
                .Select(name => int.Parse(name.Substring("chargePoint:".Length)));

            foreach (var position in chargePointPositions)
            {
                var status = await StateManager.GetStateAsync<string>($"chargePoint:{position}");

                chargePoints.Add(new ChargePointStatus(position, status));
            }

            return new DashboardStatus(
                producedEnergy.HasValue ? producedEnergy.Value : 0,
                consumedEnergy.HasValue ? consumedEnergy.Value : 0,
                chargeEnergy.HasValue ? chargeEnergy.Value : 0,
                chargePoints);
        }
    }
}
