﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        Task RegisterDashboardAsync(string dashboardId, CancellationToken cancellationToken);

        Task ProcessChargeRecordAsync(ChargeRecord chargeRecord, CancellationToken cancellationToken);
    }
}
