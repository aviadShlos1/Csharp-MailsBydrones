//Names: Aviad Shlosberg       314960881      
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
    partial class BL
    {
        public List<BaseStationToList> GetBaseStationsBl (Predicate<BaseStationToList> myPredicate=null)
        {
            List<BaseStationToList> myBaseStationsBl = new();
            List<IDAL.DO.BaseStationDal> dalBaseStations = DalAccess.GetBaseStationsList().ToList();
            foreach (var item in dalBaseStations)
            {
                //int busyChargeSlots = DalAccess.GetDronesChargeList().ToList().FindAll(x => x.StationId == item.Id).Count();
                myBaseStationsBl.Add(new BaseStationToList { Id = item.Id, BaseStationName = item.Name,
                    FreeChargeSlots = item.FreeChargeSlots, BusyChargeSlots = DalAccess.GetDronesChargeList(x => x.StationId == item.Id).Count() });
            }
            return myBaseStationsBl.FindAll(x=> myPredicate == null ? true : myPredicate(x));
        }
        public List<DroneToList> GetDronesBl()
        {
            return DronesListBL;
        }
        public List<CustomerToList> GetCustomersBl()
        {
            List<CustomerToList> myCustomersBl = new();
            List<IDAL.DO.CustomerDal> dalCustomers = DalAccess.GetCustomersList().ToList();
            foreach (var item in dalCustomers)
            {
                List<IDAL.DO.ParcelDal> mySentParcels = DalAccess.GetParcelsList().TakeWhile(x => x.SenderId == item.Id).ToList();
                int sentAndSuppliedParcels = mySentParcels.TakeWhile(x => x.SupplyingTime != DateTime.MinValue).Count();
                int sentAndNotSuppliedParcels = mySentParcels.TakeWhile(x => x.SupplyingTime == DateTime.MinValue).Count();
                List<IDAL.DO.ParcelDal> myRecievedParcels = DalAccess.GetParcelsList().TakeWhile(x => x.TargetId == item.Id).ToList();
                int recieverGotParcels = myRecievedParcels.TakeWhile(x => x.SupplyingTime != DateTime.MinValue).Count();
                int parcelsInWayToReciever = myRecievedParcels.TakeWhile(x => x.SupplyingTime == DateTime.MinValue).Count();
                myCustomersBl.Add(new CustomerToList { Id = item.Id, Name = item.Name, Phone = item.Phone, SendAndSuppliedParcels = sentAndSuppliedParcels, SendAndNotSuppliedParcels = sentAndNotSuppliedParcels, RecieverGotParcels = recieverGotParcels, ParcelsInWayToReciever = parcelsInWayToReciever });
            }
            return myCustomersBl;
        }
        public List<ParcelToList> GetParcelsBl(Predicate<ParcelToList> myPredicate = null)
        {
            List<ParcelToList> myParcelsBl = new();
            List<IDAL.DO.ParcelDal> dalParcels = DalAccess.GetParcelsList().ToList();

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
            return myParcelsBl.FindAll(x => myPredicate == null ? true : myPredicate(x));
        }

        //public List<ParcelToList> GetParcelsWithoutDroneBl()
        //{
        //    List<ParcelToList> myParcelsWithoutDroneBl = new();
        //    myParcelsWithoutDroneBl = GetParcelsBl().TakeWhile(x => x.ParcelStatus == ParcelStatus.Created).ToList();
        //    return myParcelsWithoutDroneBl;
        //}

        //public List<BaseStationToList> GetStationsWithFreeChargeBl()
        //{
        //    List<BaseStationToList> myBaseStationsWithFreeChargeBl = new();
        //    myBaseStationsWithFreeChargeBl = GetBaseStationsBl().TakeWhile(x => x.FreeChargeSlots != 0).ToList();
        //    return myBaseStationsWithFreeChargeBl;
        //}
        
    }
}
