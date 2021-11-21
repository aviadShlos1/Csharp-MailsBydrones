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
        public void AddBaseStation(int myId, string myBaseStationName, Location myBaseStationLocation, int myFreeChargeSlots)
        {
            try
            {
                BaseStation tempBase = new();
                tempBase.Id = myId;
                tempBase.Name = myBaseStationName;
                tempBase.Latitude = myBaseStationLocation.Latitude;
                tempBase.Longitude = myBaseStationLocation.Longitude;
                tempBase.FreeChargeSlots = myFreeChargeSlots;
                DalAccess.AddStation(tempBase);

            }
            catch (IDAL.DO.AlreadyExistException)
            {

                throw;
            }
        }
        public void AddDrone(int myDroneId, string myModel, WeightCategoriesBL myDroneWeight, int myBaseStationId)
        {
            try
            {
                DroneToList droneBL = new();
                Drone tempDrone = new();
                tempDrone.Id = myDroneId;
                tempDrone.Model = myModel;
                tempDrone.DroneWeight = (WeightCategoriesDal)myDroneWeight;
                foreach (var item in DalAccess.GetBaseStationsList())
                {
                    if (item.Id == myBaseStationId)
                    {
                        droneBL.DroneLocation.Longitude = item.Longitude;
                        droneBL.DroneLocation.Latitude = item.Latitude;
                    }
                }
                droneBL.BatteryPercent = (rand.NextDouble() * 20) + 20;
                droneBL.DroneStatus = DroneStatus.Maintaince;
                DalAccess.AddDrone(tempDrone);
                DronesListBL.Add(droneBL);
            }
            catch (IDAL.DO.AlreadyExistException)
            {
                throw ;
            }
        }

        public void AddCustomer(int myId, string myName, string myPhone, Location myCustLocation)
        {
            try
            {
                Customer tempCust = new();
                tempCust.Id = myId;
                tempCust.Name = myName;
                tempCust.Phone = myPhone;
                tempCust.CustomerLongitude = myCustLocation.Longitude;
                tempCust.CustomerLatitude = myCustLocation.Latitude;
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
                Parcel tempParcel = new();
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
                //ParcelBL parcelBL = new();
                //parcelBL.DroneAssignToParcel = null;

            }
            catch (IDAL.DO.AlreadyExistException)
            {

                throw;
            }
        }


    }
}
