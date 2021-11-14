using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationToList
    {
        public int Id { get; set; }
        public string BaseStationName { get; set; }
        public int FreeChargeSlots { get; set; }
        public int FullChargeSlots { get; set; }
        public override string ToString()
        {
            return $"BaseStationToList: Id:{Id}, BaseStationName:{BaseStationName},FreeChargeSlots:{FreeChargeSlots},FullChargeSlots:{FullChargeSlots} ";
        }
    }
}
