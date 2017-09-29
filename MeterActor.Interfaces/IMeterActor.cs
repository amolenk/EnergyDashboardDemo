﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;

namespace MeterActor.Interfaces
{
    /// <summary>
    /// This interface defines the methods exposed by an actor.
    /// Clients use this interface to interact with the actor that implements it.
    /// </summary>
    public interface IMeterActor : IActor
    {
        Task RegisterDashboardAsync(string dashboardId, CancellationToken cancellationToken);

        Task ProcessReadingAsync(object reading, CancellationToken cancellationToken);
    }
}
