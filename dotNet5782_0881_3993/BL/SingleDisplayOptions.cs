using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    partial class BL
    {
        public List<BO.DroneInCharge> GetBaseStation(int baseStationId)
        {
            IDAL.DO.BaseStation myBaseStation = new();
            try
            {
                myBaseStation = DalAccess.GetBaseStation(baseStationId);
            }
            catch (Exception)
            {

                throw;
            }
            int i = 0;
            DalAccess.GetBaseStation(baseStationId);
            BO.BaseStationBL myStationBl = new();
            var dronesInChargePerStation = DalAccess.GetDronesChargeList().TakeWhile(x => x.StationId == baseStationId).ToList();
            foreach (var item in dronesInChargePerStation)
            {
                var tempDrone = DronesListBL.Find(x => x.DroneId == item.DroneId);
                myStationBl.DronesInChargeList[i].Id = tempDrone.DroneId;
                myStationBl.DronesInChargeList[i].BatteryPercent = tempDrone.BatteryPercent;
                i++;
            }
            return myStationBl.DronesInChargeList;
        }
        public List<BO.DroneInShipment> GetDrone(int droneId)
        {
            DalAccess.GetDrone(droneId);


        }
        //public void GetCustomer();
        //public void GetParcel();
        //}

    }
}
