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
namespace IBL
{
    /// <summary>
    /// This class implements the update methods of the business layer
    /// </summary>
    partial class BL
    {
        /// <summary>
        /// Updating a drone model name
        /// </summary>
        /// <param name="droneId">The id of the exist drone</param>
        /// <param name="newModel">The new model name</param>
        public void UpdateDroneName(int droneId, string newModel)
        {
            IDAL.DO.DroneDal myDrone = DalAccess.GetDronesList().ToList().Find(x => x.Id == droneId);
            if (newModel!="")
            {
                myDrone.Model = newModel;
            }
            DalAccess.UpdateDrone(myDrone);
        }
       
        /// <summary>
        /// Updating base station name and update the free charge slots number 
        /// </summary>
        /// <param name="baseStationId"> The id of the exist base station</param>
        /// <param name="newName">a new name</param>
        /// <param name="totalChargeSlots">a new total charge slots number</param>
        public void UpdateBaseStationData(int baseStationId, string newName, int totalChargeSlots)
        {
            IDAL.DO.BaseStationDal myBaseStation = DalAccess.GetBaseStationsList().ToList().Find(x => x.Id == baseStationId);
            if (newName!="")
            {
                 myBaseStation.Name = newName;
            }
            foreach (var item in DalAccess.GetBaseStationsList())
            {
                if (item.Id == baseStationId)
                {
                    if (totalChargeSlots != 0)
                    {
                        int dronesInCharge = 0;
                        foreach (var item2 in DronesListBL)
                        {
                            if (item2.DroneStatus == BO.DroneStatusesBL.Maintaince)
                                dronesInCharge++;
                        }
                        if (dronesInCharge > totalChargeSlots)
                            throw new BO.NotEnoughChargeSlotsInThisStation(baseStationId);
                       int free = totalChargeSlots - dronesInCharge;
                        myBaseStation.FreeChargeSlots = free;
                    }
                }
            }
            DalAccess.UpdateBaseStation(myBaseStation);
        }
        
        /// <summary>
        /// Updating customer details: new name and phone
        /// </summary>
        /// <param name="myId">The id of the exist customer</param>
        /// <param name="newName">a new name</param>
        /// <param name="newPhone">a new phone</param>
        public void UpdateCustomerData(int myId, string newName, string newPhone)
        {
            IDAL.DO.CustomerDal myCustomer = DalAccess.GetCustomersList().ToList().Find(x => x.Id == myId);
            if (newName != "")
            {
                myCustomer.Name = newName;
            }
            if (newPhone != "")
            {
                myCustomer.Phone = newPhone;
            }
            DalAccess.UpdateCustomer(myCustomer);
        }
        
        /// <summary>
        /// Assigning a parcel to a drone 
        /// </summary>
        /// <param name="myDroneId">an exist drone</param>
        public void AssignParcelToDrone(int myDroneId)
        {
            IDAL.DO.ParcelDal assignedParcel = new();
            IDAL.DO.CustomerDal senderCustomer = new();
            double closetDistance = default;

            // finding the high priority parcel, taking in conclusion the priority,weight and distance
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == myDroneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            if (droneItem.DroneStatus != BO.DroneStatusesBL.Available)
            {
                throw new BO.DroneIsNotAvailable(myDroneId);
            }

            senderCustomer = GetCustomerDetails(HighestPriorityAndWeightParcels()[0].SenderId);
            /// Finding the closet parcel among the highest priority and weight parcels list
            closetDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude);
            foreach (var item in HighestPriorityAndWeightParcels())
            {
                senderCustomer = GetCustomerDetails(item.SenderId);
                double tempDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude);
                if (tempDistance <= closetDistance)
                {
                    closetDistance = tempDistance;
                    assignedParcel = item;
                }
            }
            //checking the battery consumption
            double arriveToSenderBattery = closetDistance * freeWeightConsumption;

            IDAL.DO.CustomerDal targetCustomer = GetCustomerDetails(assignedParcel.TargetId);
            double targetDistance = GetDistance(senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude, targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude);
            double srcToTrgetBattery = targetDistance * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1];

            BO.BaseStationBl closetStationFromTarget = ClosetStation(targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude, DalAccess.GetBaseStationsList().ToList());
            double targetToCharge = GetDistance(targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude, closetStationFromTarget.Location.Longitude, closetStationFromTarget.Location.Latitude);
            double trgetToChargeBattery = targetToCharge * freeWeightConsumption;
            double totalDemandBattery = arriveToSenderBattery + srcToTrgetBattery + trgetToChargeBattery;

            if (droneItem.BatteryPercent < totalDemandBattery) //if the drone will not be able to do the shipment
            {
                throw new BO.CannotAssignDroneToParcelException(myDroneId);
            }
            else
            {
                droneItem.DroneStatus = BO.DroneStatusesBL.Shipment;
                droneItem.Delivery = assignedParcel.Id;
                assignedParcel.DroneToParcelId = myDroneId;
                assignedParcel.AssignningTime = DateTime.Now;

                DalAccess.AssignParcelToDrone(assignedParcel.Id, myDroneId);
            }

        }
        #region Help methods for AssignParcelToDrone method
        /// <summary>
        /// Auxiliary method: Searching the highest priority parcels, according to the priority field
        /// </summary>
        /// <returns>The highest priority parcels list</returns>
        private List<IDAL.DO.ParcelDal> HighestPriorityParcels()
        {
            List<IDAL.DO.ParcelDal> parcelsWithUrgentPriority = new();
            List<IDAL.DO.ParcelDal> parcelsWithFastPriority = new();
            List<IDAL.DO.ParcelDal> parcelsWithNormalPriority = new();

            foreach (var item in DalAccess.GetParcelsList())/////////////////!!!!!!!!!!!!!!!!!!add condition
            {
                switch ((PrioritiesBL)item.Priority)
                {
                    case PrioritiesBL.Normal:
                        parcelsWithNormalPriority.Add(item);
                        break;
                    case PrioritiesBL.Fast:
                        parcelsWithFastPriority.Add(item);
                        break;
                    case PrioritiesBL.Urgent:
                        parcelsWithUrgentPriority.Add(item);
                        break;
                    default:
                        break;
                }
            }
            // checking the highest exist priority and return it 
            return (parcelsWithUrgentPriority.Any() ? parcelsWithUrgentPriority :
                parcelsWithFastPriority.Any() ? parcelsWithFastPriority
                : parcelsWithNormalPriority);
        }
        /// <summary>
        /// Auxiliary method: Searching the highest priority and weight parcels based on the highest priority parcels list
        /// </summary>
        /// <returns>The highest priority and weight parcels list</returns>
        private List<IDAL.DO.ParcelDal> HighestPriorityAndWeightParcels()
        {
            List<IDAL.DO.ParcelDal> heavyParcels = new();
            List<IDAL.DO.ParcelDal> mediumParcels = new();
            List<IDAL.DO.ParcelDal> lightParcels = new();

            foreach (var item in HighestPriorityParcels())
            {
                switch ((WeightCategoriesBL)item.Weight)
                {
                    case WeightCategoriesBL.Light:
                        lightParcels.Add(item);
                        break;
                    case WeightCategoriesBL.Medium:
                        mediumParcels.Add(item);
                        break;
                    case WeightCategoriesBL.Heavy:
                        heavyParcels.Add(item);
                        break;
                    default:
                        break;
                }
            }
            // checking the highest exist weight and return it 
            return (heavyParcels.Any() ? heavyParcels : mediumParcels.Any() ?
                mediumParcels : lightParcels);
        }
        #endregion

        /// <summary>
        /// Updating the parcel pick up details
        /// </summary>
        /// <param name="droneId"> an exist drone</param>
        public void PickUpParcel(int droneId)
        {
            IDAL.DO.ParcelDal parcelItem = new();
            IDAL.DO.CustomerDal senderItem = new();
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            if (droneItem.Delivery == 0) // if the drone doesn't take any parcel
                throw new BO.CannotPickUpException("The drone has not transfered parcels yet");
            parcelItem = DalAccess.GetSingleParcel(droneItem.Delivery);
            senderItem = GetCustomerDetails(parcelItem.SenderId);

            if (parcelItem.PickingUpTime != null) //checking if the parcel has already picked up
                throw new BO.CannotPickUpException("The parcel has already picked up");

            else //updating the battery,location and picking up time
            {
                double currentToSender = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderItem.CustomerLongitude, senderItem.CustomerLatitude);
                droneItem.BatteryPercent -= currentToSender * freeWeightConsumption;
                droneItem.DroneLocation.Longitude = senderItem.CustomerLongitude;
                droneItem.DroneLocation.Latitude = senderItem.CustomerLatitude;
                parcelItem.PickingUpTime = DateTime.Now;
                DalAccess.PickUpParcel(droneItem.DroneId);
            }
        }
        
        /// <summary>
        /// Updating the parcel supply details
        /// </summary>
        /// <param name="droneId"> an exist drone</param>
        public void SupplyParcel(int droneId)
        {
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            if (droneItem.Delivery == 0) //if the drone doesn't take any parcel
                throw new BO.CannotSupplyException("The drone has not transfered parcels yet");
            
            var parcelItem = DalAccess.GetSingleParcel(droneItem.Delivery);
            var targetItem = GetCustomerDetails(parcelItem.TargetId);
            if (parcelItem.PickingUpTime == null)
                throw new BO.CannotSupplyException("The parcel has not picked up yet");
            if (parcelItem.SupplyingTime != null)
                throw new BO.CannotSupplyException("The parcel has already supplied");
           
            else //updating the battery,location, status and suppling time
            {
                double currentToTarget = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, targetItem.CustomerLongitude, targetItem.CustomerLatitude);
                droneItem.BatteryPercent -= currentToTarget * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1];
                droneItem.DroneLocation.Longitude = targetItem.CustomerLongitude;
                droneItem.DroneLocation.Latitude = targetItem.CustomerLatitude;
                droneItem.Delivery = 0; // initialize the id of the transfer parcel, in that we will know that the drone will be available for a new mission
                droneItem.DroneStatus = BO.DroneStatusesBL.Available;
                parcelItem.SupplyingTime = DateTime.Now;
                DalAccess.SupplyParcel(droneItem.DroneId);
            }

        }

        /// <summary>
        /// Sending drone for charging in order to fill its battery
        /// </summary>
        /// <param name="droneId"></param>
        public void DroneToCharge(int droneId)
        {
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            
            List<IDAL.DO.BaseStationDal> freeChargeSlotsStations = DalAccess.GetBaseStationsList().ToList(); /////////////////!!!!!!!!!!!!!!!!!!add condition
            if ((droneItem.DroneStatus != BO.DroneStatusesBL.Available)) //if the drone is not available (maintaince or shipment)
                throw new DroneIsNotAvailable(droneId);
            if (freeChargeSlotsStations.Count == 0 )
                throw new BO.CannotGoToChargeException(droneId);
            else
            {
                BO.BaseStationBl closetBaseStation = new();
                closetBaseStation = ClosetStation(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, freeChargeSlotsStations);
                double closetDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, closetBaseStation.Location.Longitude, closetBaseStation.Location.Latitude);
             
                if (droneItem.BatteryPercent >= closetDistance * freeWeightConsumption)
                {
                    droneItem.BatteryPercent -= closetDistance * freeWeightConsumption;
                    droneItem.DroneLocation.Longitude = closetBaseStation.Location.Longitude;
                    droneItem.DroneLocation.Latitude = closetBaseStation.Location.Latitude;
                    droneItem.DroneStatus = BO.DroneStatusesBL.Maintaince;
                    closetBaseStation.FreeChargeSlots--;
                    DalAccess.DroneToCharge(droneItem.DroneId, closetBaseStation.Id);
                }
                else
                {
                    throw new BO.CannotGoToChargeException(droneId);
                }
            }

        }
        
        /// <summary>
        /// Releasing a drone from charging
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="chargeTime"></param>
        public void ReleaseDroneCharge(int droneId, TimeSpan chargeTime)
        {
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            if (droneItem.DroneStatus != BO.DroneStatusesBL.Maintaince)
            {
                throw new BO.CannotReleaseFromChargeException(droneId);
            }
            else
            {
                double timeInMinutes = chargeTime.TotalMinutes;//converting the format to number of minutes, for instance, 1:30 to 90 minutes
                timeInMinutes /= 60; //getting the time in hours 
                droneItem.BatteryPercent = timeInMinutes * chargeRate; // the battery calculation
                if (droneItem.BatteryPercent > 100) //battery can't has more than a 100 percent
                    droneItem.BatteryPercent = 100;
                
                droneItem.DroneStatus = BO.DroneStatusesBL.Available;
                var droneChargeItem = DalAccess.GetDronesChargeList().ToList().Find(x => x.DroneId == droneId);
                var stationItem = DalAccess.GetBaseStationsList().ToList().Find(x => x.Id == droneChargeItem.StationId);
                stationItem.FreeChargeSlots++;
                DalAccess.GetDronesChargeList().ToList().Remove(droneChargeItem);
            }
        }
        
    }
}
