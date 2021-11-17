using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneBL
    {
        public int DroneId { get; set; }
        public string Model { get; set; }
        public WeightCategoriesBL DroneWeight { get; set; }
        public double BatteryPercent { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public ParcelInShipment ParInShip { get; set; }
        public Location CurrentLocation { get; set; }
        public override string ToString()
        {
            return $"DroneBL: DroneId:{DroneId}, Model:{Model}, DroneWeight:{DroneWeight}, BatteryPercent:{BatteryPercent}, DroneStatus:{DroneStatus}, ParcelInShipment:{ParInShip}, Location:{CurrentLocation}";
        }
    }
}
