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
        public DroneStatus DronStatus { get; set; }
        public Location Current { get; set; }
        public int CurrTransferParcelNument{ get; set; }
         
    }
}
