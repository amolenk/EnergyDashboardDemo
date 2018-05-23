using System;
using DashboardActor.Interfaces;
using GatewayApiCore.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GatewayApiCore
{
    public class DashboardEventHandler : IDashboardEvents
    {
        private readonly IHubContext<DashboardHub> _hubContext;

        public DashboardEventHandler(IHubContext<DashboardHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void DashboardUpdated(DashboardStatus status)
        {
            _hubContext.Clients.All.SendAsync("message", status)
                .GetAwaiter().GetResult();
        }
    }
}
