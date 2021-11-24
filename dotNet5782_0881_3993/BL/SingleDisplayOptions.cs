﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IBL.BO;
namespace IBL
{
    //This class implements the single display methods of the business layer
    partial class BL
    {
        /// <summary>
        /// Getting the details of a single base station
        /// </summary>
        /// <param name="baseStationId"> The wanted object to display</param>
        /// <returns> Base station bl object</returns>
        public BaseStationBl GetSingleBaseStation(int baseStationId)
        {
            IDAL.DO.BaseStationDal dalBaseStation = new();
            try
            {
                dalBaseStation = DalAccess.GetSingleBaseStation(baseStationId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }

            Location myLocation = new() { Latitude = dalBaseStation.Latitude, Longitude = dalBaseStation.Longitude };
            BaseStationBl myStationBl = new() { Id = dalBaseStation.Id, BaseStationName = dalBaseStation.Name, Location = myLocation, FreeChargeSlots = dalBaseStation.FreeChargeSlots, DronesInChargeList = new() };

            var dronesInChargePerStation = DalAccess.GetDronesChargeList().TakeWhile(x => x.StationId == baseStationId).ToList();
            foreach (var item in dronesInChargePerStation)
            {
                var tempDrone = DronesListBL.Find(x => x.DroneId == item.DroneId);
                myStationBl.DronesInChargeList.Add(new DroneInCharge { Id = tempDrone.DroneId, BatteryPercent = tempDrone.BatteryPercent });
            }
            return myStationBl;
        }

        /// <summary>
        /// Getting the details of a single drone 
        /// </summary>
        /// <param name="myDroneId"> The wanted object to display</param>
        /// <returns> drone bl object</returns>
        public DroneBl GetSingleDrone(int myDroneId)
        {
            IDAL.DO.DroneDal dalDrone = new();
            try
            {
                dalDrone = DalAccess.GetSingleDrone(myDroneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }

            var tempDroneBl = DronesListBL.Find(x => x.DroneId == myDroneId);
            DroneBl myDroneBl = new() { DroneId = dalDrone.Id, Model = dalDrone.Model, DroneWeight = (WeightCategoriesBL)dalDrone.DroneWeight, BatteryPercent = tempDroneBl.BatteryPercent, DroneStatus = tempDroneBl.DroneStatus, DroneLocation = tempDroneBl.DroneLocation };
            if (myDroneBl.DroneStatus == DroneStatus.Shipment)
            {
                var tempParcel= DalAccess.GetParcelsList().ToList().Find(x => x.DroneToParcelId == myDroneId);
                myDroneBl.ParcelInShip.Id =tempParcel.Id ;
                myDroneBl.ParcelInShip.Weight = (WeightCategoriesBL)tempParcel.Weight;
                myDroneBl.ParcelInShip.Priority = (PrioritiesBL)tempParcel.Priority;
                myDroneBl.ParcelInShip.Sender.Id = tempParcel.SenderId;
                myDroneBl.ParcelInShip.Sender.Name = GetCustomerDetails(tempParcel.SenderId).Name;
                myDroneBl.ParcelInShip.Reciever.Id = tempParcel.TargetId;
                myDroneBl.ParcelInShip.Reciever.Name = GetCustomerDetails(tempParcel.TargetId).Name;
                Location myPickUpLocation = new Location() { Longitude = GetCustomerDetails(tempParcel.SenderId).CustomerLongitude, Latitude = GetCustomerDetails(tempParcel.SenderId).CustomerLatitude };
                myDroneBl.ParcelInShip.PickUpLocation = myPickUpLocation;
                Location myTargetLocation = new Location() { Longitude = GetCustomerDetails(tempParcel.TargetId).CustomerLongitude, Latitude = GetCustomerDetails(tempParcel.TargetId).CustomerLatitude };
                myDroneBl.ParcelInShip.TargetLocation = myTargetLocation;
                myDroneBl.ParcelInShip.ShippingDistance = GetDistance(myPickUpLocation.Longitude, myPickUpLocation.Latitude, myTargetLocation.Longitude, myTargetLocation.Latitude);
                
                if (tempParcel.PickingUpTime != DateTime.MinValue) //if the parcel has already picked up
                    myDroneBl.ParcelInShip.ShippingOnTheWay = true;
                else
                    myDroneBl.ParcelInShip.ShippingOnTheWay = false;
            }
            return myDroneBl;
        }

        /// <summary>
        /// Getting the details of a single customer
        /// </summary>
        /// <param name="customerId"> The wanted object to display</param>
        /// <returns> customer bl object</returns>
        public CustomerBL GetSingleCustomer(int customerId)
        {
            IDAL.DO.CustomerDal myCustomer = new();
            try
            {
                myCustomer = DalAccess.GetSingleCustomer(customerId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }

            Location myLocation = new() { Latitude = myCustomer.CustomerLatitude, Longitude = myCustomer.CustomerLongitude };
            CustomerBL myCustomerBl = new() { CustomerId = myCustomer.Id, CustomerName = myCustomer.Name, CustomerPhone = myCustomer.Phone, CustomerLocation = myLocation, ParcelsFromCustomerList = new(), ParcelsToCustomerList = new() };
            List<IDAL.DO.ParcelDal> mySentParcels = DalAccess.GetParcelsList().TakeWhile(x => x.SenderId == customerId).ToList();
            List<IDAL.DO.ParcelDal> myRecievedParcels = DalAccess.GetParcelsList().TakeWhile(x => x.TargetId == customerId).ToList();
            // getting the all parcels which were sent by the customer 
            foreach (var senderItem in mySentParcels)
            {
                ParcelByCustomer myParcelByCustomer = new ParcelByCustomer()
                {
                    Id = senderItem.Id,
                    Priority = (PrioritiesBL)senderItem.Priority,
                    Weight = (WeightCategoriesBL)senderItem.Weight,
                    SourceOrTargetMan = new AssignCustomerToParcel { Id = senderItem.Id, Name = GetCustomerDetails(senderItem.SenderId).Name }
                };
                if (senderItem.AssignningTime != DateTime.MinValue)
                    myParcelByCustomer.Status = ParcelStatus.Assigned;
                if (senderItem.CreatingTime != DateTime.MinValue)
                    myParcelByCustomer.Status = ParcelStatus.Created;
                if (senderItem.PickingUpTime != DateTime.MinValue)
                    myParcelByCustomer.Status = ParcelStatus.PickedUp;
                if (senderItem.SupplyingTime != DateTime.MinValue)
                    myParcelByCustomer.Status = ParcelStatus.Supplied;
                myCustomerBl.ParcelsFromCustomerList.Add(myParcelByCustomer);
            }
            // getting the all parcels which were recieved by the customer 
            foreach (var TargetItem in myRecievedParcels)
            {
                ParcelByCustomer targetParcelByCustomer = new ParcelByCustomer()
                {
                    Id = TargetItem.Id,
                    Priority = (PrioritiesBL)TargetItem.Priority,
                    Weight = (WeightCategoriesBL)TargetItem.Weight,
                    SourceOrTargetMan = new AssignCustomerToParcel { Id = TargetItem.Id, Name = GetCustomerDetails(TargetItem.TargetId).Name }
                };
                if (TargetItem.AssignningTime != DateTime.MinValue)
                    targetParcelByCustomer.Status = ParcelStatus.Assigned;
                if (TargetItem.CreatingTime != DateTime.MinValue)
                    targetParcelByCustomer.Status = ParcelStatus.Created;
                if (TargetItem.PickingUpTime != DateTime.MinValue)
                    targetParcelByCustomer.Status = ParcelStatus.PickedUp;
                if (TargetItem.SupplyingTime != DateTime.MinValue)
                    targetParcelByCustomer.Status = ParcelStatus.Supplied;
                myCustomerBl.ParcelsToCustomerList.Add(targetParcelByCustomer);

            }
            return myCustomerBl;
        }

        /// <summary>
        /// Getting the details of a single parcel
        /// </summary>
        /// <param name="parcelId"> The wanted object to display</param>
        /// <returns> parcel bl object</returns>
        public ParcelBl GetSingleParcel(int parcelId)
        {
            IDAL.DO.ParcelDal dalParcel = new();
            try
            {
                dalParcel = DalAccess.GetSingleParcel(parcelId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }

            AssignCustomerToParcel senderItem = new AssignCustomerToParcel { Id = dalParcel.SenderId, Name = GetCustomerDetails(dalParcel.SenderId).Name };
            AssignCustomerToParcel recieverItem = new AssignCustomerToParcel { Id = dalParcel.TargetId, Name = GetCustomerDetails(dalParcel.TargetId).Name };
            ParcelBl myParcelBl = new() { ParcelId = dalParcel.Id, ParcelWeight=(WeightCategoriesBL)dalParcel.Weight, Priority=(PrioritiesBL)dalParcel.Priority, CreatingTime=dalParcel.CreatingTime, AssignningTime=dalParcel.AssignningTime, PickingUpTime=dalParcel.PickingUpTime, SupplyingTime=dalParcel.SupplyingTime, Sender=senderItem, Reciever=recieverItem};
            if (myParcelBl.AssignningTime!=DateTime.MinValue)
            {
                var tempDrone = DronesListBL.Find(x => x.DroneId == dalParcel.DroneToParcelId);
                DroneInShipment droneInShipmentItem = new DroneInShipment { DroneId = tempDrone.DroneId, BatteryPercent = tempDrone.BatteryPercent, DroneInShipLocation = tempDrone.DroneLocation };
                myParcelBl.DroneAssignToParcel = droneInShipmentItem;
            }
            return myParcelBl;
        }
    }
}
