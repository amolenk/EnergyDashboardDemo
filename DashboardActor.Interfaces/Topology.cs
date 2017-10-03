using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DashboardActor.Interfaces
{
    [DataContract]
    public class Topology
    {
        public Topology(
            string producedEnergyMeterId,
            string consumedEnergyMeterId,
            string chargeEnergyMeterId,
            Dictionary<int, string> chargePointIds)
        {
            ProducedEnergyMeterId = producedEnergyMeterId;
            ConsumedEnergyMeterId = consumedEnergyMeterId;
            ChargeEnergyMeterId = chargeEnergyMeterId;
            ChargePointIds = chargePointIds;
        }

        [DataMember]
        public string ProducedEnergyMeterId { get; private set; }

        [DataMember]
        public string ConsumedEnergyMeterId { get; private set; }

        [DataMember]
        public string ChargeEnergyMeterId { get; private set; }

        [DataMember]
        public Dictionary<int, string> ChargePointIds { get; private set; }
    }
}
