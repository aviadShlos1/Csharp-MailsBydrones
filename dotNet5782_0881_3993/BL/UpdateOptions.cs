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
            var element = DronesListBL.Find(x => x.DroneId == myDroneId);

            if (freeChargeSlotsStations.Count==0 || (element != default && element.DroneStatus != BO.DroneStatus.Free))
                throw new BO.CannotGoToChargeException(myDroneId);           
            else
            {
                double stationLon = freeChargeSlotsStations[0].Longitude;
                double stationLat = freeChargeSlotsStations[0].Latitude;
                double closetDistance = GetDistance(element.DroneLocation.Longitude, element.DroneLocation.Latitude, stationLon, stationLat);
                BO.BaseStationBL closetBaseStation = ClosetStation(element.DroneLocation.Longitude, element.DroneLocation.Latitude, freeChargeSlotsStations);
                if (element.BatteryPercent >= closetDistance * freeWeightConsumption)
                {
                    element.BatteryPercent = closetDistance * freeWeightConsumption;
                    element.DroneLocation.Longitude = stationLon;
                    element.DroneLocation.Latitude = stationLat;
                    element.DroneStatus = BO.DroneStatus.Maintaince;
                    closetBaseStation.FreeChargeSlots--;
                    DalAccess.DroneToCharge(element.DroneId, closetBaseStation.Id);
                }
                else
                {
                    throw new BO.CannotGoToChargeException(myDroneId);
                }
            }

        }
        public void ReleaseDroneCharge(int myDroneId,TimeSpan chargeTime)
        {
            var droneItem = DronesListBL.Find(x => x.DroneId == myDroneId);
            if (droneItem.DroneStatus!=BO.DroneStatus.Maintaince)
            {
                throw new BO.CannotReleaseFromChargeException(myDroneId);
            }
            else
            {
                double timeInMinutes = chargeTime.Minutes;
                timeInMinutes /= 60;
                droneItem.BatteryPercent = timeInMinutes * chargeRate;
                droneItem.DroneStatus = BO.DroneStatus.Free;
                var droneChargeItem = DalAccess.GetDronesChargeList().ToList().Find(x => x.DroneId == myDroneId);
                var stationItem = DalAccess.GetBaseStationsList().ToList().Find(x => x.Id == droneChargeItem.StationId);
                stationItem.FreeChargeSlots++;
                DalAccess.GetDronesChargeList().ToList().Remove(droneChargeItem);
            }
        }
        public void AssignParcelToDrone()
        {

        }
        //    public void PickUpParcel();
        //    public void SupplyParcel();
        //}
    }
}
