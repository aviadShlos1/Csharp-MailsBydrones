﻿using System;
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
        public List<BaseStationToList> GetBaseStationsList()
        {
            List<BaseStationToList> myBaseStationsBl = new();
            List<IDAL.DO.BaseStationDal> dalBaseStations = DalAccess.GetBaseStationsList().ToList();
            foreach (var item in dalBaseStations)
            {
                int busyChargeSlots = DalAccess.GetDronesChargeList().ToList().FindAll(x => x.StationId == item.Id).Count();
                myBaseStationsBl.Add(new BaseStationToList { Id = item.Id, BaseStationName = item.Name, FreeChargeSlots = item.FreeChargeSlots, BusyChargeSlots=busyChargeSlots });
            }
            return myBaseStationsBl;
        }
        public List<DroneToList> GetDronesList()
        {
            return DronesListBL;
        }
        public List<CustomerToList> GetCustomersList()
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
        public List<ParcelToList> GetParcelsList()
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
            return myParcelsBl;
        }
        public List<ParcelToList> ParcelsWithoutDroneListDisplay()
        {
            List<ParcelToList> parcelsWithoutDrone = new();
            return 
        }
        public void FreeChargeSlotsListDisplay()
        {

        }
        
    }
}
