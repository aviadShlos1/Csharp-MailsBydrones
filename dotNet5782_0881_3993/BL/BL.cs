using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;

namespace IBL
{
    public class BL : IBL
    {

        public List<DroneToList> DronesListBL { get; set; }
        public List<ParcelToList> ParcelsListBL { get; set; }
        public List<BaseStationToList> BaseStationsListBL { get; set; }
        
        public static Random rand = new();
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rand.Next(v.Length));
        }
        public BL()
        {
            IDAL.IDal AccessPoint = new DalObject.DalObject();
            double[] Arr = AccessPoint.EnergyConsumption();
            double FreeWeight = Arr[0];
            double LightWeight = Arr[1];
            double MediumWeight = Arr[2];
            double HeavyWeight = Arr[3];
            double ChargeRate = Arr[4];

            IEnumerable<IDAL.DO.Drone> DronesDalList = AccessPoint.DronesListDisplay();
            DronesListBL = new List<DroneToList>();
            foreach (var item in DronesDalList)
            {
                DronesListBL.Add(new DroneToList { DroneId = item.Id, Model = item.Model, DroneWeight = (WeightCategoriesBL)item.MaxWeight });
            }

            IEnumerable<IDAL.DO.Parcel> ParcelsDalList = AccessPoint.ParcelsListDisplay();
            ParcelsListBL = new List<ParcelToList>();
            foreach (var item in ParcelsDalList)
            {
                ParcelsListBL.Add(new ParcelToList { Id = item.Id, Priority = (PrioritiesBL)item.Priority, Weight = (WeightCategoriesBL)item.Weight });
            }

            IEnumerable<IDAL.DO.BaseStation> BaseStationsDalList = AccessPoint.StationsListDisplay();
            BaseStationsListBL = new List<BaseStationToList>();
            foreach (var item in BaseStationsDalList)
            {
                BaseStationsListBL.Add(new BaseStationToList { Id = item.Id, BaseStationName = item.Name });
            }

            foreach (var item in DronesListBL)
            {
                
                if (item.DroneStatus != DroneStatus.Shipment) 
                {
                    item.DroneStatus = (DroneStatus)rand.Next(0, 1);
                }
                if (item.DroneStatus == DroneStatus.Maintaince)
                {
                   
                }
            }

            foreach (var item in ParcelsListBL)
            {
                if (item.ParcelStatus!=ParcelStatus.Supplied)
                {

                }
            }
        }


        //partial class AddOptions
        //{
        //    public void AddBaseStation() ;
        //    public void AddDrone();
        //    public void AddCustomer();
        //    public void AddParcel();
        //}



        //partial class UpdateOptions

        //{
        //    public void UpdateDroneName();
        //    public void UpdateBaseStationData();
        //    public void UpdateCustomerData();
        //    public void DroneToCharge();
        //    public void ReleaseDroneCharge();
        //    public void AssignParcelToDrone();
        //    public void PickUpParcel();
        //    public void SupplyParcel();
        //}



        //partial class DisplayOptions
        //{
        //public void BaseStationDisplay();
        //public void DroneDisplay();
        //public void CustomerDisplay();
        //public void ParcelDisplay();
        //}


        //partial class ListDisplayOptions
        //{
        //public void BaseStationsListDisplay();
        //public void DronesListDisplay();
        //public void CustomersListDisplay();
        //public void ParcelsListDisplay();
        //public void ParcelsWithoutDroneListDisplay();
        //public void FreeChargeSlotsListDisplay();
        //}
    }










}

