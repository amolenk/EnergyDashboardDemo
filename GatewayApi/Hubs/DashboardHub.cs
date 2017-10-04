using System.Collections.Generic;
using System.Threading.Tasks;
using DashboardActor.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace GatewayApi.Hubs
{
    public class DashboardHub : Hub, IDashboardEvents
    {
        private List<string> _subscribedDashboards;

        public DashboardHub()
        {
            _subscribedDashboards = new List<string>();
        }

        public override async Task OnConnected()
        {
            var dashboardId = "demo";

            if (!_subscribedDashboards.Contains(dashboardId))
            {
                var proxy = ActorProxy.Create<IDashboardActor>(new ActorId(dashboardId));
                await proxy.SubscribeAsync<IDashboardEvents>(this);

                _subscribedDashboards.Add(dashboardId);
            }
        }

        public void DashboardUpdated(DashboardStatus status)
        {
            Clients.All.message(status);
        }
    }
}
