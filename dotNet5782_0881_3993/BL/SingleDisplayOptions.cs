//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using BO;
namespace BlApi
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
            DO.BaseStationDal dalBaseStation = new();
            try
            {
                dalBaseStation = DalAccess.GetSingleBaseStation(baseStationId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }

            Location myLocation = new() { Latitude = dalBaseStation.Latitude, Longitude = dalBaseStation.Longitude };
            BaseStationBl myStationBl = new() { Id = dalBaseStation.Id, BaseStationName = dalBaseStation.Name, Location = myLocation, FreeChargeSlots = dalBaseStation.FreeChargeSlots, DronesInChargeList = new() };

            var dronesInChargePerStation = DalAccess.GetDronesChargeList().Where(x => x.StationId == baseStationId).ToList();
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
            var tempDroneBl = DronesListBL.Find(x => x.DroneId == myDroneId); //type of "DroneToList"
            if (tempDroneBl ==default)
            {
                throw new BO.NotExistException();
            }
            DroneBl droneToDisplay = new() { DroneId = tempDroneBl.DroneId, Model = tempDroneBl.Model, DroneWeight = tempDroneBl.DroneWeight, BatteryPercent = tempDroneBl.BatteryPercent, DroneStatus = tempDroneBl.DroneStatus, DroneLocation = tempDroneBl.DroneLocation, ParcelInShip = new() };
            if (tempDroneBl.DroneStatus == DroneStatusesBL.Shipment)
            {
                var tempParcel = DalAccess.GetParcelsList().ToList().Find(x => x.DroneToParcelId == myDroneId);
                AssignCustomerToParcel mySender = new() { CustId = tempParcel.SenderId, CustName = GetCustomerDetails(tempParcel.SenderId).Name };
                AssignCustomerToParcel myReciever = new() { CustId = tempParcel.TargetId, CustName = GetCustomerDetails(tempParcel.TargetId).Name };
                Location myPickUpLocation = new() { Longitude = GetCustomerDetails(tempParcel.SenderId).Longitude, Latitude = GetCustomerDetails(tempParcel.SenderId).Latitude };
                Location myTargetLocation = new() { Longitude = GetCustomerDetails(tempParcel.TargetId).Longitude, Latitude = GetCustomerDetails(tempParcel.TargetId).Latitude };
                bool isOnTheSupplyWay=default;
                if (tempParcel.PickingUpTime != null) //if the parcel has already picked up
                    isOnTheSupplyWay = true;
                else
                    isOnTheSupplyWay = false;

                droneToDisplay.ParcelInShip.Id = tempParcel.Id;
                droneToDisplay.ParcelInShip.Weight = (WeightCategoriesBL)tempParcel.Weight;
                droneToDisplay.ParcelInShip.Priority = (PrioritiesBL)tempParcel.Priority;
                droneToDisplay.ParcelInShip.Sender = mySender;
                droneToDisplay.ParcelInShip.Reciever = myReciever;
                droneToDisplay.ParcelInShip.PickUpLocation = myPickUpLocation;
                droneToDisplay.ParcelInShip.TargetLocation = myTargetLocation;
                droneToDisplay.ParcelInShip.ShippingDistance = GetDistance(myPickUpLocation.Longitude, myPickUpLocation.Latitude, myTargetLocation.Longitude, myTargetLocation.Latitude);
                droneToDisplay.ParcelInShip.ShippingOnTheSupplyWay = isOnTheSupplyWay;               
            }         
            return droneToDisplay;
        }

        /// <summary>
        /// Getting the details of a single customer
        /// </summary>
        /// <param name="customerId"> The wanted object to display</param>
        /// <returns> customer bl object</returns>
        public CustomerBL GetSingleCustomer(int customerId)
        {
            DO.CustomerDal myCustomer = new();
            try
            {
                myCustomer = DalAccess.GetSingleCustomer(customerId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }

            Location myLocation = new() { Latitude = myCustomer.Latitude, Longitude = myCustomer.Longitude };
            CustomerBL myCustomerBl = new() { Id = myCustomer.Id, Name = myCustomer.Name, Phone = myCustomer.Phone, Location = myLocation, ParcelsFromCustomerList = new(), ParcelsToCustomerList = new() };
            List<DO.ParcelDal> mySentParcels = DalAccess.GetParcelsList().Where(x => x.SenderId == customerId).ToList();
            List<DO.ParcelDal> myRecievedParcels = DalAccess.GetParcelsList().Where(x => x.TargetId == customerId).ToList();
            // getting the all parcels which were sent by the customer 
            foreach (var senderItem in mySentParcels)
            {
                ParcelByCustomer myParcelByCustomer = new ParcelByCustomer()
                {
                    Id = senderItem.Id,
                    Priority = (PrioritiesBL)senderItem.Priority,
                    Weight = (WeightCategoriesBL)senderItem.Weight,
                    SourceOrTargetMan = new AssignCustomerToParcel { CustId = senderItem.TargetId, CustName = GetCustomerDetails(senderItem.TargetId).Name }
                };
                if (senderItem.AssignningTime != null)
                    myParcelByCustomer.Status = ParcelStatus.Assigned;
                if (senderItem.CreatingTime != null)
                    myParcelByCustomer.Status = ParcelStatus.Created;
                if (senderItem.PickingUpTime != null)
                    myParcelByCustomer.Status = ParcelStatus.PickedUp;
                if (senderItem.SupplyingTime != null)
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
                    SourceOrTargetMan = new AssignCustomerToParcel { CustId = TargetItem.SenderId, CustName = GetCustomerDetails(TargetItem.SenderId).Name }
                };
                if (TargetItem.AssignningTime != null)
                    targetParcelByCustomer.Status = ParcelStatus.Assigned;
                if (TargetItem.CreatingTime != null)
                    targetParcelByCustomer.Status = ParcelStatus.Created;
                if (TargetItem.PickingUpTime != null)
                    targetParcelByCustomer.Status = ParcelStatus.PickedUp;
                if (TargetItem.SupplyingTime != null)
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
            DO.ParcelDal dalParcel = new();
            try
            {
                dalParcel = DalAccess.GetSingleParcel(parcelId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }

            AssignCustomerToParcel senderItem = new AssignCustomerToParcel { CustId = dalParcel.SenderId, CustName = GetCustomerDetails(dalParcel.SenderId).Name };
            AssignCustomerToParcel recieverItem = new AssignCustomerToParcel { CustId = dalParcel.TargetId, CustName = GetCustomerDetails(dalParcel.TargetId).Name };
            ParcelBl myParcelBl = new() { ParcelId = dalParcel.Id, ParcelWeight = (WeightCategoriesBL)dalParcel.Weight, Priority = (PrioritiesBL)dalParcel.Priority, CreatingTime = dalParcel.CreatingTime, AssignningTime = dalParcel.AssignningTime, PickingUpTime = dalParcel.PickingUpTime, SupplyingTime = dalParcel.SupplyingTime, Sender = senderItem, Reciever = recieverItem };
            if (myParcelBl.AssignningTime != null)
            {
                var tempDrone = DronesListBL.Find(x => x.DroneId == dalParcel.DroneToParcelId);
                DroneInShipment droneInShipmentItem = new DroneInShipment { DroneId = tempDrone.DroneId, BatteryPercent = tempDrone.BatteryPercent, DroneInShipLocation = tempDrone.DroneLocation };
                myParcelBl.DroneAssignToParcel = droneInShipmentItem;
            }
            return myParcelBl;
        }
    }
}
