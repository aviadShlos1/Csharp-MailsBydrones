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
        public void AddBaseStation(int myId, string myBaseStationName, Location myBaseStationLocation, int myFreeChargeSlots = 0)
        {
            BaseStation tempBase = new();
            tempBase.Id = myId;
            tempBase.Name = myBaseStationName;
            tempBase.Latitude = myBaseStationLocation.Latitude;
            tempBase.Longitude = myBaseStationLocation.Longitude;
            tempBase.FreeChargeSlots = myFreeChargeSlots;
            DalAccess.AddStation(tempBase);
            List<DroneInCharge> DronesInChargeList = null;
        }
        public void AddDrone(int myDroneId, string myModel, WeightCategoriesBL myDroneWeight, int myBaseStationId)
        {
            DroneToList droneBL = new();
            Drone tempDrone = new();
            tempDrone.Id = myDroneId;
            tempDrone.Model = myModel;
            tempDrone.DroneWeight = (WeightCategoriesDal)myDroneWeight;
            foreach (var item in DalAccess.BaseStationsListDisplay())
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

        public void AddCustomer(int myId, string myName, string myPhone, Location myCustLocation)
        {
            Customer tempCust = new();
            tempCust.Id = myId;
            tempCust.Name = myName;
            tempCust.Phone = myPhone;
            tempCust.CustomerLongitude = myCustLocation.Longitude;
            tempCust.CustomerLatitude = myCustLocation.Latitude;
            DalAccess.AddCustomer(tempCust);
        }
        public void AddParcel(int mySenderId, int myRecieverId, WeightCategoriesBL myParcelWeight, PrioritiesBL myPriority)
        {
            Parcel tempParcel = new();
            tempParcel.SenderId = mySenderId;
            tempParcel.TargetId = myRecieverId;
            tempParcel.Weight = (WeightCategoriesDal)myParcelWeight;
            tempParcel.Priority = (Priorities)myPriority;
            tempParcel.Created = DateTime.Now;
            tempParcel.Assigned = DateTime.MinValue;
            tempParcel.PickedUp = DateTime.MinValue;
            tempParcel.Supplied = DateTime.MinValue;
            tempParcel.DroneToParcel_Id = 0;
            DalAccess.AddParcel(tempParcel);
            //ParcelBL parcelBL = new();
            //parcelBL.DroneAssignToParcel = null;
        }


    }
}
