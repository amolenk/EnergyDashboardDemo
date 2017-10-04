using System.Threading.Tasks;
using MeterActor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace GatewayApi.Controllers
{
    [Route("api/[controller]")]
    public class MeterController : Controller
    {
        // PUT api/meter/building
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody]int reading)
        {
            ActorId actorId = new ActorId(id);
            IMeterActor actorProxy = ActorProxy.Create<IMeterActor>(actorId);

            await actorProxy.ProcessReadingAsync(reading);
        }
    }
}
