using System;
using System.Threading;
using System.Threading.Tasks;
using DashboardActor.Interfaces;
using Microsoft.ServiceFabric.Actors;

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
