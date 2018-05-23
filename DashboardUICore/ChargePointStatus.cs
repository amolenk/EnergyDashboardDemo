using System.Runtime.Serialization;

namespace DashboardUI
{
    public class ChargePointStatus
    {
        public ChargePointStatus(int position, string status)
        {
            Position = position;
            Status = status;
        }

        public int Position { get; private set; }

        public string Status { get; private set; }
    }
}
