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
    partial class BL
    {
        public List<BaseStationToList> GetBaseStationsBl (Predicate<BaseStationToList> myPredicate = null)
        {
            List<BaseStationToList> myBaseStationsBl = new();
            List<DO.BaseStationDal> dalBaseStations = DalAccess.GetBaseStationsList().ToList();
            foreach (var item in dalBaseStations)
            {
                //int busyChargeSlots = DalAccess.GetDronesChargeList().ToList().FindAll(x => x.StationId == item.Id).Count();
                myBaseStationsBl.Add(new BaseStationToList { Id = item.Id, BaseStationName = item.Name,
                    FreeChargeSlots = item.FreeChargeSlots, BusyChargeSlots = DalAccess.GetDronesChargeList().Count() });
            }
            return myBaseStationsBl.FindAll(x => myPredicate == null ? true : myPredicate(x)).ToList()/*x => x.FreeChargeSlots > 0*/;
        }
        public List<DroneToList> GetDronesBl(Predicate<DroneToList> myPredicate = null)
        {
            return DronesListBL.FindAll(x => myPredicate == null ? true : myPredicate(x)).ToList();
        }
        public List<CustomerToList> GetCustomersBl()
        {
            List<CustomerToList> myCustomersBl = new();
            List<DO.CustomerDal> dalCustomers = DalAccess.GetCustomersList().ToList();
            foreach (var item in dalCustomers)
            {
                List<DO.ParcelDal> mySentParcels = DalAccess.GetParcelsList().ToList().FindAll(x => x.SenderId == item.Id).ToList();
                int sentAndSuppliedParcels = mySentParcels.FindAll(x => x.SupplyingTime != null).Count();
                int sentAndNotSuppliedParcels = mySentParcels.FindAll(x => x.SupplyingTime == null).Count();
                List<DO.ParcelDal> myRecievedParcels = DalAccess.GetParcelsList().ToList().FindAll(x => x.TargetId == item.Id).ToList();
                int recieverGotParcels = myRecievedParcels.FindAll(x => x.SupplyingTime != null).Count();
                int parcelsInWayToReciever = myRecievedParcels.FindAll(x => x.SupplyingTime == null).Count();
                myCustomersBl.Add(new CustomerToList { Id = item.Id, Name = item.Name, Phone = item.Phone, SentAndSuppliedParcels = sentAndSuppliedParcels, SentAndNotSuppliedParcels = sentAndNotSuppliedParcels, RecieverGotParcels = recieverGotParcels, ParcelsInWayToReciever = parcelsInWayToReciever });
            }
            return myCustomersBl;
        }
        public List<ParcelToList> GetParcelsBl(Predicate<ParcelToList> myPredicate = null)
        {
            List<ParcelToList> myParcelsBl = new();
            List<DO.ParcelDal> dalParcels = DalAccess.GetParcelsList().ToList();

            foreach (var item in dalParcels)
            {
                ParcelToList tempParcelTolist = new ParcelToList { Id = item.Id, Weight = (WeightCategoriesBL)item.Weight, Priority = (PrioritiesBL)item.Priority, SenderName = GetCustomerDetails(item.SenderId).Name, RecieverName = GetCustomerDetails(item.TargetId).Name };

                if (item.AssignningTime != DateTime.MinValue)
                    tempParcelTolist.ParcelStatus = ParcelStatus.Assigned;
                if (item.CreatingTime != DateTime.MinValue)
                    tempParcelTolist.ParcelStatus = ParcelStatus.Created;
                if (item.PickingUpTime != DateTime.MinValue)
                    tempParcelTolist.ParcelStatus = ParcelStatus.PickedUp;
                if (item.SupplyingTime != DateTime.MinValue)
                    tempParcelTolist.ParcelStatus = ParcelStatus.Supplied;

                myParcelsBl.Add(tempParcelTolist);
            }
            return myParcelsBl.FindAll(x => myPredicate == null ? true : myPredicate(x)).ToList()/*(x => x.ParcelStatus == ParcelStatus.Created)*/;
        }

        //public List<ParcelToList> GetParcelsWithoutDroneBl()
        //{
        //    List<ParcelToList> myParcelsWithoutDroneBl = new();
        //    myParcelsWithoutDroneBl = GetParcelsBl().FindAll(x => x.ParcelStatus == ParcelStatus.Created).ToList();
        //    return myParcelsWithoutDroneBl;
        //}

        //public List<BaseStationToList> GetStationsWithFreeChargeBl()
        //{
        //    List<BaseStationToList> myBaseStationsWithFreeChargeBl = new();
        //    myBaseStationsWithFreeChargeBl = GetBaseStationsBl().FindAll(x => x.FreeChargeSlots > 0).ToList();
        //    return myBaseStationsWithFreeChargeBl;
        //}

    }
}
