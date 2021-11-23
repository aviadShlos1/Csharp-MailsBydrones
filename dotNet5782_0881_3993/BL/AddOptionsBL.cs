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
            newDroneBl.BatteryPercent = (rand.NextDouble() * 20) + 20;
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
            if (DalAccess.GetSingleBaseStation(firstChargeStation).FreeChargeSlots <= 0)
                throw new NoStationsWithFreeChargeException();
        }
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
        public void AddParcel(ParcelBl newParcel)
        {
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
