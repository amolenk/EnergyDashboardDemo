using System.Threading.Tasks;
using DashboardActor.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace GatewayApi.Hubs
{
    public class DashboardHub : Hub, IDashboardEvents
    {
        public override async Task OnConnected()
        {
            // TODO Only subscribe once!
            var proxy = ActorProxy.Create<IDashboardActor>(new ActorId("demo"));
            await proxy.SubscribeAsync<IDashboardEvents>(this);
        }

        public void DashboardUpdated(DashboardStatus status)
        {
            Clients.All.message(status);
        }
    }
}
