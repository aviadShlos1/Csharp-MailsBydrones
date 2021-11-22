﻿using System;
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
                BaseStationDal tempBase = new();
                tempBase.Id = myId;
                tempBase.Name = myBaseStationName;
                tempBase.Latitude = myBaseStationLocation.Latitude;
                tempBase.Longitude = myBaseStationLocation.Longitude;
                tempBase.FreeChargeSlots = myFreeChargeSlots;
                DalAccess.AddStation(tempBase);
                BaseStationBl myStationBl = new();
                myStationBl.DronesInChargeList = null;
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
                DroneDal tempDrone = new();
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
