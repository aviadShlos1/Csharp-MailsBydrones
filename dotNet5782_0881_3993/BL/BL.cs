//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
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
        IDAL.IDal DalAccess = new DalObject.DalObject();
        public List<DroneToList> DronesListBL { get; set; }
        public static Random rand = new();

        public double freeWeightConsumption;
        public double lightWeightConsumption;
        public double mediumWeightConsumption;
        public double heavyWeightConsumption;
        public double chargeRate;

        #region Help methods
        private IDAL.DO.CustomerDal GetCustomerDetails(int id)
        {
            IDAL.DO.CustomerDal myCust = new();

            foreach (var item in DalAccess.GetCustomersList())
            {
                if (item.Id == id)
                    myCust = item;
            }
            return myCust;
        }

        private static double GetDistance(double myLongitude, double myLatitude, double stationLongitude, double stationLatitude)
        {
            var num1 = myLongitude * (Math.PI / 180.0);
            var d1 = myLatitude * (Math.PI / 180.0);
            var d2 = stationLongitude * (Math.PI / 180.0);
            var num2 = stationLatitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0); //https://iw.waldorf-am-see.org/588999-calculating-distance-between-two-latitude-QPAAIP
            double distanceInMeters = (double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));                                                                                                                 //We calculate the distance according to a formula that also takes into account the curvature of the earth
            return distanceInMeters/1000 ; //return ditance in kilometers
        }
        private BaseStationBl ClosetStation(double myLon, double myLat, List<IDAL.DO.BaseStationDal> stationsList)
        {
            BaseStationBl closetBaseStation = default;
            double stationLon = stationsList[0].Longitude;
            double stationLat = stationsList[0].Latitude;
            double closetDistance = GetDistance(myLon, myLat, stationLon, stationLat);
            foreach (var item in stationsList)
            {
                double tempDistance = GetDistance(myLon, myLat, item.Longitude, item.Latitude);
                if (tempDistance <= closetDistance)
                {
                    closetDistance = tempDistance;
                    Location myLoc = new() { Longitude = item.Longitude, Latitude = item.Latitude };
                    closetBaseStation = new() { Id = item.Id, BaseStationName = item.Name, Location = myLoc, FreeChargeSlots = item.FreeChargeSlots };
                }  
            }
            return closetBaseStation;
        }
        //private List<IDAL.DO.CustomerDal> CustomersSuppliedParcels()
        //{
        //    List<IDAL.DO.CustomerDal> temp = new();
        //    foreach (var itemPar in DalAccess.GetParcelsList())
        //    {
        //        foreach (var itemCus in DalAccess.GetCustomersList())
        //        {
        //            if (itemPar.TargetId == itemCus.Id && itemPar.SupplyingTime != DateTime.MinValue)
        //            {
        //                temp.Add(itemCus);
        //            }
        //        }
        //    }
        //    return temp;
        //}
        #endregion
        //ctor
        public BL()
        {
            double[] energyConsumption = DalAccess.EnergyConsumption();
            freeWeightConsumption = energyConsumption[0];
            lightWeightConsumption = energyConsumption[1];
            mediumWeightConsumption = energyConsumption[2];
            heavyWeightConsumption = energyConsumption[3];
            chargeRate = energyConsumption[4];
            
            // initializing the entities lists from the dal layer
            List <DroneDal> DronesDalList = DalAccess.GetDronesList().ToList();
            List <ParcelDal> ParcelsDalList = DalAccess.GetParcelsList().ToList();
            List <BaseStationDal> BaseStationsDalList = DalAccess.GetBaseStationsList().ToList();
            List <CustomerDal> CustomersDalList = DalAccess.GetCustomersList().ToList();
            
            DronesListBL = new List<DroneToList>();
            foreach (var item in DronesDalList)
            {
                DronesListBL.Add(new DroneToList { DroneId = item.Id, Model = item.Model, DroneWeight = (WeightCategoriesBL)item.DroneWeight, DroneLocation = new() });
            }
            foreach (var itemDrone in DronesListBL)
            {

                foreach (var itemParcel in ParcelsDalList)
                {
                    if (itemParcel.DroneToParcelId == itemDrone.DroneId && itemParcel.SupplyingTime == DateTime.MinValue)
                    {
                        itemDrone.DroneStatus = DroneStatus.Shipment;
                        if (itemParcel.AssignningTime != DateTime.MinValue && itemParcel.PickingUpTime == DateTime.MinValue)
                        {
                            double senderLon = GetCustomerDetails(itemParcel.SenderId).CustomerLongitude;
                            double senderLat = GetCustomerDetails(itemParcel.SenderId).CustomerLatitude;
                            itemDrone.DroneLocation = ClosetStation(senderLon, senderLat, DalAccess.GetBaseStationsList().ToList()).Location;
                        }
                        if (itemParcel.PickingUpTime != DateTime.MinValue && itemParcel.SupplyingTime == DateTime.MinValue)
                        {
                            itemDrone.DroneLocation.Longitude = GetCustomerDetails(itemParcel.SenderId).CustomerLongitude;
                            itemDrone.DroneLocation.Latitude = GetCustomerDetails(itemParcel.SenderId).CustomerLatitude;
                        }

                        double targetLon = GetCustomerDetails(itemParcel.TargetId).CustomerLongitude;
                        double targerLat = GetCustomerDetails(itemParcel.TargetId).CustomerLatitude;
                        double targetDistance = GetDistance(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, targetLon, targerLat);
                        double minCharge1 = energyConsumption[(int)itemDrone.DroneWeight+1] * targetDistance;
                        Location closetStation = ClosetStation(itemDrone.DroneLocation.Longitude,itemDrone.DroneLocation.Latitude, DalAccess.GetBaseStationsList().ToList()).Location;
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
                    itemDrone.DroneLocation.Latitude = BaseStationsDalList[index].Latitude;
                    itemDrone.DroneLocation.Longitude = BaseStationsDalList[index].Longitude;
                    itemDrone.BatteryPercent = rand.NextDouble() * 20;
                }
                if (itemDrone.DroneStatus == DroneStatus.Free)
                {

                    List<IDAL.DO.ParcelDal> assignedParcels = ParcelsDalList.FindAll(x => x.DroneToParcelId == itemDrone.DroneId && x.SupplyingTime != DateTime.MinValue);

                    if (assignedParcels.Count != 0) //the list isn't empty 
                    {
                        int index = rand.Next(0, assignedParcels.Count);
                        itemDrone.DroneLocation.Latitude = CustomersDalList.Find(x => x.Id == assignedParcels[index].TargetId).CustomerLatitude;
                        itemDrone.DroneLocation.Longitude = CustomersDalList.Find(x => x.Id == assignedParcels[index].TargetId).CustomerLongitude;
                    }
                    else
                    {
                        IDAL.DO.BaseStationDal temp = BaseStationsDalList[rand.Next(0, BaseStationsDalList.Count)];
                        if (!BaseStationsDalList.Any())
                        {
                            itemDrone.DroneLocation.Latitude = temp.Latitude;
                            itemDrone.DroneLocation.Longitude = temp.Longitude;
                        }
                    }
                    
                    Location closetStation = new Location();
                    closetStation = ClosetStation(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude,BaseStationsDalList).Location;
                    double minCharge = freeWeightConsumption * GetDistance(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude, closetStation.Longitude, closetStation.Latitude);
                    itemDrone.BatteryPercent = rand.NextDouble()*(100-minCharge)+minCharge ;
                }
            }
            
        }
      

        

    }










}

