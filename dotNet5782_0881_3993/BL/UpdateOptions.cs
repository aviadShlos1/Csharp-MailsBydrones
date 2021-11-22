using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    partial class BL
    {
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
                            {
                                dronesInCharge++;
                            }
                        }
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
        public void DroneToCharge(int myDroneId)
        {
            List<IDAL.DO.BaseStation> freeChargeSlotsStations = DalAccess.GetStationsWithFreeCharge().ToList();
            var droneItem = DronesListBL.Find(x => x.DroneId == myDroneId);

            if (freeChargeSlotsStations.Count == 0 || (droneItem != default && droneItem.DroneStatus != BO.DroneStatus.Free))
                throw new BO.CannotGoToChargeException(myDroneId);
            else
            {
                double stationLon = freeChargeSlotsStations[0].Longitude;
                double stationLat = freeChargeSlotsStations[0].Latitude;
                double closetDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, stationLon, stationLat);
                BO.BaseStationBL closetBaseStation = ClosetStation(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, freeChargeSlotsStations);
                if (droneItem.BatteryPercent >= closetDistance * freeWeightConsumption)
                {
                    droneItem.BatteryPercent = closetDistance * freeWeightConsumption;
                    droneItem.DroneLocation.Longitude = stationLon;
                    droneItem.DroneLocation.Latitude = stationLat;
                    droneItem.DroneStatus = BO.DroneStatus.Maintaince;
                    closetBaseStation.FreeChargeSlots--;
                    DalAccess.DroneToCharge(droneItem.DroneId, closetBaseStation.Id);
                }
                else
                {
                    throw new BO.CannotGoToChargeException(myDroneId);
                }
            }

        }
        public void ReleaseDroneCharge(int myDroneId, TimeSpan chargeTime)
        {
            var droneItem = DronesListBL.Find(x => x.DroneId == myDroneId);
            if (droneItem.DroneStatus != BO.DroneStatus.Maintaince)
            {
                throw new BO.CannotReleaseFromChargeException(myDroneId);
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
                var droneChargeItem = DalAccess.GetDronesChargeList().ToList().Find(x => x.DroneId == myDroneId);
                var stationItem = DalAccess.GetBaseStationsList().ToList().Find(x => x.Id == droneChargeItem.StationId);
                stationItem.FreeChargeSlots++;
                DalAccess.GetDronesChargeList().ToList().Remove(droneChargeItem);
            }
        }
        public void AssignParcelToDrone(int myDroneId)
        {
            IDAL.DO.Parcel assignedParcel = default;
            IDAL.DO.Customer senderCustomer = default;
            double closetDistance = default;

            // finding the high priority parcel, taking in conclusion the priority,weight and distance. 
            var droneItem = DronesListBL.Find(x => x.DroneId == myDroneId);
            if (droneItem.DroneStatus != BO.DroneStatus.Free)
            {
                throw new BO.CannotAssignDroneToParcelException(myDroneId);
            }
            List<IDAL.DO.Parcel> urgentParcels = DalAccess.GetParcelsList().TakeWhile(x => x.Priority == IDAL.DO.Priorities.Urgent).ToList();
            List<IDAL.DO.Parcel> urgentPlusHeavyParcels = urgentParcels.TakeWhile(x => x.Weight == IDAL.DO.WeightCategoriesDal.Heavy).ToList();
            senderCustomer = GetCustomerDetails(urgentPlusHeavyParcels[0].SenderId);
            closetDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude);
            foreach (var item in urgentPlusHeavyParcels)
            {
                senderCustomer = GetCustomerDetails(item.SenderId);
                double tempDistance = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude);
                if (tempDistance <= closetDistance)
                {
                    closetDistance = tempDistance;
                    assignedParcel = item;
                }
            }
            //checking the battery level
            double arriveToSenderBattery = closetDistance * freeWeightConsumption;

            IDAL.DO.Customer targetCustomer = GetCustomerDetails(assignedParcel.TargetId);
            double targetDistance = GetDistance(senderCustomer.CustomerLongitude, senderCustomer.CustomerLatitude, targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude);
            double srcToTrgetBattery = targetDistance * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1];

            BO.BaseStationBL closetStationFromTarget = ClosetStation(targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude, DalAccess.GetBaseStationsList().ToList());
            double targetToCharge = GetDistance(targetCustomer.CustomerLongitude, targetCustomer.CustomerLatitude, closetStationFromTarget.Location.Longitude, closetStationFromTarget.Location.Latitude);
            double trgetToChargeBattery = targetToCharge * freeWeightConsumption;
            if (droneItem.BatteryPercent < (arriveToSenderBattery + srcToTrgetBattery + trgetToChargeBattery))
            {
                throw new BO.NotEnoughBatteryException(myDroneId);
            }
            else
            {
                droneItem.DroneStatus = BO.DroneStatus.Shipment;
                assignedParcel.DroneToParcelId = myDroneId;
                assignedParcel.AssignningTime = DateTime.Now;
            }

        }

        public void PickUpParcel(int droneId)
        {
            var droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            var parcelItem = DalAccess.GetParcelsList().ToList().Find(x => x.DroneToParcelId == droneId);
            var senderItem = GetCustomerDetails(parcelItem.SenderId);

            if (!(parcelItem.AssignningTime != DateTime.MinValue && parcelItem.PickingUpTime == DateTime.MinValue))
            {
                throw new BO.CannotPickUpException(droneId);
            }
            else
            {
                double currentToSender = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, senderItem.CustomerLongitude, senderItem.CustomerLatitude);
                droneItem.BatteryPercent = currentToSender * freeWeightConsumption;
                droneItem.DroneLocation.Longitude = senderItem.CustomerLongitude;
                droneItem.DroneLocation.Latitude = senderItem.CustomerLatitude;
                parcelItem.PickingUpTime = DateTime.Now;
            }
        }
        public void SupplyParcel(int droneId)
        {
            var droneItem = DronesListBL.Find(x => x.DroneId == droneId);
            var parcelItem = DalAccess.GetParcelsList().ToList().Find(x => x.DroneToParcelId == droneId);
            var targetItem = GetCustomerDetails(parcelItem.TargetId);

            if (!(parcelItem.PickingUpTime != DateTime.MinValue && parcelItem.SupplyingTime == DateTime.MinValue))
            {
                throw new BO.CannotSupplyException(droneId);
            }
            else
            {
                double currentToTarget = GetDistance(droneItem.DroneLocation.Longitude, droneItem.DroneLocation.Latitude, targetItem.CustomerLongitude, targetItem.CustomerLatitude);
                droneItem.BatteryPercent = currentToTarget * DalAccess.EnergyConsumption()[(int)droneItem.DroneWeight + 1];
                droneItem.DroneLocation.Longitude = targetItem.CustomerLongitude;
                droneItem.DroneLocation.Latitude = targetItem.CustomerLatitude;
                droneItem.DroneStatus = BO.DroneStatus.Free;
                parcelItem.SupplyingTime = DateTime.Now;
            }

        }

    }
}
