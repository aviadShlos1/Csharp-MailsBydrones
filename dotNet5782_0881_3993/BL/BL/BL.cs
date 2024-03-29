﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: In this program we added xml data files
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DalApi;
using BO;
using DO;

namespace BL
{
    /// <summary>
    /// This class is divided to modules that contains the methods implement of the CRUD options
    /// </summary>
    partial class BL : IBL
    {
        /// <summary>
        /// Singleton definition to ensure the uniqueness of an object 
        /// </summary>
        #region Singelton

        static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }

        internal IDal DalAccess = DalFactory.GetDal();
        #endregion
        public List<DroneToList> DronesListBL;//This list contains drones of type of "Drone to list" 

        public double freeWeightConsumption;
        public double lightWeightConsumption;
        public double mediumWeightConsumption;
        public double heavyWeightConsumption;
        public double chargeRate;

        public Random rand = new();

        #region Help methods
        /// <summary>
        /// This function gives the customer details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>entity of customerDal</returns>
        private DO.CustomerDal GetCustomerDetails(int id)
        {
            DO.CustomerDal myCust = new();

            foreach (var item in DalAccess.GetCustomersList())
            {
                if (item.Id == id)
                    myCust = item;
            }
            return myCust;
        }
        /// <summary>
        /// The function get the distance between two locations
        /// </summary>
        /// <param name="myLongitude"></param>
        /// <param name="myLatitude"></param>
        /// <param name="stationLongitude"></param>
        /// <param name="stationLatitude"></param>
        /// <returns>The distance in double number</returns>
        private static double GetDistance(double myLongitude, double myLatitude, double stationLongitude, double stationLatitude)
        {
            //For the calculation we calculate the earth into a circle (ellipse) Divide its 360 degrees by half
            //180 for each longitude / latitude and then make a pie on each half to calculate the radius for
            //the formula below
            var num1 = myLongitude * (Math.PI / 180.0);
            var d1 = myLatitude * (Math.PI / 180.0);
            var num2 = stationLongitude * (Math.PI / 180.0) - num1;
            var d2 = stationLatitude * (Math.PI / 180.0);

            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0); //calculate according to the formula in this site: https://www.movable-type.co.uk/scripts/latlong.html
            return ((double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))))) / 3000;
        }

        /// <summary>
        /// This func checks who is the closet station to my location that i gives
        /// </summary>
        /// <param name="myLon">my longitude</param>
        /// <param name="myLat">my latitude</param>
        /// <param name="stationsList">A list of stations to looking for</param>
        /// <returns></returns>
        private static BaseStationBl ClosetStation(double myLon, double myLat, List<DO.BaseStationDal> stationsList)
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

        private static double UpToTwoDecimalPoints(double num)
        {
            var total = Convert.ToDouble(string.Format("{0:0.00}", num));
            return total;
        }
        #endregion

        //ctor
        private BL()
        {
            double[] energyConsumption = DalAccess.EnergyConsumption();//Get from the dal layer the array that contains the energy consumption
            freeWeightConsumption = energyConsumption[0];
            lightWeightConsumption = energyConsumption[1];
            mediumWeightConsumption = energyConsumption[2];
            heavyWeightConsumption = energyConsumption[3];
            chargeRate = energyConsumption[4];

            // initializing the entities lists from the dal layer
            List<DroneDal> DronesDalList = DalAccess.GetDronesList().ToList();
            List<ParcelDal> ParcelsDalList = DalAccess.GetParcelsList().ToList();
            List<BaseStationDal> BaseStationsDalList = DalAccess.GetBaseStationsList().ToList();
            List<CustomerDal> CustomersDalList = DalAccess.GetCustomersList().ToList();

            DronesListBL = new List<DroneToList>();
            // first, the function copies the dal drone details  
            foreach (var item in DronesDalList)
            {
                DronesListBL.Add(new DroneToList { DroneId = item.Id, Model = item.Model, DroneWeight = (WeightCategoriesBL)item.DroneWeight, DroneLocation = new() });
            }
            // secondly, through the ctor we will add the new details: battery percent, status and location.
            foreach (var itemDrone in DronesListBL)
            {
                foreach (var itemParcel in ParcelsDalList)
                {
                    //If the parcel was not supplied but the drone has already assigned
                    if (itemParcel.DroneToParcelId == itemDrone.DroneId && itemParcel.SupplyingTime == null)
                    {
                        itemDrone.DroneStatus = DroneStatusesBL.Shipment;
                        itemDrone.ParcelAssignedId = itemParcel.Id;

                        //If the parcel is already assigned but isn't picked up
                        if (itemParcel.AssignningTime != null && itemParcel.PickingUpTime == null)
                        {
                            double senderLon = GetCustomerDetails(itemParcel.SenderId).Longitude;
                            double senderLat = GetCustomerDetails(itemParcel.SenderId).Latitude;
                            itemDrone.DroneLocation = ClosetStation(senderLon, senderLat, DalAccess.GetBaseStationsList().ToList()).Location;
                        }

                        //If the parcel is already picked up but isn't supplied
                        if (itemParcel.PickingUpTime != null && itemParcel.SupplyingTime == null)
                        {
                            itemDrone.DroneLocation.Longitude = GetCustomerDetails(itemParcel.SenderId).Longitude;
                            itemDrone.DroneLocation.Latitude = GetCustomerDetails(itemParcel.SenderId).Latitude;
                        }
                        //target - the reciever location
                        double targetLon = GetCustomerDetails(itemParcel.TargetId).Longitude;
                        double targerLat = GetCustomerDetails(itemParcel.TargetId).Latitude;
                        double targetDistance = GetDistance(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, targetLon, targerLat);
                        double minCharge1 = energyConsumption[(int)itemDrone.DroneWeight + 1] * targetDistance;//The battery consumption that enables the drone to supply the parcel
                        Location closetStation = ClosetStation(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, DalAccess.GetBaseStationsList().ToList()).Location;
                        double minCharge2 = freeWeightConsumption * GetDistance(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, closetStation.Longitude, closetStation.Latitude);//The battery consumption that enables to the drone to arrive the station for charge
                        itemDrone.BatteryPercent = UpToTwoDecimalPoints(rand.NextDouble() * (minCharge1 + minCharge2) + (100 - (minCharge1 + minCharge2)));
                    }
                }
                //If the drone isn't in a shipment
                if (itemDrone.DroneStatus != DroneStatusesBL.Shipment)
                {
                    itemDrone.DroneStatus = (DroneStatusesBL)rand.Next(0, 2);
                }
                //If the drone is in maintaince status
                if (itemDrone.DroneStatus == DroneStatusesBL.Maintaince)
                {
                    int index = rand.Next(BaseStationsDalList.Count());
                    itemDrone.DroneLocation.Latitude = BaseStationsDalList[index].Latitude;
                    itemDrone.DroneLocation.Longitude = BaseStationsDalList[index].Longitude;
                    itemDrone.BatteryPercent = UpToTwoDecimalPoints(rand.NextDouble() * 20);//rand between 0-20 percent
                    DalAccess.SendDroneToCharge(itemDrone.DroneId, BaseStationsDalList[index].Id);
                }

                //If the drone is free
                if (itemDrone.DroneStatus == DroneStatusesBL.Available)
                {
                    //Finding the customers that have supplied parcels
                    List<DO.ParcelDal> suppliedParcels = ParcelsDalList.FindAll(x => x.SupplyingTime != null);
                    List<DO.CustomerDal> suppliedCustomers = new();
                    foreach (var item in DalAccess.GetCustomersList())
                    {
                        foreach (var item2 in suppliedParcels)
                        {
                            if (item.Id == item2.TargetId)
                                suppliedCustomers.Add(item);
                        }
                    }
                    if (suppliedCustomers.Count != 0) //the list isn't empty 
                    {
                        int index = rand.Next(0, suppliedCustomers.Count);
                        itemDrone.DroneLocation.Latitude = suppliedCustomers[index].Latitude;
                        itemDrone.DroneLocation.Longitude = suppliedCustomers[index].Longitude;
                    }
                    else
                    {
                        //rand a base station from the list and set the location of the drone in the station
                        DO.BaseStationDal temp = BaseStationsDalList[rand.Next(0, BaseStationsDalList.Count)];
                        itemDrone.DroneLocation.Latitude = temp.Latitude;
                        itemDrone.DroneLocation.Longitude = temp.Longitude;
                    }
                    Location closetStation = new Location();
                    closetStation = ClosetStation(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude, BaseStationsDalList).Location;
                    double minCharge = freeWeightConsumption * GetDistance(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude, closetStation.Longitude, closetStation.Latitude);//the minimum charge to enable it going to charge
                    itemDrone.BatteryPercent = UpToTwoDecimalPoints(rand.NextDouble() * (100 - minCharge) + minCharge);
                }
            }

        }
    }
}

