using System;
using Microsoft.AspNet.SignalR;

namespace GatewayApi.Hubs
{
    public class DashboardHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.message(message);
        }
    }
}
