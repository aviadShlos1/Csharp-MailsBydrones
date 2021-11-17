using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class AssignDroneToParcel
    {
        public int Id { get; set; }
        public double BatteryPercent { get; set; }
        public Location Current { get; set; }

        public override string ToString()
        {
            return $"AssignDroneToParcel: CustomerId:{Id}, BatteryPercent:{BatteryPercent}, Location:{Current}";
        }

    }
}
