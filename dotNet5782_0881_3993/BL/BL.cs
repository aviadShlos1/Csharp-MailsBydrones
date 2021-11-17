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
        //public static T RandomEnumValue<T>()
        //{
        //    var v = Enum.GetValues(typeof(T));
        //    return (T)v.GetValue(rand.Next(v.Length));
        //}


        public IDAL.DO.Customer GetCustomer(int id , IEnumerable<IDAL.DO.Customer> customers )
        {
            IDAL.DO.Customer myCust = new();

            foreach (var item in customers)
            {

                if (item.Id == id)
                    myCust = item;  
            }
            return myCust;
        }
        public List<IDAL.DO.Customer> CustomersSuppliedParcels(IEnumerable<IDAL.DO.Parcel> tempParcels, IEnumerable<IDAL.DO.Customer> tempCustomers)
        {
            List<IDAL.DO.Customer> temp = new();
            foreach (var itemPar in tempParcels)
            {
                foreach (var itemCus in tempCustomers)
                {
                    if (itemPar.TargetId == itemCus.Id && itemPar.Supplied !=DateTime.MinValue) 
                    {
                        temp.Add(itemCus);
                    }
                }
            }
            return temp;
        }
        public IDAL.DO.BaseStation SenderClosetStation(,IEnumerable<IDAL.DO.BaseStation>)
        {

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
            IEnumerable<IDAL.DO.Parcel> ParcelsDalList = DalAccess.ParcelsListDisplay();
            IEnumerable<IDAL.DO.BaseStation> BaseStationsDalList = DalAccess.StationsListDisplay();
            IEnumerable<IDAL.DO.Customer> CustomersDalList = DalAccess.CustomersListDisplay();
            DronesListBL = new List<DroneToList>();


            foreach (var item in DronesDalList)
            {
                DronesListBL.Add(new DroneToList { DroneId = item.Id, Model = item.Model, DroneWeight = (WeightCategoriesBL)item.MaxWeight });
            }

         

            foreach (var drone in DronesListBL)
            {

                foreach (var parcel in ParcelsDalList)
                {
                    if (parcel.DroneToParcel_Id == drone.DroneId && parcel.Supplied == DateTime.MinValue) 
                    {
                        drone.DroneStatus = DroneStatus.Shipment;
                        if (parcel.Assigned != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                            drone.DroneLocation = SenderClosetStation(BaseStationsDalList);
                        if (parcel.PickedUp != DateTime.MinValue && parcel.Supplied == DateTime.MinValue)
                        {
                            drone.DroneLocation.Longitude = GetCustomer(parcel.SenderId, CustomersDalList).CustomerLongitude;
                            drone.DroneLocation.Latitude = GetCustomer(parcel.SenderId, CustomersDalList).CustomerLatitude;
                        }
                            //drone.DroneLocation = CustomersDalList

                    }
                }
                if (drone.DroneStatus != DroneStatus.Shipment)
                {
                    drone.DroneStatus = (DroneStatus)rand.Next(0, 1);
                }
                if (drone.DroneStatus == DroneStatus.Maintaince)
                {
                    int index = rand.Next(BaseStationsDalList.Count());
                    drone.DroneLocation.Latitude = BaseStationsDalList.ToList()[index].Latitude;
                    drone.DroneLocation.Longitude = BaseStationsDalList.ToList()[index].Longitude;
                    drone.BatteryPercent = rand.NextDouble() * 20;
                }
                if (drone.DroneStatus == DroneStatus.Free) 
                {
                    List<IDAL.DO.Customer> custSupplied = CustomersSuppliedParcels(ParcelsDalList, CustomersDalList);
                    int index = rand.Next(custSupplied.Count());
                    drone.DroneLocation.Latitude = custSupplied.ToList()[index].CustomerLatitude;
                    drone.DroneLocation.Longitude = custSupplied.ToList()[index].CustomerLongitude;
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

