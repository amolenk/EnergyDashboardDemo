using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace DashboardActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IDashboardActor : IActor, IActorEventPublisher<IDashboardEvents>
    {
        Task RegisterAsync(Topology topology);

        Task UpdateChargePointAsync(int position, string status);

        Task UpdateMeterStatus(MeterReadingType readingType, int reading);
    }
}
