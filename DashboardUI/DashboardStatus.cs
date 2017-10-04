using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DashboardUI
{
    public class DashboardStatus
    {
        public DashboardStatus(
            int producedEnergy,
            int consumedEnergy,
            int chargeEnergy,
            List<ChargePointStatus> chargePoints)
        {
            ProducedEnergy = producedEnergy;
            ConsumedEnergy = consumedEnergy;
            ChargeEnergy = chargeEnergy;
            ChargePoints = chargePoints;
        }

        public int ProducedEnergy { get; set; }

        public int ConsumedEnergy { get; set; }

        public int ChargeEnergy { get; set; }

        public List<ChargePointStatus> ChargePoints { get; set; }
    }
}
