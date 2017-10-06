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

        protected override async Task OnActivateAsync()
        {
            var subscribeReminder = await RegisterReminderAsync(
                    "SubscribeToCpo",           // Reminder name
                    new byte[0],                // Any user state to pass to callback
                    TimeSpan.Zero,              // Amount of time to delay before the callback is invoked
                    TimeSpan.FromHours(11));    // Time interval between invocations of the callback method

            await base.OnActivateAsync();
        }

        public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            if (reminderName.Equals("SubscribeToCpo"))
            {
                await SubscribeToCpoAsync();
            }
        }

        private Task SubscribeToCpoAsync()
        {
            // TODO Make API call to CPO.

            return Task.FromResult(true);
        }
    }
}
