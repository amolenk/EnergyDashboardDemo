using System.Threading.Tasks;
using ChargePointActor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace GatewayApi.Controllers
{
    [Route("api/[controller]")]
    public class ChargePointController : Controller
    {
        // PUT api/meter/building
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]ChargeRecord record)
        {
            ActorId actorId = new ActorId(id);
            IChargePointActor actorProxy = ActorProxy.Create<IChargePointActor>(actorId);

            await actorProxy.ProcessChargeRecordAsync(record);
        }
    }
}
