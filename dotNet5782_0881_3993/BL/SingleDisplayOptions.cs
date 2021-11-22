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
            IDAL.DO.BaseStation dalBaseStation = new();
            try
            {
                dalBaseStation = DalAccess.GetSingleBaseStation(baseStationId);
            }
            catch (IDAL.DO.NotExistException)
            {
               /* throw new BO.NotExistException(baseStationId,"baseStationId",*/ /*);*/
            }
            
            Location myLocation = new() { Latitude = dalBaseStation.Latitude, Longitude = dalBaseStation.Longitude };
            BaseStationBL myStationBl = new() { Id = dalBaseStation.Id, BaseStationName = dalBaseStation.Name, Location = myLocation, FreeChargeSlots = dalBaseStation.FreeChargeSlots, DronesInChargeList = new() };
            
            var dronesInChargePerStation = DalAccess.GetDronesChargeList().TakeWhile(x => x.StationId == baseStationId).ToList();
            foreach (var item in dronesInChargePerStation)
            {
                var tempDrone = DronesListBL.Find(x => x.DroneId == item.DroneId);
                myStationBl.DronesInChargeList.Add( new DroneInCharge { Id = tempDrone.DroneId, BatteryPercent = tempDrone.BatteryPercent });
            }
            return myStationBl;
        }
        public DroneBL GetDrone(int myDroneId)
        {
            IDAL.DO.Drone dalDrone = new();
            try
            {
                dalDrone = DalAccess.GetSingleDrone(myDroneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw;
            }

            var tempDroneBl = DronesListBL.Find(x => x.DroneId == myDroneId);
            if (tempDroneBl.DroneStatus==DroneStatus.Shipment)
            {

            }
            
            DroneBL myDroneBl = new() {DroneId=dalDrone.Id , Model=dalDrone.Model , DroneWeight =(WeightCategoriesBL)dalDrone.DroneWeight , BatteryPercent=tempDroneBl.BatteryPercent , DroneStatus=tempDroneBl.DroneStatus , /*ParcelInShip=*/ DroneLocation=tempDroneBl.DroneLocation};
           
            return myDroneBl;


        }       
        public void GetCustomer(int customerId)
        {
            DalAccess.GetSingleCustomer(customerId);
        }
        public void GetParcel(int parcelId)
        {
            DalAccess.GetSingleParcel(parcelId);
        }
        

    }
}
