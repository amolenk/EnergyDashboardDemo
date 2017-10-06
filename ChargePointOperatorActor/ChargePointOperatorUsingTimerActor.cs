using System;
using System.Threading.Tasks;
using ChargePointOperatorActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace ChargePointOperatorActor
{
    internal class ChargePointOperatorUsingTimerActor : Actor, IChargePointOperatorActor
    {
        private IActorTimer _subscribeTimer;

        public ChargePointOperatorUsingTimerActor(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }

        protected override Task OnActivateAsync()
        {
            _subscribeTimer = RegisterTimer(
                SubscribeToCpoAsync,        // Callback method
                null,                       // Parameter to pass to the callback method
                TimeSpan.Zero,              // Amount of time to delay before the callback is invoked
                TimeSpan.FromHours(11));    // Time interval between invocations of the callback method

            return base.OnActivateAsync();
        }

        protected override Task OnDeactivateAsync()
        {
            if (_subscribeTimer != null)
            {
                UnregisterTimer(_subscribeTimer);
            }

            return base.OnDeactivateAsync();
        }

        private Task SubscribeToCpoAsync(object state)
        {
            // TODO Make API call to CPO.

            return Task.FromResult(true);
        }
    }
}
