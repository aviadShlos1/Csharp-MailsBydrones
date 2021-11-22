using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;

namespace IBL
{
    partial class BL
    {
        public void AddBaseStation(BaseStationBl newBaseStationBl)
        {
            try
            {
                BaseStationDal tempBase = new();
                tempBase.Id = newBaseStationBl.Id;
                tempBase.Name = newBaseStationBl.BaseStationName;
                tempBase.Latitude = newBaseStationBl.Location.Latitude;
                tempBase.Longitude = newBaseStationBl.Location.Longitude;
                tempBase.FreeChargeSlots = newBaseStationBl.FreeChargeSlots;
                DalAccess.AddStation(tempBase);
            }
            catch (IDAL.DO.AlreadyExistException)
            {
                throw;
            }
        }
        public void AddDrone(DroneToList newDroneBl, int firstChargeStation)
        {
            try
            {
                DroneDal tempDrone = new();
                tempDrone.Id = newDroneBl.DroneId;
                tempDrone.Model = newDroneBl.Model;
                tempDrone.DroneWeight = (WeightCategoriesDal)newDroneBl.DroneWeight;
                
                foreach (var item in DalAccess.GetBaseStationsList())
                {
                    if (item.Id == firstChargeStation)
                    {
                        newDroneBl.DroneLocation.Longitude = item.Longitude;
                        newDroneBl.DroneLocation.Latitude = item.Latitude;
                    }
                }
                newDroneBl.BatteryPercent = (rand.NextDouble() * 20) + 20;
                newDroneBl.DroneStatus = DroneStatus.Maintaince;
                DalAccess.AddDrone(tempDrone);
                DronesListBL.Add(newDroneBl);
            }
            catch (IDAL.DO.AlreadyExistException)
            {
                throw ;
            }
        }

        public void AddCustomer(CustomerBL newCustomer)
        {
            try
            {
                CustomerDal tempCust = new();
                tempCust.Id = newCustomer.CustomerId;
                tempCust.Name = newCustomer.CustomerName;
                tempCust.Phone = newCustomer.CustomerPhone;
                tempCust.CustomerLongitude = newCustomer.CustomerLocation.Longitude;
                tempCust.CustomerLatitude = newCustomer.CustomerLocation.Latitude;
                DalAccess.AddCustomer(tempCust);

            }
            catch (IDAL.DO.AlreadyExistException)
            {

                throw;
            }
        }
        public void AddParcel(int mySenderId, int myRecieverId, WeightCategoriesBL myParcelWeight, PrioritiesBL myPriority)
        {
            try
            {
                ParcelDal tempParcel = new();
                tempParcel.SenderId = mySenderId;
                tempParcel.TargetId = myRecieverId;
                tempParcel.Weight = (WeightCategoriesDal)myParcelWeight;
                tempParcel.Priority = (Priorities)myPriority;
                tempParcel.CreatingTime = DateTime.Now;
                tempParcel.AssignningTime = DateTime.MinValue;
                tempParcel.PickingUpTime = DateTime.MinValue;
                tempParcel.SupplyingTime = DateTime.MinValue;
                tempParcel.DroneToParcelId = 0;////
                DalAccess.AddParcel(tempParcel);
                //ParcelBl parcelBL = new();
                //parcelBL.DroneAssignToParcel = null;

            }
            catch (IDAL.DO.AlreadyExistException)
            {

                throw;
            }
        }


    }
}
