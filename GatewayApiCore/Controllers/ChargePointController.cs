using System.Threading.Tasks;
using ChargePointActor.Interfaces;
using GatewayApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace GatewayApi.Controllers
{
    [Route("api/[controller]")]
    public class ChargePointController : Controller
    {
        // PUT api/chargepoint/{id}
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]ChargeRecord record)
        {
            ActorId actorId = new ActorId(id);
            IChargePointActor actorProxy = ActorProxy.Create<IChargePointActor>(actorId);

            // service = ChargePointActorType

            switch (record.EventType)
            {
                case "sessionStarted":
                    await actorProxy.NotifyChargeSessionStartedAsync();
                    break;

                case "sessionEnded":
                    await actorProxy.NotifyChargeSessionEndedAsync();
                    break;

                case "sessionUpdated":
                    await actorProxy.NotifyChargeSessionUpdatedAsync(long.Parse(record.EventPayload));
                    break;
            }
        }
    }
}
