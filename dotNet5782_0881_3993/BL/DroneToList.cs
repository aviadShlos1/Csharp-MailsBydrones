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
    /// This class presents a drone to list entity
    /// </summary>
    public class DroneToList
    {
        public int DroneId { get; set; }
        public string Model { get; set; }
        public WeightCategoriesBL DroneWeight { get; set; }
        public double BatteryPercent { get; set; }
        public DroneStatus DroneStatus { get; set; }
        public Location DroneLocation { get; set; }
        public int TransferParcelsNum { get; set; } // This field includes the parcel id which is related to this drone 
        public override string ToString()
        {
            return $"DroneId: {DroneId}, Model: {Model}, DroneWeight: {DroneWeight}, BatteryPercent: {BatteryPercent}, DroneStatus: {DroneStatus}, Location: {DroneLocation}, TransferParcelsNum: {TransferParcelsNum}";
        }
    }
}
