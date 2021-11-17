using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInCharge
    {
        public int Id { get; set; }
        public double BatteryPercent { get; set; }
        public override string ToString()
        {
            return $"DroneBL: DroneId:{Id},  BatteryPercent:{BatteryPercent}";
        }
    }
}
