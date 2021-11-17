using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;


namespace IBL
{
    public partial class BL : IBL
    {
        public IDAL.IDal DalAccess;
        public List<DroneToList> DronesListBL { get; set; }
        public static Random rand = new();

        #region Help methods
        private IDAL.DO.Customer GetCustomer(int id)
        {
            IDAL.DO.Customer myCust = new();

            foreach (var item in DalAccess.CustomersListDisplay())
            {

                if (item.Id == id)
                    myCust = item;
            }
            return myCust;
        }
        private List<IDAL.DO.Customer> CustomersSuppliedParcels()
        {
            List<IDAL.DO.Customer> temp = new();
            foreach (var itemPar in DalAccess.ParcelsListDisplay())
            {
                foreach (var itemCus in DalAccess.CustomersListDisplay())
                {
                    if (itemPar.TargetId == itemCus.Id && itemPar.Supplied != DateTime.MinValue)
                    {
                        temp.Add(itemCus);
                    }
                }
            }
            return temp;
        }
        private static double GetDistance(double myLongitude, double myLatitude, double stationLongitude, double stationLatitude)
        {
            var num1 = myLongitude * (Math.PI / 180.0);
            var d1 = myLatitude * (Math.PI / 180.0);
            var d2 = stationLongitude * (Math.PI / 180.0);
            var num2 = stationLatitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0); //https://iw.waldorf-am-see.org/588999-calculating-distance-between-two-latitude-QPAAIP
                                                                                                                                   //We calculate the distance according to a formula that also takes into account the curvature of the earth
            return (double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }
        private Location SenderClosetStation(int senderId)
        {
            List<IDAL.DO.BaseStation> stations = DalAccess.BaseStationsListDisplay().ToList();
            double myLon = GetCustomer(senderId).CustomerLongitude; //20 
            double myLat = GetCustomer(senderId).CustomerLatitude; //30 
            double stationLon = stations[0].Longitude;
            double stationLat = stations[0].Latitude;
            double closetDistance = GetDistance(myLon, myLat, stationLon, stationLat);
            foreach (var item in stations)
            {
                double tempDis = GetDistance(myLon, myLat, item.Longitude, item.Latitude);
                if (tempDis < closetDistance)
                {
                    closetDistance = tempDis;
                    stationLon = item.Longitude;
                    stationLat = item.Latitude;
                }
            }

            Location closetLocation = new();
            closetLocation.Longitude = stationLon;
            closetLocation.Latitude = stationLat;
            return closetLocation;

        }
        #endregion
        //ctor
        public BL()
        {
            IDAL.IDal DalAccess = new DalObject.DalObject();
            double[] energyConsumption = DalAccess.EnergyConsumption();
            double FreeWeight = energyConsumption[0];
            double LightWeight = energyConsumption[1];
            double MediumWeight = energyConsumption[2];
            double HeavyWeight = energyConsumption[3];
            double ChargeRate = energyConsumption[4];

            IEnumerable<IDAL.DO.Drone> DronesDalList = DalAccess.DronesListDisplay();
            IEnumerable<IDAL.DO.Parcel> ParcelsDalList = DalAccess.ParcelsListDisplay();
            IEnumerable<IDAL.DO.BaseStation> BaseStationsDalList = DalAccess.BaseStationsListDisplay();
            IEnumerable<IDAL.DO.Customer> CustomersDalList = DalAccess.CustomersListDisplay();
            DronesListBL = new List<DroneToList>();


            foreach (var item in DronesDalList)
            {
                DronesListBL.Add(new DroneToList { DroneId = item.Id, Model = item.Model, DroneWeight = (WeightCategoriesBL)item.DroneWeight });
            }



            foreach (var drone in DronesListBL)
            {

                foreach (var parcel in ParcelsDalList)
                {
                    if (parcel.DroneToParcel_Id == drone.DroneId && parcel.Supplied == DateTime.MinValue)
                    {
                        drone.DroneStatus = DroneStatus.Shipment;
                        if (parcel.Assigned != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                        {
                            drone.DroneLocation = SenderClosetStation(parcel.SenderId);
                        }
                        if (parcel.PickedUp != DateTime.MinValue && parcel.Supplied == DateTime.MinValue)
                        {
                            drone.DroneLocation.Longitude = GetCustomer(parcel.SenderId).CustomerLongitude;
                            drone.DroneLocation.Latitude = GetCustomer(parcel.SenderId).CustomerLatitude;
                        }
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
                    List<IDAL.DO.Customer> custSupplied = CustomersSuppliedParcels();
                    int index = rand.Next(custSupplied.Count());
                    drone.DroneLocation.Latitude = custSupplied.ToList()[index].CustomerLatitude;
                    drone.DroneLocation.Longitude = custSupplied.ToList()[index].CustomerLongitude;
                }
                //
            }

        }



        public void AddBaseStation(int myId, string myBaseStationName, Location myBaseStationLocation, int myFreeChargeSlots = 0)
        {
            BaseStation tempBase = new();
            tempBase.Id = myId;
            tempBase.Name = myBaseStationName;
            tempBase.Latitude = myBaseStationLocation.Latitude;
            tempBase.Longitude = myBaseStationLocation.Longitude;
            tempBase.FreeChargeSlots = myFreeChargeSlots;
            DalAccess.BaseStationsListDisplay().ToList().Add(tempBase);
            List<DroneInCharge> DronesInChargeList = null;
        }
        public void AddDrone(int myDroneId, string myModel, WeightCategoriesBL myDroneWeight, int myBaseStationId)
        {
            Drone tempDrone = new();
            tempDrone.Id = myDroneId;
            tempDrone.Model = myModel;
            tempDrone.DroneWeight = (WeightCategoriesDal)myDroneWeight;
            foreach (var item in DalAccess.BaseStationsListDisplay())
            {
                if (item.Id == myBaseStationId)
                {
                    tempDrone.Longitude = item.Longitude;
                    tempDrone.CurrentLocation.Latitude = item.Latitude;
                }
            }
            tempDrone.BatteryPercent = (rand.NextDouble() * 20) + 20;
            tempDrone.DroneStatus = DroneStatus.Maintaince;
        }

        public void AddCustomer(int myId, string myName, int myPhone, Location myCustLocation)
        {
            CustomerBL tempCust = new();
            tempCust.CustomerId = myId;
            tempCust.CustomerName = myName;
            tempCust.Phone = myPhone;
            tempCust.CustomerLocation = myCustLocation;
        }
        public void AddParcel(int senderId, int recieverId, WeightCategoriesBL myParcelWeight, PrioritiesBL myPriority)
        {
            ParcelBL tempParcel = new();
            tempParcel.Sender.Id = senderId;
            tempParcel.Reciever.Id = recieverId;
            tempParcel.ParcelWeight = myParcelWeight;
            tempParcel.Priority = myPriority;
            tempParcel.CreatingTime = DateTime.Now;
            tempParcel.AssignningTime = DateTime.MinValue;
            tempParcel.PickingUpTime = DateTime.MinValue;
            tempParcel.SupplyingTime = DateTime.MinValue;
            tempParcel.DroneAssignToParcel = null;
        }




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

