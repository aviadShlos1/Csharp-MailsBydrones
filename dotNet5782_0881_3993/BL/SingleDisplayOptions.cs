using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using IBL.BO;
namespace IBL
{
    partial class BL
    {
        public BaseStationBL GetBaseStation(int baseStationId)
        {
            IDAL.DO.BaseStation dalBaseStation = new();
            try
            {
                dalBaseStation = DalAccess.GetSingleBaseStation(baseStationId);
            }
            catch (IDAL.DO.NotExistException)
            {
                /* throw new BO.NotExistException(baseStationId,"baseStationId",*/ /*);*/
            }
            
            Location myLocation = new() { Latitude = dalBaseStation.Latitude, Longitude = dalBaseStation.Longitude };
            BaseStationBL myStationBl = new() { Id = dalBaseStation.Id, BaseStationName = dalBaseStation.Name, Location = myLocation, FreeChargeSlots = dalBaseStation.FreeChargeSlots, DronesInChargeList = new() };
            
            var dronesInChargePerStation = DalAccess.GetDronesChargeList().TakeWhile(x => x.StationId == baseStationId).ToList();
            foreach (var item in dronesInChargePerStation)
            {
                var tempDrone = DronesListBL.Find(x => x.DroneId == item.DroneId);
                myStationBl.DronesInChargeList.Add(new DroneInCharge { Id = tempDrone.DroneId, BatteryPercent = tempDrone.BatteryPercent });
            }
            return myStationBl;
        }
        public DroneBL GetDrone(int myDroneId)
        {
            IDAL.DO.Drone dalDrone = new();
            try
            {
                dalDrone = DalAccess.GetSingleDrone(myDroneId);
            }
            catch (IDAL.DO.NotExistException)
            {
                throw;
            }

            var tempDroneBl = DronesListBL.Find(x => x.DroneId == myDroneId);
            if (tempDroneBl.DroneStatus==DroneStatus.Shipment)
            {

            }
            
            DroneBL myDroneBl = new() {DroneId=dalDrone.Id , Model=dalDrone.Model , DroneWeight =(WeightCategoriesBL)dalDrone.DroneWeight , BatteryPercent=tempDroneBl.BatteryPercent , DroneStatus=tempDroneBl.DroneStatus , /*ParcelInShip=*/ DroneLocation=tempDroneBl.DroneLocation};
           
            return myDroneBl;


        }
        public CustomerBL GetCustomer(int customerId)
        {
            IDAL.DO.Customer myCustomer = new();
            try
            {
                myCustomer = DalAccess.GetCustomer(customerId);
            }
            catch (Exception)
            {

                throw;
            }
            Location myLocation = new() { Latitude = myCustomer.CustomerLatitude, Longitude = myCustomer.CustomerLongitude };
            CustomerBL myCustomerBl = new() { CustomerId = myCustomer.Id, CustomerName = myCustomer.Name, CustomerPhone = myCustomer.Phone, CustomerLocation = myLocation, ParcelsFromCustomerList = new(), ParcelsToCustomerList = new() };
            List<IDAL.DO.Parcel> mySentParcels = DalAccess.GetParcelsList().TakeWhile(x => x.SenderId == customerId).ToList();
            List<IDAL.DO.Parcel> myRecievedParcels = DalAccess.GetParcelsList().TakeWhile(x => x.TargetId == customerId).ToList();
            foreach (var senderItem in mySentParcels)
            {
                ParcelByCustomer myParcelByCustomer = new ParcelByCustomer()
                {
                    Id = senderItem.Id,
                    Priority = (PrioritiesBL)senderItem.Priority,
                    Weight = (WeightCategoriesBL)senderItem.Weight,
                    SourceOrTargetMan = new AssignCustomerToParcel { Id = senderItem.Id, Name = myCustomer.Name }
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

                foreach (var TargetItem in myRecievedParcels)
                {
                    ParcelByCustomer targetParcelByCustomer = new ParcelByCustomer()
                    {
                        Id = TargetItem.Id,
                        Priority = (PrioritiesBL)TargetItem.Priority,
                        Weight = (WeightCategoriesBL)TargetItem.Weight,
                        SourceOrTargetMan = new AssignCustomerToParcel { Id = TargetItem.Id, Name = myCustomer.Name }
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
        public ParcelBL GetParcel(int parcelId)
        {
            IDAL.DO.Parcel myParcel = new();
            try
            {
                myParcel = DalAccess.GetParcel(parcelId);
            }
            catch (Exception)
            {

                throw;
            }

            ParcelBL myParcelBl = new() { ParcelId = myParcel.Id, };


        }


    }
}
