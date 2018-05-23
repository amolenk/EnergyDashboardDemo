using System.Threading.Tasks;
using DashboardActor.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace GatewayApi.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        // POST api/dashboard/[id]
        [HttpPost("{id}")]
        public async Task Post(string id, [FromBody]Topology topology)
        {
            ActorId actorId = new ActorId(id);
            IDashboardActor actorProxy = ActorProxy.Create<IDashboardActor>(actorId);

            await actorProxy.RegisterAsync(topology);
        }
    }
}
