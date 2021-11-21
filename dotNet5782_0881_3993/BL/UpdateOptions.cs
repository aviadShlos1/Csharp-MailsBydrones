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

            foreach (var item in DalAccess.DronesListDisplay())
            {
                if (item.Id == droneId)
                {
                    string m = item.Model;
                    m = newModel;

                }
            }

        }
        public void UpdateBaseStationData(int baseStationId, string newName, int totalChargeSlots)
        {

            foreach (var item in DalAccess.BaseStationsListDisplay())
            {
                if (item.Id == baseStationId)
                {
                    if (newName != "")
                    {
                        string temp = item.Name;
                        temp = newName;

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
            foreach (var item in DalAccess.CustomersListDisplay())
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
            List<IDAL.DO.BaseStation> freeChargeSlotsStations = DalAccess.StationsWithFreeChargeSlots().ToList();
            var element = DronesListBL.Find(x => x.DroneId == myDroneId);

            if (freeChargeSlotsStations.Count==0 || (element != default && element.DroneStatus != BO.DroneStatus.Free))
            {
                throw new BO.CannotGoToChargeException(myDroneId);
            }
            else
            {
                double stationLon = freeChargeSlotsStations[0].Longitude;
                double stationLat = freeChargeSlotsStations[0].Latitude;
                double closetDistance = GetDistance(element.DroneLocation.Longitude, element.DroneLocation.Latitude, stationLon, stationLat);
                foreach (var item in freeChargeSlotsStations)
                {
                    double tempDis = GetDistance(element.DroneLocation.Longitude, element.DroneLocation.Latitude, item.Longitude, item.Latitude);
                    if (tempDis < closetDistance)
                    {
                        closetDistance = tempDis;
                        stationLon = item.Longitude;
                        stationLat = item.Latitude;                       
                    }
                }


                element.DroneStatus = BO.DroneStatus.Maintaince;
            }

        }
        //    public void ReleaseDroneCharge();
        //    public void AssignParcelToDrone();
        //    public void PickUpParcel();
        //    public void SupplyParcel();
        //}
    }
}
