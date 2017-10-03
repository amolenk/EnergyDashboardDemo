using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace ChargePointActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IChargePointActor : IActor
    {
        Task RegisterDashboardAsync(string dashboardId, int position);

        Task NotifyChargeSessionStartedAsync();

        Task NotifyChargeSessionEndedAsync();

        Task NotifyChargeSessionUpdatedAsync(long sessionCharge);
    }
}
