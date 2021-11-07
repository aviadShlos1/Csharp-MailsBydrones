using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStation
    {
        int Id;
        string BaseStationName;
        Location Loc;
        int FreeChargeSlots;
        List<DroneInCharge> DronesInChargeList; 
    }
}
