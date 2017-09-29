using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardActor.Interfaces
{
    public class Topology
    {
        public List<string> MeterIds { get; set; }

        public List<string> ChargePointIds { get; set; }
    }
}
