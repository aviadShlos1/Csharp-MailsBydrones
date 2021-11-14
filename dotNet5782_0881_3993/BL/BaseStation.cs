using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStation
    {
        public int Id { get; set; }
        public string BaseStationName { get; set; }
        public Location Loc { get; set; }
        public int FreeChargeSlots { get; set; }
        public List<DroneInCharge> DronesInChargeList { get; set; }
        public override string ToString()
        {
            return $"BaseStation: Id:{Id}, BaseStationName:{BaseStationName}, Location:{Loc},FreeChargeSloted:{FreeChargeSlots}" +
               "DroneInCharge:" + String.Join(",",DronesInChargeList); 
               
        }
    }
}
