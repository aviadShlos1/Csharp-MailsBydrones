//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// This class presents a drone bl entity
    /// </summary>
    public class DroneBl
    {
        public int DroneId { get; set; }
        public string Model { get; set; }
        public WeightCategoriesBL DroneWeight { get; set; }
        public double BatteryPercent { get; set; }
        public DroneStatusesBL DroneStatus { get; set; }
        public ParcelInShipment ParcelInShip { get; set; } // This field contains the details of the parcek in shipment
        public Location DroneLocation { get; set; }
        public override string ToString()
        {
            return $"DroneId: {DroneId}, Model: {Model}, DroneWeight: {DroneWeight}, BatteryPercent: {BatteryPercent}, DroneStatusesBL: {DroneStatus}, ParcelInShipment: {ParcelInShip}, Location: {DroneLocation}";
        }
    }
}
