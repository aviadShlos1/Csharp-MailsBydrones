﻿//Names: Aviad Shlosberg       314960881      
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
    /// <summary>
    /// This class is divided to modules that contains the methods implement of the CRUD options
    /// </summary>
    public partial class BL : IBL
    {
        IDAL.IDal DalAccess = new DalObject.DalObject();//This is the access point from the data layer
        public List<DroneToList> DronesListBL { get; set; }//This list is contains drones of type of "Drone to list" 
        public static Random rand = new();

        public double freeWeightConsumption;
        public double lightWeightConsumption;
        public double mediumWeightConsumption;
        public double heavyWeightConsumption;
        public double chargeRate;

        #region Help methods
        /// <summary>
        /// This function gives the customer details
        /// </summary>
        /// <param name="id"></param>
        /// <returns>entity of customerDal</returns>
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
            var d1 = myLatitude * (Math.PI / 180.0);
            var d2 = stationLatitude * (Math.PI / 180.0);
            var num1 = myLongitude * (Math.PI / 180.0);
            var num2 = stationLongitude * (Math.PI / 180.0);
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin((num2-num1) / 2.0), 2.0); //calculate according to the formula in this site: https://www.movable-type.co.uk/scripts/latlong.html
            double distanceInMeters = (double)(6371000.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));                             
            return distanceInMeters/100000 ; //return ditance in format that matches the battery units
        }
        /// <summary>
        /// This func checks who is the closet station to my location that i gives
        /// </summary>
        /// <param name="myLon">my longitude</param>
        /// <param name="myLat">my latitude</param>
        /// <param name="stationsList">A list of stations to looking for</param>
        /// <returns></returns>
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
        #endregion
        //ctor
        public BL()
        {
            double[] energyConsumption = DalAccess.EnergyConsumption();//Get from the dal layer the array that contains the energy consumption
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
                    //If the parcel not supplied but the drone is already assign
                    if (itemParcel.DroneToParcelId == itemDrone.DroneId && itemParcel.SupplyingTime == null)
                    {
                        itemDrone.DroneStatus = DroneStatus.Shipment;
                        if (itemParcel.AssignningTime != null && itemParcel.PickingUpTime == null)//If the parcel is already assigned but isn't picked up
                        {
                            double senderLon = GetCustomerDetails(itemParcel.SenderId).CustomerLongitude;
                            double senderLat = GetCustomerDetails(itemParcel.SenderId).CustomerLatitude;
                            itemDrone.DroneLocation = ClosetStation(senderLon, senderLat, DalAccess.GetBaseStationsList().ToList()).Location;
                        }
                        if (itemParcel.PickingUpTime != null && itemParcel.SupplyingTime == null)//If the parcel is already picked up but isn't supplied
                        {
                            itemDrone.DroneLocation.Longitude = GetCustomerDetails(itemParcel.SenderId).CustomerLongitude;
                            itemDrone.DroneLocation.Latitude = GetCustomerDetails(itemParcel.SenderId).CustomerLatitude;
                        }

                        double targetLon = GetCustomerDetails(itemParcel.TargetId).CustomerLongitude;
                        double targerLat = GetCustomerDetails(itemParcel.TargetId).CustomerLatitude;
                        double targetDistance = GetDistance(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, targetLon, targerLat);
                        double minCharge1 = energyConsumption[(int)itemDrone.DroneWeight+1] * targetDistance;//The battery consumption that enables the drone to supply the parcel
                        Location closetStation = ClosetStation(itemDrone.DroneLocation.Longitude,itemDrone.DroneLocation.Latitude, DalAccess.GetBaseStationsList().ToList()).Location;
                        double minCharge2 = freeWeightConsumption * GetDistance(itemDrone.DroneLocation.Longitude, itemDrone.DroneLocation.Latitude, closetStation.Longitude, closetStation.Latitude);//The battery consumption that enables to the drone to arrive the station for charge
                        itemDrone.BatteryPercent = Math.Round(rand.NextDouble() * (minCharge1 + minCharge2) + (100 - (minCharge1 + minCharge2)));
                    }
                }
                //If the drone not doing a shipment
                if (itemDrone.DroneStatus != DroneStatus.Shipment)
                {
                    itemDrone.DroneStatus = (DroneStatus)rand.Next(0, 1);
                }
                //If the drone is in maintaince status
                if (itemDrone.DroneStatus == DroneStatus.Maintaince)
                {
                    int index = rand.Next(BaseStationsDalList.Count());
                    itemDrone.DroneLocation.Latitude = BaseStationsDalList[index].Latitude;
                    itemDrone.DroneLocation.Longitude = BaseStationsDalList[index].Longitude;
                    itemDrone.BatteryPercent = rand.NextDouble() * 20;//rand between 0-20 percent
                }
                //If the drone is free
                if (itemDrone.DroneStatus == DroneStatus.Free)
                {
                    //Finding the customers that have supplied parcels
                    List<IDAL.DO.ParcelDal> assignedParcels = ParcelsDalList.FindAll(x => x.DroneToParcelId == itemDrone.DroneId && x.SupplyingTime != null);

                    if (assignedParcels.Count != 0) //the list isn't empty 
                    {
                        int index = rand.Next(0, assignedParcels.Count);
                        itemDrone.DroneLocation.Latitude = CustomersDalList.Find(x => x.Id == assignedParcels[index].TargetId).CustomerLatitude;
                        itemDrone.DroneLocation.Longitude = CustomersDalList.Find(x => x.Id == assignedParcels[index].TargetId).CustomerLongitude;
                    }
                    else
                    {
                        //rand a base station from the list and set the location of the drone in the station
                        IDAL.DO.BaseStationDal temp = BaseStationsDalList[rand.Next(0, BaseStationsDalList.Count)];
                        if (!BaseStationsDalList.Any())
                        {
                            itemDrone.DroneLocation.Latitude = temp.Latitude;
                            itemDrone.DroneLocation.Longitude = temp.Longitude;
                        }
                    }
                    
                    Location closetStation = new Location();
                    closetStation = ClosetStation(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude,BaseStationsDalList).Location;
                    double minCharge = freeWeightConsumption * GetDistance(itemDrone.DroneLocation.Latitude, itemDrone.DroneLocation.Longitude, closetStation.Longitude, closetStation.Latitude);//the minimum charge to enable it going to charge
                    itemDrone.BatteryPercent = Math.Round(rand.NextDouble()*(100-minCharge)+minCharge) ;
                }
            }
            
        }
    }
}

