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
    /// This class presents a base station to list entity
    /// </summary>
    public class BaseStationToList
    {
        public int Id { get; set; }
        public string BaseStationName { get; set; }
        public int FreeChargeSlots { get; set; } 
        public int BusyChargeSlots { get; set; } // full charge slots
        public override string ToString()
        {
            return $"BaseStationId:{Id}, BaseStationName:{BaseStationName},FreeChargeSlots:{FreeChargeSlots},BusyChargeSlots:{BusyChargeSlots} ";
        }
    }
}
