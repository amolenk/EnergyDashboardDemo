using System;
using System.Threading.Tasks;
using ChargePointOperatorActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ChargePointOperatorActor
{
    [StatePersistence(StatePersistence.Persisted)]
    internal class ChargePointOperatorActor : Actor, IChargePointOperatorActor, IRemindable
    {
        public ChargePointOperatorActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            var reminderRegistration = RegisterReminderAsync(
                    "SubscribeToCpo",
                    new byte[0],
                    TimeSpan.Zero,
                    TimeSpan.FromHours(11));

            return base.OnActivateAsync();
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName.Equals("SubscribeToCpo"))
            {
                // TODO Make API call to CPO.
            }

            return Task.FromResult(true);
        }
    }
}
