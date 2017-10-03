using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        /// <summary>
        /// Initializes a new instance of DashboardActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public DashboardActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        public Task RegisterAsync(Topology topology)
        {
            var dashboardId = Id.GetStringId();

            return Task.WhenAll(
                RegisterTopologyWithChargePointsAsync(dashboardId, topology.ChargePointIds),
                RegisterTopologyWithMetersAsync(dashboardId, topology.MeterIds));
        }

        private static Task RegisterTopologyWithChargePointsAsync(string dashboardId, IEnumerable<string> chargePointIds)
        {
            var registerTasks = chargePointIds.Select(id =>
            {
                var chargePointActor = ActorProxy.Create<IChargePointActor>(new ActorId(id));
                return chargePointActor.RegisterDashboardAsync(dashboardId);
            });  

            return Task.WhenAll(registerTasks);
        }

        private static Task RegisterTopologyWithMetersAsync(string dashboardId, IEnumerable<string> meterIds)
        {
            var registerTasks = meterIds.Select(id =>
            {
                var meterActor = ActorProxy.Create<IMeterActor>(new ActorId(id));
                return meterActor.RegisterDashboardAsync(dashboardId);
            });

            return Task.WhenAll(registerTasks);
        }

        public Task UpdateDashboardAsync()
        {
            // TODO Calculate some stuff.
            // TODO Publish Actor events.

            throw new NotImplementedException();
        }

        public Task UpdateValue()
        {
            throw new NotImplementedException();
        }
    }
}
