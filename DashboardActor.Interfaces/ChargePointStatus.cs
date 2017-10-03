using System.Runtime.Serialization;

namespace DashboardActor.Interfaces
{
    [DataContract]
    public class ChargePointStatus
    {
        public ChargePointStatus(int position, string status)
        {
            Position = position;
            Status = status;
        }

        [DataMember]
        public int Position { get; private set; }

        [DataMember]
        public string Status { get; private set; }
    }
}
