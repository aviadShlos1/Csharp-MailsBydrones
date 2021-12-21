//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// This class presents a base station entity
    /// </summary>
    public class BaseStationBl
    {
        public int Id { get; set; }
        public string BaseStationName { get; set; }
        public Location Location { get; set; }
        public int FreeChargeSlots { get; set; }
        public List<DroneInCharge> DronesInChargeList { get; set; } //List of the drones in charge
        public override string ToString()
        {
            return $"Id: {Id}, BaseStationName: {BaseStationName}, Location: {Location},FreeChargeSloted: {FreeChargeSlots} " +
               "DroneInCharge: " + String.Join(",",DronesInChargeList); 
               
        }
    }
}
