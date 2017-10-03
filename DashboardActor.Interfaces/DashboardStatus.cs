using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DashboardActor.Interfaces
{
    [DataContract]
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

        [DataMember]
        public int ProducedEnergy { get; set; }

        [DataMember]
        public int ConsumedEnergy { get; set; }

        [DataMember]
        public int ChargeEnergy { get; set; }

        [DataMember]
        public List<ChargePointStatus> ChargePoints { get; set; }
    }
}
