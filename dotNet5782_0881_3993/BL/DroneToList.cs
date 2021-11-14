using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneToList
    {
        public int DroneId { get; set; }
        public string Model { get; set; }
        public WeightCategories DroneWeight { get; set; }
        public double BatteryPercent { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public Location Current { get; set; }
        public int CurrentTransferParcelNum { get; set; }
        public override string ToString()
        {
            return $"Drone: DroneId:{DroneId}, Model:{Model}, DroneWeight:{DroneWeight}, BatteryPercent:{BatteryPercent}, DroneStatus:{DroneStatus}, Location:{Current}, CurrentTransferParcelNum:{CurrentTransferParcelNum}";
        }
    }
}
