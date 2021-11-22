using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IBL.BO;
namespace IBL
{
    partial class BL
    {
        public BaseStationBL GetBaseStation(int baseStationId)
        {
            IDAL.DO.BaseStation myBaseStation = new();
            try
            {
                myBaseStation = DalAccess.GetBaseStation(baseStationId);
            }
            catch (IDAL.DO.NotExistException)
            {
               /* throw new BO.NotExistException(baseStationId,"baseStationId",*/ /*);*/
            }
            
            Location myLocation = new() { Latitude = myBaseStation.Latitude, Longitude = myBaseStation.Longitude };
            BaseStationBL myStationBl = new() { Id = myBaseStation.Id, BaseStationName = myBaseStation.Name, Location = myLocation, FreeChargeSlots = myBaseStation.FreeChargeSlots, DronesInChargeList = new() };
            
            var dronesInChargePerStation = DalAccess.GetDronesChargeList().TakeWhile(x => x.StationId == baseStationId).ToList();
            foreach (var item in dronesInChargePerStation)
            {
                var tempDrone = DronesListBL.Find(x => x.DroneId == item.DroneId);
                myStationBl.DronesInChargeList.Add( new DroneInCharge { Id = tempDrone.DroneId, BatteryPercent = tempDrone.BatteryPercent });
            }
            return myStationBl;
        }
        public List<BO.DroneInShipment> GetDrone(int droneId)
        {
            DalAccess.GetDrone(droneId);


        }       
        public  GetCustomer(int customerId)
        {
            DalAccess.GetCustomer(customerId);
        }
        public void GetParcel(int parcelId)
        {
            DalAccess.GetParcel(parcelId);
        }
        

    }
}
