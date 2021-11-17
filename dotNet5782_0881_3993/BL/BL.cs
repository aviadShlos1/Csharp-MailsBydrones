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
        
        public static Random rand = new();
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rand.Next(v.Length));
        }
        public BL()
        {
            IDAL.IDal DalAccess = new DalObject.DalObject();
            double[] Arr = DalAccess.EnergyConsumption();
            double FreeWeight = Arr[0];
            double LightWeight = Arr[1];
            double MediumWeight = Arr[2];
            double HeavyWeight = Arr[3];
            double ChargeRate = Arr[4];

            IEnumerable<IDAL.DO.Drone> DronesDalList = DalAccess.DronesListDisplay();
            DronesListBL = new List<DroneToList>();
            foreach (var item in DronesDalList)
            {
                DronesListBL.Add(new DroneToList { DroneId = item.Id, Model = item.Model, DroneWeight = (WeightCategoriesBL)item.MaxWeight });
            }

            IEnumerable<IDAL.DO.Parcel> ParcelsDalList = DalAccess.ParcelsListDisplay();
            IEnumerable<IDAL.DO.BaseStation> BaseStationsDalList = DalAccess.StationsListDisplay();
            IEnumerable<IDAL.DO.Customer> CustomersDalList = DalAccess.CustomersListDisplay();

            foreach (var drone in DronesListBL)
            {

                foreach (var parcel in ParcelsDalList)
                {
                    if (parcel.DroneToParcel_Id == drone.DroneId && parcel.Supplied == DateTime.MinValue) 
                    {
                        drone.DroneStatus = DroneStatus.Shipment;
                        if (parcel.Assigned != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                            drone.DroneLocation = SenderClosetStation(BaseStationsDalList);
                        if(parcel.PickedUp != DateTime.MinValue && parcel.Supplied == DateTime.MinValue)
                            drone.DroneLocation.Latitude = 

                    }
                }
                if (drone.DroneStatus != DroneStatus.Shipment)
                {
                    drone.DroneStatus = (DroneStatus)rand.Next(0, 1);
                }
                if (drone.DroneStatus == DroneStatus.Maintaince)
                {
                    drone.DroneLocation =
                    }
            }

            foreach (var item in ParcelsDalList)
            {
                if (item!=ParcelStatus.Supplied)
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

