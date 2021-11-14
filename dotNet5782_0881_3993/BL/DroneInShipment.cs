using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInShipment
    {
        public int DroneId { get; set; }
        public double BatteryPercent { get; set; }
        public Location Current { get; set; }

        
    }
}
