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

            foreach (var item in DalAccess.GetCustomersList())
            {
                if (item.Id == id)
                    myCust = item;
            }
            return myCust;
        }
        private List<IDAL.DO.Customer> CustomersSuppliedParcels()
        {
            List<IDAL.DO.Customer> temp = new();
            foreach (var itemPar in DalAccess.GetParcelsList())
            {
                foreach (var itemCus in DalAccess.GetCustomersList())
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
        private BaseStationBL ClosetStation(double myLon, double myLat, List<IDAL.DO.BaseStation> stationsList )
        { 
            double stationLon = stationsList[0].Longitude;
            double stationLat = stationsList[0].Latitude;
            double closetDistance = GetDistance(myLon, myLat, stationLon, stationLat);
            foreach (var item in stationsList)
            {
                double tempDistance = GetDistance(myLon, myLat, item.Longitude, item.Latitude);
                if (tempDistance < closetDistance)
                {
                    closetDistance = tempDistance;
                    stationLon = item.Longitude;
                    stationLat = item.Latitude;
                }
            }

            BaseStationBL closetBaseStation = new();
            closetBaseStation.BaseStationLocation.Longitude = stationLon;
            closetBaseStation.BaseStationLocation.Latitude = stationLat;
            return closetBaseStation;
        }
        #endregion
        //ctor
        public BL()
        {
            IDAL.IDal DalAccess = new DalObject.DalObject();
            double[] energyConsumption = DalAccess.EnergyConsumption();
            double freeWeightConsumption = energyConsumption[0];
            double lightWeightConsumption = energyConsumption[1];
            double mediumWeightConsumption = energyConsumption[2];
            double heavyWeightConsumption = energyConsumption[3];
            double chargeRate = energyConsumption[4];

            IEnumerable<IDAL.DO.Drone> DronesDalList = DalAccess.GetDronesList();
            IEnumerable<IDAL.DO.Parcel> ParcelsDalList = DalAccess.GetParcelsList();
            IEnumerable<IDAL.DO.BaseStation> BaseStationsDalList = DalAccess.GetBaseStationsList();
            IEnumerable<IDAL.DO.Customer> CustomersDalList = DalAccess.GetCustomersList();
            DronesListBL = new List<DroneToList>();
            foreach (var item in DronesDalList)
            {
                DronesListBL.Add(new DroneToList { DroneId = item.Id, Model = item.Model, DroneWeight = (WeightCategoriesBL)item.DroneWeight });
            }
            foreach (var itemDrone in DronesListBL)
            {

                foreach (var itemParcel in ParcelsDalList)
                {
                    if (itemParcel.DroneToParcel_Id == itemDrone.DroneId && itemParcel.Supplied == DateTime.MinValue)
                    {
                        itemDrone.DroneStatus = DroneStatus.Shipment;
                        if (itemParcel.Assigned != DateTime.MinValue && itemParcel.PickedUp == DateTime.MinValue)
                        {
                            double senderLon = GetCustomer(itemParcel.SenderId).CustomerLongitude;
                            double senderLat = GetCustomer(itemParcel.SenderId).CustomerLatitude;
                            itemDrone.DroneLocation = ClosetStation(senderLon, senderLat, DalAccess.GetBaseStationsList().ToList()).BaseStationLocation;
                        }
                        if (itemParcel.PickedUp != DateTime.MinValue && itemParcel.Supplied == DateTime.MinValue)
                        {
                            itemDrone.DroneLocation.Longitude = GetCustomer(itemParcel.SenderId).CustomerLongitude;
                            itemDrone.DroneLocation.Latitude = GetCustomer(itemParcel.SenderId).CustomerLatitude;
                        }

                        double targetLon = GetCustomer(itemParcel.TargetId).CustomerLongitude;
                        double targerLat = GetCustomer(itemParcel.TargetId).CustomerLatitude;
                        double targetDistance = GetDistance(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, targetLon, targerLat);
                        double minCharge1 = energyConsumption[(int)itemDrone.DroneWeight+1] * targetDistance;
                        Location closetStation = ClosetStation(itemDrone.DroneLocation.Longitude,itemDrone.DroneLocation.Latitude, DalAccess.GetBaseStationsList().ToList()).BaseStationLocation;
                        double minCharge2 = freeWeightConsumption * GetDistance(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, closetStation.Longitude, closetStation.Latitude);
                        itemDrone.BatteryPercent = rand.NextDouble() * (100 - (minCharge1+minCharge2)) + minCharge1+minCharge2;
                    }
                }
                if (itemDrone.DroneStatus != DroneStatus.Shipment)
                {
                    itemDrone.DroneStatus = (DroneStatus)rand.Next(0, 1);
                }
                if (itemDrone.DroneStatus == DroneStatus.Maintaince)
                {
                    int index = rand.Next(BaseStationsDalList.Count());
                    itemDrone.DroneLocation.Latitude = BaseStationsDalList.ToList()[index].Latitude;
                    itemDrone.DroneLocation.Longitude = BaseStationsDalList.ToList()[index].Longitude;
                    itemDrone.BatteryPercent = rand.NextDouble() * 20;
                }
                if (itemDrone.DroneStatus == DroneStatus.Free)
                {
                    List<IDAL.DO.Customer> custSupplied = CustomersSuppliedParcels();
                    int index = rand.Next(custSupplied.Count());
                    itemDrone.DroneLocation.Latitude = custSupplied.ToList()[index].CustomerLatitude;
                    itemDrone.DroneLocation.Longitude = custSupplied.ToList()[index].CustomerLongitude;
                    Location closetStation = ClosetStation(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude, DalAccess.GetBaseStationsList().ToList()).BaseStationLocation;
                    double minCharge = freeWeightConsumption * GetDistance(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude, closetStation.Longitude, closetStation.Latitude);
                    itemDrone.BatteryPercent = rand.NextDouble()*(100-minCharge)+minCharge ;
                }
            }
        }
        //partial class DisplayOptions
        //{
        //public void GetBaseStation();
        //public void GetDrone();
        //public void GetCustomer();
        //public void GetParcel();
        //}


        //partial class ListDisplayOptions
        //{
        //public void GetBaseStationsList();
        //public void GetDronesList();
        //public void GetCustomersList();
        //public void GetParcelsList();
        //public void ParcelsWithoutDroneListDisplay();
        //public void FreeChargeSlotsListDisplay();
        //}

    }










}

