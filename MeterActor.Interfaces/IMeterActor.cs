using System;
using System.Threading;
using System.Threading.Tasks;
using DashboardActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;

[assembly: FabricTransportActorRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace MeterActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IMeterActor : IActor
    {
        Task RegisterDashboardAsync(string dashboardId, MeterReadingType meterReadingType);

        Task ProcessReadingAsync(int reading);
    }
}
