//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;

namespace IBL
{
    /// <summary>
    /// This class imlements the add methods of the buisneess layer
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
            catch (IDAL.DO.AlreadyExistException)
            {
                throw new BO.AlreadyExistException();
            }
        }
        /// <summary>
        /// Adding a new Bl drone
        /// </summary>
        /// <param name="newDroneBl">The entity for adding</param>
        public void AddDrone(DroneToList newDroneBl, int firstChargeStation)
        {
            
            DroneDal tempDrone = new();
            tempDrone.Id = newDroneBl.DroneId;
            tempDrone.Model = newDroneBl.Model;
            tempDrone.DroneWeight = (WeightCategoriesDal)newDroneBl.DroneWeight;
            int existIndex = DalAccess.GetBaseStationsList().ToList().FindIndex(x => x.Id == firstChargeStation);
            if (existIndex == -1)
                throw new BO.NotExistException();
            foreach (var item in DalAccess.GetBaseStationsList())
            {
                if (item.Id == firstChargeStation)
                {
                    Location location = new() { Longitude = item.Longitude, Latitude = item.Latitude };
                    newDroneBl.DroneLocation = location;                  
                }
            }
            newDroneBl.BatteryPercent = (rand.NextDouble() * 20) + 20;//rand between 20-40 percent
            newDroneBl.DroneStatus = DroneStatus.Maintaince;
            try
            {
                DalAccess.AddDrone(tempDrone);
                DronesListBL.Add(newDroneBl);
            }
            catch (IDAL.DO.AlreadyExistException)
            {
                throw new BO.AlreadyExistException();
            }
            if (DalAccess.GetSingleBaseStation(firstChargeStation).FreeChargeSlots <= 0)//Checks if the station doesn't have free charge slots
                throw new NoStationsWithFreeChargeException();
        }
        /// <summary>
        /// Adding a new Bl customer
        /// </summary>
        /// <param name="newCustomer">The entity for adding</param>
        public void AddCustomer(CustomerBL newCustomer)
        {
            
            CustomerDal tempCust = new();
            tempCust.Id = newCustomer.CustomerId;
            tempCust.Name = newCustomer.CustomerName;
            tempCust.Phone = newCustomer.CustomerPhone;
            tempCust.CustomerLongitude = newCustomer.CustomerLocation.Longitude;
            tempCust.CustomerLatitude = newCustomer.CustomerLocation.Latitude;
            try
            {
                DalAccess.AddCustomer(tempCust);
            }
            catch (IDAL.DO.AlreadyExistException)
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
                DalAccess.GetSingleCustomer(newParcel.Sender.Id);
                DalAccess.GetSingleCustomer(newParcel.Reciever.Id);
            }
            catch (BO.NotExistException)
            {
                throw new BO.NotExistException("Not exist customer id");
            }
            ParcelDal tempParcel = new();
            tempParcel.SenderId = newParcel.Sender.Id;
            tempParcel.TargetId = newParcel.Reciever.Id;
            tempParcel.Weight = (WeightCategoriesDal)newParcel.ParcelWeight;
            tempParcel.Priority = (Priorities)newParcel.Priority;
            tempParcel.CreatingTime = DateTime.Now;
            tempParcel.DroneToParcelId = 0;
            DalAccess.AddParcel(tempParcel);
        }
    }
}
