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
        public Location BaseStationLocation { get; set; }
        public int FreeChargeSlots { get; set; }
        public List<DroneInCharge> DronesInChargeList { get; set; }
        public override string ToString()
        {
            return $"BaseStation: Id:{Id}, BaseStationName:{BaseStationName}, Location:{BaseStationLocation},FreeChargeSloted:{FreeChargeSlots}" +
               "DroneInCharge:" + String.Join(",",DronesInChargeList); 
               
        }
    }
}
