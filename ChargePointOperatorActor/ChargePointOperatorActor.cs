using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using ChargePointOperatorActor.Interfaces;

namespace ChargePointOperatorActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class ChargePointOperatorActor : Actor, IChargePointOperatorActor, IRemindable
    {
        private const string UpdateReminder = "DashboardUpdated";

        /// <summary>
        /// Initializes a new instance of ChargePointOperatorActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public ChargePointOperatorActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            var reminderRegistration = RegisterReminderAsync(
                    UpdateReminder,
                    new byte[0],
                    TimeSpan.Zero,
                    TimeSpan.FromSeconds(3));

            return base.OnActivateAsync();
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName.Equals(UpdateReminder))
            {
                //var ev = GetEvent<IDashboardEvents>();
                //ev.DashboardUpdated(DateTime.UtcNow.ToString());
            }

            return Task.FromResult(true);
        }
    }
}
