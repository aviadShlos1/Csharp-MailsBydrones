﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
using DO;

namespace BL
{
    /// <summary>
    /// This class implements the add methods of the buisness layer
    /// </summary>
    partial class BL
    {
        /// <summary>
        /// Adding a new Bl base station
        /// </summary>
        /// <param name="newBaseStationBl">The entity for adding</param>
        public void AddBaseStation(BaseStationBl newBaseStationBl)
        {
            
            BaseStationDal tempBase = new();
            tempBase.Id = newBaseStationBl.Id;
            tempBase.Name = newBaseStationBl.BaseStationName;
            tempBase.Latitude = newBaseStationBl.Location.Latitude;
            tempBase.Longitude = newBaseStationBl.Location.Longitude;
            tempBase.FreeChargeSlots = newBaseStationBl.FreeChargeSlots;
            try
            {
                DalAccess.AddStation(tempBase);
            }
            catch (DO.AlreadyExistException)
            {
                throw new BO.AlreadyExistException();
            }
        }
        /// <summary>
        /// Adding a new Bl drone
        /// </summary>
        /// <param name="newDroneBl">The entity for adding</param>

        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void AddDrone(DroneToList newDroneBl , int firstChargeStation)
        {
            
            DroneDal tempDrone = new();
            tempDrone.Id = newDroneBl.DroneId;
            tempDrone.Model = newDroneBl.Model;
            tempDrone.DroneWeight = (WeightCategoriesDal)newDroneBl.DroneWeight;
            int existIndex = DalAccess.GetBaseStationsList().ToList().FindIndex(x => x.Id == firstChargeStation);
            if (existIndex == -1)
                throw new BO.NotExistException();
            Location location = new Location()
            {
                Longitude = DalAccess.GetSingleBaseStation(firstChargeStation).Longitude,
                Latitude = DalAccess.GetSingleBaseStation(firstChargeStation).Latitude
            };
            newDroneBl.DroneLocation = location;
            newDroneBl.BatteryPercent = rand.Next(20, 41) ;//rand between 20-40 percent
            newDroneBl.DroneStatus = DroneStatusesBL.Maintaince;
            try
            {
                DalAccess.AddDrone(tempDrone);
                DronesListBL.Add(newDroneBl);
            }
            catch (DO.AlreadyExistException)
            {
                throw new BO.AlreadyExistException();
            }
            if (DalAccess.GetSingleBaseStation(firstChargeStation).FreeChargeSlots == 0)//Checks if the station doesn't have free charge slots
                throw new NoStationsWithFreeChargeException();
        }
        /// <summary>
        /// Adding a new Bl customer
        /// </summary>
        /// <param name="newCustomer">The entity for adding</param>
        public void AddCustomer(CustomerBL newCustomer)
        {
            
            CustomerDal tempCust = new();
            tempCust.Id = newCustomer.Id;
            tempCust.Name = newCustomer.Name;
            tempCust.Phone = newCustomer.Phone;
            tempCust.Longitude = newCustomer.Location.Longitude;
            tempCust.Latitude = newCustomer.Location.Latitude;
            try
            {
                DalAccess.AddCustomer(tempCust);
            }
            catch (DO.AlreadyExistException)
            {
                throw new BO.AlreadyExistException();
            }
        }
        /// <summary>
        /// Adding a new Bl parcel
        /// </summary>
        /// <param name="newParcel">The entity for adding</param>
        public void AddParcel(ParcelBl newParcel)
        {
            try//Checks if its have a customer assigned to the parcel 
            {
                DalAccess.GetSingleCustomer(newParcel.Sender.CustId);
                DalAccess.GetSingleCustomer(newParcel.Reciever.CustId);
            }
            catch (BO.NotExistException)
            {
                throw new BO.NotExistException("Not exist customer id");
            }
            ParcelDal tempParcel = new();
            tempParcel.SenderId = newParcel.Sender.CustId;
            tempParcel.TargetId = newParcel.Reciever.CustId;
            tempParcel.Weight = (WeightCategoriesDal)newParcel.ParcelWeight;
            tempParcel.Priority = (Priorities)newParcel.Priority;
            tempParcel.CreatingTime = DateTime.Now;
            tempParcel.AssignningTime = null;
            tempParcel.PickingUpTime = null;
            tempParcel.SupplyingTime = null;
            tempParcel.DroneToParcelId = 0;
            DalAccess.AddParcel(tempParcel);
        }
    }
}
