using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationBl
    {
        public int Id { get; set; }
        public string BaseStationName { get; set; }
        public Location Location { get; set; }
        public int FreeChargeSlots { get; set; }
        public List<DroneInCharge> DronesInChargeList { get; set; }
        public override string ToString()
        {
            return $"BaseStationBl: CustomerId:{Id}, BaseStationName:{BaseStationName}, Location:{Location},FreeChargeSloted:{FreeChargeSlots}" +
               "DroneInCharge:" + String.Join(",",DronesInChargeList); 
               
        }
    }
}
