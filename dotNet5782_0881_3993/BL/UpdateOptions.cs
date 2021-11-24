using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace IBL
{
    partial class BL
    {
        public void AssignParcelToDrone(int myDroneId)
        {
            IDAL.DO.ParcelDal assignedParcel = default;
            IDAL.DO.CustomerDal senderCustomer = default;
            double closetDistance = default;

            // finding the high priority parcel, taking in conclusion the priority,weight and distance. 
            DroneToList droneItem = new();
            try
            {
                droneItem = DronesListBL.Find(x => x.DroneId == myDroneId);
            }
            catch (IDAL.DO.NotExistException)
            {

                throw new BO.NotExistException();
            }
            if (droneItem.DroneStatus != BO.DroneStatus.Free)
            {
                throw new BO.DroneIsNotAvailable(myDroneId);
            }

            senderCustomer = GetCustomerDetails(HighestPriorityAndWeightParcels()[0].SenderId);
            closetDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude);
            foreach (var item in HighestPriorityAndWeightParcels())
            {
                senderCustomer = GetCustomerDetails(item.SenderId);
                double tempDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude);
                if (tempDistance <= closetDistance)
                {
                    closetDistance = tempDistance;
                    assignedParcel = item;
                }//
            }
            //checking the battery level
            double arriveToSenderBattery = closetDistance * freeWeightConsumption;

            IDAL.DO.CustomerDal targetCustomer = GetCustomerDetails(assignedParcel.TargetId);
            double targetDistance = GetDistance(senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude, targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude);
            double srcToTrgetBattery = targetDistance * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1];

            BO.BaseStationBl closetStationFromTarget = ClosetStation(targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude, DalAccess.GetBaseStationsList().ToList());
            double targetToCharge = GetDistance(targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude, closetStationFromTarget.Location.Longitude, closetStationFromTarget.Location.Latitude);
            double trgetToChargeBattery = targetToCharge * freeWeightConsumption;

            if (droneItem.BatteryPercent < (arriveToSenderBattery + srcToTrgetBattery + trgetToChargeBattery))
            {
                throw new BO.CannotAssignDroneToParcelException(myDroneId);
            }
            else
            {
                droneItem.DroneStatus = BO.DroneStatus.Shipment;
                droneItem.TransferParcelsNum = assignedParcel.Id;
                assignedParcel.DroneToParcelId = myDroneId;
                assignedParcel.AssignningTime = DateTime.Now;

                DalAccess.AssignParcelToDrone(assignedParcel.Id, myDroneId);
            }

        }
        #region Help methods for AssignParcelToDrone method
        private List<IDAL.DO.ParcelDal> HighestPriorityParcels()
        {
            List<IDAL.DO.ParcelDal> parcelsWithUrgentPriority = new();
            List<IDAL.DO.ParcelDal> parcelsWithFastPriority = new();
            List<IDAL.DO.ParcelDal> parcelsWithNormalPriority = new();

            foreach (var item in DalAccess.GetParcelsWithoutDrone())
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
            if (droneItem.TransferParcelsNum == 0)
                throw new BO.CannotPickUpException("The drone has not transfered parcels yet");
            parcelItem = DalAccess.GetSingleParcel(droneItem.TransferParcelsNum);
            senderItem = GetCustomerDetails(parcelItem.SenderId);

            if (parcelItem.PickingUpTime != DateTime.MinValue)
            {
                throw new BO.CannotPickUpException("The parcel has already picked up");
            }
            else
            {
                double currentToSender = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderItem.CustomerLongitude, senderItem.CustomerLatitude);
                droneItem.BatteryPercent -= currentToSender * freeWeightConsumption;
                droneItem.DroneLocation.Longitude = senderItem.CustomerLongitude;
                droneItem.DroneLocation.Latitude = senderItem.CustomerLatitude;
                parcelItem.PickingUpTime = DateTime.Now;
                DalAccess.PickUpParcel(parcelItem.Id);
            }
        }
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
            if (droneItem.TransferParcelsNum == 0)
                throw new BO.CannotSupplyException("The drone has not transfered parcels yet");
            var parcelItem = DalAccess.GetSingleParcel(droneItem.TransferParcelsNum);
            var targetItem = GetCustomerDetails(parcelItem.TargetId);
            if (parcelItem.PickingUpTime == DateTime.MinValue)
                throw new BO.CannotSupplyException("The parcel has not picked up yet");
            if (parcelItem.SupplyingTime != DateTime.MinValue)
                throw new BO.CannotSupplyException("The parcel has already supplied");
            else
            {
                double currentToTarget = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, targetItem.CustomerLongitude, targetItem.CustomerLatitude);
                droneItem.BatteryPercent -= currentToTarget * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1];
                droneItem.DroneLocation.Longitude = targetItem.CustomerLongitude;
                droneItem.DroneLocation.Latitude = targetItem.CustomerLatitude;
                droneItem.TransferParcelsNum = 0;
                droneItem.DroneStatus = BO.DroneStatus.Free;
                parcelItem.SupplyingTime = DateTime.Now;
                DalAccess.SupplyParcel(parcelItem.Id);
            }

        }
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
            
            List<IDAL.DO.BaseStationDal> freeChargeSlotsStations = DalAccess.GetStationsWithFreeCharge().ToList();
            if ((droneItem.DroneStatus != BO.DroneStatus.Free))
                throw new DroneIsNotAvailable(droneId);
            if (freeChargeSlotsStations.Count == 0 )
                throw new BO.CannotGoToChargeException(droneId);
            else
            {
                double stationLon = freeChargeSlotsStations[0].Longitude;
                double stationLat = freeChargeSlotsStations[0].Latitude;
                double closetDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, stationLon, stationLat);
                BO.BaseStationBl closetBaseStation = ClosetStation(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, freeChargeSlotsStations);
                if (droneItem.BatteryPercent >= closetDistance * freeWeightConsumption)
                {
                    droneItem.BatteryPercent -= closetDistance * freeWeightConsumption;
                    droneItem.DroneLocation.Longitude = stationLon;
                    droneItem.DroneLocation.Latitude = stationLat;
                    droneItem.DroneStatus = BO.DroneStatus.Maintaince;
                    closetBaseStation.FreeChargeSlots--;
                    DalAccess.DroneToCharge(droneItem.DroneId, closetBaseStation.Id);
                }
                else
                {
                    throw new BO.CannotGoToChargeException(droneId);
                }
            }

        }
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
            if (droneItem.DroneStatus != BO.DroneStatus.Maintaince)
            {
                throw new BO.CannotReleaseFromChargeException(droneId);
            }
            else
            {
                double timeInMinutes = chargeTime.TotalMinutes;
                timeInMinutes /= 60;
                droneItem.BatteryPercent = timeInMinutes * chargeRate;
                if (droneItem.BatteryPercent > 100)
                {
                    droneItem.BatteryPercent = 100;
                }
                droneItem.DroneStatus = BO.DroneStatus.Free;
                var droneChargeItem = DalAccess.GetDronesChargeList().ToList().Find(x => x.DroneId == droneId);
                var stationItem = DalAccess.GetBaseStationsList().ToList().Find(x => x.Id == droneChargeItem.StationId);
                stationItem.FreeChargeSlots++;
                DalAccess.GetDronesChargeList().ToList().Remove(droneChargeItem);
            }
        }
        public void UpdateDroneName(int droneId, string newModel)
        {
            foreach (var item in DalAccess.GetDronesList())
            {
                if (item.Id == droneId)
                {
                    string tempModel = item.Model;
                    tempModel = newModel;

                }
            }
        }
        public void UpdateBaseStationData(int baseStationId, string newName, int totalChargeSlots)
        {
            foreach (var item in DalAccess.GetBaseStationsList())
            {
                if (item.Id == baseStationId)
                {
                    if (newName != "")
                    {
                        string tempName = item.Name;
                        tempName = newName;
                    }
                    if (totalChargeSlots != 0)
                    {
                        int dronesInCharge = 0;
                        foreach (var item2 in DronesListBL)
                        {
                            if (item2.DroneStatus == BO.DroneStatus.Maintaince)
                                dronesInCharge++;
                        }
                        if (dronesInCharge > totalChargeSlots)
                            throw new BO.NotEnoughChargeSlotsInThisStation(baseStationId);
                        int free = item.FreeChargeSlots;
                        free = totalChargeSlots - dronesInCharge;
                    }
                }
            }

        }
        public void UpdateCustomerData(int myId, string newName, string newPhone)
        {
            foreach (var item in DalAccess.GetCustomersList())
            {
                if (item.Id == myId)
                {
                    if (newName != "")
                    {
                        string tempName = item.Name;
                        tempName = newName;
                    }
                    if (newPhone != "")
                    {
                        string tempPhone = item.Phone;
                        tempPhone = newPhone;
                    }
                }
            }
        }
      
    }
}
