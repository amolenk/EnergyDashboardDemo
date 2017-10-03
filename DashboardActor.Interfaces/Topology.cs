using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DashboardActor.Interfaces
{
    [DataContract]
    public class Topology
    {
        [DataMember]
        public List<string> MeterIds { get; set; }

        [DataMember]
        public List<string> ChargePointIds { get; set; }
    }
}
