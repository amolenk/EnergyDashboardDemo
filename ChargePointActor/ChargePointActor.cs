using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private const string DashboardIdsState = "dashboardIds";

        public ChargePointActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return StateManager.TryAddStateAsync(DashboardIdsState, new List<string>());
        }

        public async Task RegisterDashboardAsync(string dashboardId, CancellationToken cancellationToken)
        {
            await StateManager.AddOrUpdateStateAsync(
                DashboardIdsState,
                new List<string> { dashboardId },
                (id, list) =>
                {
                    if (!list.Contains(id))
                    {
                        list.Add(id);
                    }
                    return list;
                });
        }

        public async Task ProcessChargeRecordAsync(ChargeRecord chargeRecord, CancellationToken cancellationToken)
        {
            // Distribute charge record to all interested dashboards.
            var dashboardIds = await StateManager.GetStateAsync<List<string>>(DashboardIdsState);

            var tasks = dashboardIds.Select(id =>
            {
                var dashboardActor = ActorProxy.Create<IDashboardActor>(new ActorId(id));
                return dashboardActor.UpdateValue();
            });

            await Task.WhenAll(tasks);
        }
    }
}
