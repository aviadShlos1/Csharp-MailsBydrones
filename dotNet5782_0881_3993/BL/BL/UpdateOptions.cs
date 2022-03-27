//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
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

        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void UpdateDroneName(int droneId, string newModel)
        {
            DO.DroneDal myDrone = DalAccess.GetDronesList().ToList().Find(x => x.Id == droneId);
            if (newModel!="")
                myDrone.Model = newModel;
            DalAccess.UpdateDrone(myDrone);
            DronesListBL.Find(x => x.DroneId == droneId).Model = newModel;
        }
       
        /// <summary>
        /// Updating base station name and update the free charge slots number 
        /// </summary>
        /// <param name="baseStationId"> The id of the exist base station</param>
        /// <param name="newName">a new name</param>
        /// <param name="totalChargeSlots">a new total charge slots number</param>
        public void UpdateBaseStationData(int baseStationId, string newName, int totalChargeSlots)
        {
            DO.BaseStationDal myBaseStation = DalAccess.GetBaseStationsList().ToList().Find(x => x.Id == baseStationId);
            if (newName!="")
            {
                 myBaseStation.Name = newName;
            }
            foreach (var item in DalAccess.GetBaseStationsList())
            {
                if (item.Id == baseStationId)
                {
                    if (totalChargeSlots > 0)
                    {
                        int dronesInCharge = 0;
                        foreach (var item2 in DronesListBL)
                        {
                            if (item.Latitude==item2.DroneLocation.Latitude&& item.Longitude == item2.DroneLocation.Longitude)
                                dronesInCharge++;
                        }
                        if (dronesInCharge > totalChargeSlots)
                            throw new BO.NotEnoughChargeSlotsInThisStation(baseStationId);
                        myBaseStation.FreeChargeSlots = totalChargeSlots - dronesInCharge;
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
            DO.CustomerDal myCustomer = DalAccess.GetCustomersList().ToList().Find(x => x.Id == myId);
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
        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void AssignParcelToDrone(int myDroneId)
        {
            DO.ParcelDal assignedParcel = new();
            DO.CustomerDal senderCustomer = new();
            double closetDistance = default;

            // finding the high priority parcel, taking in conclusion the priority,weight and distance
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == myDroneId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            if (droneItem.DroneStatus != BO.DroneStatusesBL.Available)
            {
                throw new BO.DroneIsNotAvailable(myDroneId);
            }

            int senderId = HighestPriorityAndWeightParcels()[0].SenderId;
            senderCustomer = GetCustomerDetails(senderId);
            /// Finding the closet parcel among the highest priority and weight parcels list
            closetDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.Longitude, senderCustomer.Latitude);
            foreach (var item in HighestPriorityAndWeightParcels())
            {
                senderCustomer = GetCustomerDetails(item.SenderId);
                double tempDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.Longitude, senderCustomer.Latitude);
                if (tempDistance <= closetDistance)
                {
                    closetDistance = tempDistance;
                    assignedParcel = item;
                }
            }
            //checking the battery consumption
            double arriveToSenderBattery = closetDistance * freeWeightConsumption;

            DO.CustomerDal targetCustomer = GetCustomerDetails(assignedParcel.TargetId);
            double targetDistance = GetDistance(senderCustomer.Longitude, senderCustomer.Latitude, targetCustomer.Longitude, targetCustomer.Latitude);
            double srcToTrgetBattery = targetDistance * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1];

            BO.BaseStationBl closetStationFromTarget = ClosetStation(targetCustomer.Longitude, targetCustomer.Latitude, DalAccess.GetBaseStationsList().ToList());
            double targetToCharge = GetDistance(targetCustomer.Longitude, targetCustomer.Latitude, closetStationFromTarget.Location.Longitude, closetStationFromTarget.Location.Latitude);
            double trgetToChargeBattery = targetToCharge * freeWeightConsumption;
            double totalDemandBattery = arriveToSenderBattery + srcToTrgetBattery + trgetToChargeBattery;

            if (droneItem.BatteryPercent < totalDemandBattery) //if the drone will not be able to do the shipment
            {
                throw new BO.CannotAssignDroneToParcelException(myDroneId);
            }
            else
            {
                droneItem.DroneStatus = BO.DroneStatusesBL.Shipment;
                droneItem.ParcelAssignedId = assignedParcel.Id;
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
        private List<DO.ParcelDal> HighestPriorityParcels()
        {
            List<DO.ParcelDal> parcelsWithUrgentPriority = new();
            List<DO.ParcelDal> parcelsWithFastPriority = new();
            List<DO.ParcelDal> parcelsWithNormalPriority = new();

            foreach (var item in DalAccess.GetParcelsList(x => x.AssignningTime == null))
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
        private List<DO.ParcelDal> HighestPriorityAndWeightParcels()
        {
            List<DO.ParcelDal> heavyParcels = new();
            List<DO.ParcelDal> mediumParcels = new();
            List<DO.ParcelDal> lightParcels = new();

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
        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void PickUpParcel(int droneId)
        {
            DO.ParcelDal parcelItem = new();
            DO.CustomerDal senderItem = new();
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            if (droneItem.ParcelAssignedId == 0) // if the drone doesn't take any parcel
                throw new BO.CannotPickUpException("The drone has not transfered parcels yet");
            parcelItem = DalAccess.GetSingleParcel(droneItem.ParcelAssignedId);
            senderItem = GetCustomerDetails(parcelItem.SenderId);

            if (parcelItem.PickingUpTime != null) //checking if the parcel has already picked up
                throw new BO.CannotPickUpException("The parcel has already picked up");

            else //updating the battery,location and picking up time
            {
                double currentToSender = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderItem.Longitude, senderItem.Latitude);
                droneItem.BatteryPercent -= Math.Floor(currentToSender * freeWeightConsumption);
                droneItem.DroneLocation.Longitude = senderItem.Longitude;
                droneItem.DroneLocation.Latitude = senderItem.Latitude;
                DalAccess.PickUpParcel(parcelItem.Id);
            }
        }

        /// <summary>
        /// Updating the parcel supply details
        /// </summary>
        /// <param name="droneId"> an exist drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void SupplyParcel(int droneId)
        {
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            if (droneItem.ParcelAssignedId == 0) //if the drone doesn't take any parcel
                throw new BO.CannotSupplyException("The drone has not transfered parcels yet");
            
            var parcelItem = DalAccess.GetSingleParcel(droneItem.ParcelAssignedId);
            var targetItem = GetCustomerDetails(parcelItem.TargetId);
            if (parcelItem.PickingUpTime == null)
                throw new BO.CannotSupplyException("The parcel has not picked up yet");
            if (parcelItem.SupplyingTime != null)
                throw new BO.CannotSupplyException("The parcel has already supplied");
           
            else //updating the battery,location, status and suppling time
            {
                double currentToTarget = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, targetItem.Longitude, targetItem.Latitude);
                switch ((WeightCategoriesBL)parcelItem.Weight)
                {
                    case WeightCategoriesBL.Light:
                        droneItem.BatteryPercent -= GetDistance(targetItem.Longitude, targetItem.Latitude, droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude) * lightWeightConsumption;
                        break;
                    case WeightCategoriesBL.Medium:
                        droneItem.BatteryPercent -= GetDistance(targetItem.Longitude, targetItem.Latitude, droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude) * mediumWeightConsumption;
                        break;
                    case WeightCategoriesBL.Heavy:
                        droneItem.BatteryPercent -= GetDistance(targetItem.Longitude, targetItem.Latitude, droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude) * heavyWeightConsumption;
                        break;
                    default:
                        break;
                }

                //droneItem.BatteryPercent -= Math.Floor(currentToTarget * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1]);
                droneItem.DroneLocation.Longitude = targetItem.Longitude;
                droneItem.DroneLocation.Latitude = targetItem.Latitude;
                droneItem.ParcelAssignedId = 0; // initialize the id of the transfer parcel, in that we will know that the drone will be available for a new mission
                droneItem.DroneStatus = BO.DroneStatusesBL.Available;
                DalAccess.SupplyParcel(parcelItem.Id);
            }

        }

        /// <summary>
        /// Sending drone for charging in order to fill its battery
        /// </summary>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void DroneToCharge(int droneId)
        {
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            
            List<DO.BaseStationDal> freeChargeSlotsStations = DalAccess.GetBaseStationsList(x=>x.FreeChargeSlots>0).ToList(); 
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
                    DalAccess.SendDroneToCharge(droneItem.DroneId, closetBaseStation.Id);
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
        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void ReleaseDroneCharge(int droneId)
        {
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            }
            catch (DO.NotExistException)
            {
                throw new BO.NotExistException();
            }
            if (droneItem.DroneStatus != BO.DroneStatusesBL.Maintaince)
            {
                throw new BO.CannotReleaseFromChargeException(droneId);
            }
            else
            {
                TimeSpan chargeTime = DalAccess.DroneToRelease(droneId);
                double timeInMinutes = chargeTime.TotalMinutes;//converting the format to number of minutes, for instance, 1:30 to 90 minutes
                timeInMinutes /= 60; //getting the time in hours 
                droneItem.BatteryPercent += Math.Ceiling(timeInMinutes * chargeRate); // the battery calculation
                if (droneItem.BatteryPercent > 100) //battery can't has more than a 100 percent
                    droneItem.BatteryPercent = 100;
                
                droneItem.DroneStatus = BO.DroneStatusesBL.Available;
                var droneChargeItem = DalAccess.GetDronesChargeList().ToList().Find(x => x.DroneId == droneId);
                var stationItem = DalAccess.GetBaseStationsList().ToList().Find(x => x.Id == droneChargeItem.StationId);
                DalAccess.GetDronesChargeList().ToList().Remove(droneChargeItem);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)] // an attribute that prevent two function to call simultaneously 
        public void RemoveParcel(ParcelBl myParcel)
        {
            DO.ParcelDal temp = DalAccess.GetSingleParcel(myParcel.ParcelId);
            DalAccess.RemoveParcel(temp);
        }

        //function for simulator
        public void sim(int droneId, Action reportProgressInSimultor, Func<bool> isTimeRun)
        {
            new Simulator(this, droneId, reportProgressInSimultor, isTimeRun);
        }
    }
}



//        [MethodImpl(MethodImplOptions.Synchronized)]
//        public void AssignParcelToDrone(int droneId)
//        {
//            lock (DalAccess)
//            {
//                DroneToList myDrone = DronesListBL.Find(x => x.DroneId == droneId);
//                if (myDrone == default)
//                    throw new NotExistException();

//                if (myDrone.DroneStatus != DroneStatusesBL.Available)
//                    throw new CannotAssignDroneToParcelException(myDrone.DroneId);

//                IEnumerable<DO.ParcelDal> tempParcels = from item in DalAccess.GetParcelsList(x => x.DroneToParcelId == 0 &&
//                                                          myDrone.DroneWeight >= (WeightCategoriesBL)x.Weight && possibleDistance(x, myDrone))
//                                                     orderby item.Priority descending, item.Weight descending,
//                                                             GetDistance(GetSingleCustomer(item.SenderId).Location, myDrone.DroneLocation)
//                                                     select item;

//                if (!tempParcels.Any())
//                    throw new NotExistException();

//                DO.ParcelDal theRightParcel = tempParcels.First();

//                myDrone.DroneStatus = DroneStatusesBL.Maintaince;
//                myDrone.ParcelAssignedId = theRightParcel.Id;

//                DalAccess.AssignParcelToDrone(theRightParcel.Id, droneId);
//            }
//        }

//        /// <summary>
//        /// The function calculates whether the drone can reach the parcel.
//        /// </summary>
//        /// <param name="parcel">list of the most urgent parcels</param>
//        /// <param name="myDrone">drone object</param>
//        /// <returns></returns>
//        private bool possibleDistance(DO.ParcelDal parcel, DroneToList myDrone)
//        {
//            double electricityUse = GetDistance(myDrone.DroneLocation, GetSingleCustomer(parcel.SenderId).Location) * freeWeightConsumption;
//            double distanceSenderToDestination = GetDistance(GetSingleCustomer(parcel.SenderId).Location, GetSingleCustomer(parcel.TargetId).Location);
//            switch ((WeightCategoriesBL)parcel.Weight)
//            {
//                case WeightCategoriesBL.Light:
//                    electricityUse += distanceSenderToDestination * lightWeightConsumption;
//                    break;
//                case WeightCategoriesBL.Medium:
//                    electricityUse += distanceSenderToDestination * mediumWeightConsumption;
//                    break;
//                case WeightCategoriesBL.Heavy:
//                    electricityUse += distanceSenderToDestination * heavyWeightConsumption;
//                    break;
//                default:
//                    break;
//            }

//            if (myDrone.BatteryPercent - electricityUse < 0)//if its lowest than zero no need to continue
//                return false;

//            IEnumerable<DO.BaseStationDal> holdDalBaseStation = DalAccess.GetBaseStationsList();
//            IEnumerable<BaseStationBl> baseStationBL = from item in holdDalBaseStation
//                                                     select new BaseStationBl()
//                                                     {
//                                                         Id = item.Id,
//                                                         BaseStationName = item.Name,
//                                                         FreeChargeSlots = item.FreeChargeSlots,
//                                                         Location = new Location()
//                                                         {
//                                                             Longitude = item.Longitude,
//                                                             Latitude = item.Latitude
//                                                         }
//                                                     };
//            electricityUse += minDistanceBetweenBaseStationsAndLocation(baseStationBL, GetSingleCustomer(parcel.TargetId).Location).Item2 * freeWeightConsumption;

//            if (myDrone.BatteryPercent - electricityUse < 0)
//                return false;
//            return true;
//        }

//        #region Function of finding the location of the base station closest to the location
//        /// <summary>
//        /// The function calculates the distance between a particular location and base stations.
//        /// </summary>
//        /// <param name="baseStationBL">baseStationBL List</param>
//        /// <param name="location">location</param>
//        /// <returns>The location of the base station closest to the location and the min distance</returns>
//        public (Location, double) minDistanceBetweenBaseStationsAndLocation(IEnumerable<BaseStationBl> baseStationBL, Location location)
//        {
//            IEnumerable<Location> locations = from item in baseStationBL
//                                              orderby GetDistance(location, item.Location)
//                                              select item.Location;
//            Location locationOfNearestStation = locations.First();
//            return (locationOfNearestStation, GetDistance(location, locationOfNearestStation));
//        }
//        #endregion Function of finding the location of the base station closest to the location


//        #region Function of calculating distance between points
//        /// <summary>
//        /// A function that calculates the distance between points.
//        /// </summary>
//        /// <param name="location1">location 1</param>
//        /// <param name="location2">location 2</param>
//        /// <returns>the distence between the points</returns>
//        private double GetDistance(Location location1, Location location2)
//        {
//            //For the calculation we calculate the earth into a circle (ellipse) Divide its 360 degrees by half
//            //180 for each longitude / latitude and then make a pie on each half to calculate the radius for
//            //the formula below
//            var num1 = location1.Longitude * (Math.PI / 180.0);
//            var d1 = location1.Latitude * (Math.PI / 180.0);
//            var num2 = location2.Longitude * (Math.PI / 180.0) - num1;
//            var d2 = location2.Latitude * (Math.PI / 180.0);

//            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0); //https://iw.waldorf-am-see.org/588999-calculating-distance-between-two-latitude-QPAAIP
//                                                                                                                                   //We calculate the distance according to a formula that
//                                                                                                                                   // also takes into account the curvature of the earth
//            return ((double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))))) / 1000;
//        }
//#endregion Function of calculating distance between points

