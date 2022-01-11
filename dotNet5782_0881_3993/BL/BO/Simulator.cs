using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BO;
using DalApi;
using System.Threading.Tasks;
using System.Threading;

namespace BL
{
    class Simulator
    {

        BL blAccess;

        private const double kmh = 3600;//כל קילומטר זה שנייה כי בשעה יש 3600 שניות

        public Simulator(BL bl, int droneId, Action reportProgressInSimulator, Func<bool> isTimeRun)
        {
            IDal dalAccess = DalFactory.GetDal();
            blAccess = bl;
            var dal = blAccess;

            double distance;
            double battery;

            DroneToList myDrone = blAccess.GetDronesBl().First(x => x.DroneId == droneId);

            while (!isTimeRun())
            {
                switch (myDrone.DroneStatus)
                {
                    case DroneStatusesBL.Available:
                        try
                        {
                            blAccess.AssignParcelToDrone(droneId);
                            reportProgressInSimulator();
                        }
                        catch
                        {
                        
                            //if (myDrone.BatteryPercent < 100)
                            //{
                            //    battery = myDrone.BatteryPercent;

                            //    IEnumerable<BaseStation> baseStationBL = (from item in dalAccess.GetBaseStationList()
                            //                                              select new BaseStation()
                            //                                              {
                            //                                                  Id = item.Id,
                            //                                                  Name = item.StationName,
                            //                                                  FreeChargeSlots = item.FreeChargeSlots,
                            //                                                  BaseStationLocation = new Location() { longitude = item.Longitude, latitude = item.Latitude },
                            //                                                  DroneInChargsList = new List<DroneInCharg>()
                            //                                              });

                            //    distance = blAccess.minDistanceBetweenBaseStationsAndLocation(baseStationBL, myDrone.DroneLocation).Item2;

                            //    while (distance > 0)
                            //    {
                            //        myDrone.BatteryPercent -= blAccess.Free;
                            //        reportProgressInSimulator();
                            //        distance -= 1;
                            //        Thread.Sleep(1000);
                            //    }

                            //    myDrone.BatteryPercent = battery;//הפונקציה שליחה לטעינה בודקת בודקת את המרחק ההתחלתי ולפי זה מחשבת את הסוללה ולכן צריך להחזיר למצב ההתחלתי
                                blAccess.DroneToCharge(droneId);
                                reportProgressInSimulator();
                            
                        }
                        break;
                   
                    case DroneStatusesBL.Maintaince:
                        DO.DroneChargeDal chargeDal = dalAccess.GetDronesChargeList().First(x=>x.DroneId==droneId);
                        TimeSpan interval = DateTime.Now - chargeDal.StartChargeTime;
                        double chargeInHours = interval.Hours + (((double)interval.Minutes) / 60) + (((double)interval.Seconds) / 3600);
                        double batteryCharge = chargeInHours * 50 + myDrone.BatteryPercent; //DroneLoadingRate == 50

                        while (batteryCharge < 100)
                        {
                            myDrone.BatteryPercent += 3; // every second increase in 3 percent
                            batteryCharge += 3;
                            if (myDrone.BatteryPercent > 100) // if we pass the 100 percent
                            {
                                myDrone.BatteryPercent = 100;
                            }
                            reportProgressInSimulator();
                            Thread.Sleep(1000);
                        }

                        blAccess.ReleaseDroneCharge(droneId); 
                        break;
                   
                    case DroneStatusesBL.Shipment:
                        DroneBl DroneInProgress = blAccess.GetSingleDrone(droneId); // we create droneBl entity because of the field parcelInShip

                        if (blAccess.GetSingleParcel(DroneInProgress.ParcelInShip.Id).PickingUpTime == null)
                        {
                            battery = myDrone.BatteryPercent;
                            
                            Location myLocation = new Location { Longitude = myDrone.DroneLocation.Longitude, Latitude = myDrone.DroneLocation.Latitude };
                            distance = DroneInProgress.ParcelInShip.ShippingDistance;
                            while (distance > 0)
                            {
                                myDrone.BatteryPercent -= blAccess.freeWeightConsumption;
                                distance -= 1;
                                //locationSteps(DroneInProgress.DroneLocation, blAccess.GetCustomer(DroneInProgress.ParcelInShip.Sender.Id).LocationOfCustomer, DroneInProgress);
                                myDrone.DroneLocation = DroneInProgress.DroneLocation;
                                reportProgressInSimulator();
                                Thread.Sleep(1000);
                            }
                            myDrone.DroneLocation = myLocation; //restart to the source location
                            myDrone.BatteryPercent = battery;
                            blAccess.PickUpParcel(DroneInProgress.DroneId);
                            reportProgressInSimulator();
                        }
                        else // PickedUp != null --- go to supply
                        {
                            battery = myDrone.BatteryPercent;
                            distance = DroneInProgress.ParcelInShip.ShippingDistance;

                            while (distance > 0)
                            {
                                switch (DroneInProgress.ParcelInShip.Weight)
                                {
                                    case WeightCategoriesBL.Light:
                                        myDrone.BatteryPercent -= blAccess.lightWeightConsumption;
                                        break;
                                    case WeightCategoriesBL.Medium:
                                        myDrone.BatteryPercent -= blAccess.mediumWeightConsumption;
                                        break;
                                    case WeightCategoriesBL.Heavy:
                                        myDrone.BatteryPercent -= blAccess.heavyWeightConsumption;
                                        break;
                                    default:
                                        break;
                                }

                                reportProgressInSimulator();
                                distance -= 1;
                                Thread.Sleep(1000);
                            }

                            myDrone.BatteryPercent = battery;
                            blAccess.SupplyParcel(DroneInProgress.DroneId);
                            reportProgressInSimulator();
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationOfDrone">location of drone</param>
        /// <param name="locationOfNextStep">location of next step</param>
        /// <param name="myDrone">drone</param>
        //private void locationSteps(Location locationOfDrone, Location locationOfNextStep, Drone myDrone)
        //{
        //    double droneLatitude = locationOfDrone.latitude;
        //    double droneLongitude = locationOfDrone.longitude;

        //    double nextStepLatitude = locationOfNextStep.latitude;
        //    double nextStepLongitude = locationOfNextStep.longitude;

        //    //Calculate the latitude of the new location.
        //    if (droneLatitude < nextStepLatitude)// ++++++
        //    {
        //        //double step = (nextStepLatitude - droneLatitude) / myDrone.ParcelInShip.TransportDistance;
        //        myDrone.DroneLocation.latitude += (nextStepLatitude - droneLatitude) / myDrone.ParcelInShip.TransportDistance;
        //    }
        //    else
        //    {
        //        //double step = (  droneLatitude - nextStepLatitude) / myDrone.ParcelInShip.TransportDistance;
        //        myDrone.DroneLocation.latitude -= (droneLatitude - nextStepLatitude) / myDrone.ParcelInShip.TransportDistance;
        //    }

        //    //Calculate the Longitude of the new location.
        //    if (droneLongitude < nextStepLongitude)//+++++++
        //    {
        //        // double step = (nextStepLongitude - droneLongitude) / myDrone.ParcelInShip.TransportDistance;
        //        myDrone.DroneLocation.longitude += (nextStepLongitude - droneLongitude) / myDrone.ParcelInShip.TransportDistance;
        //    }
        //    else
        //    {
        //        //double step = (droneLongitude - nextStepLongitude) / myDrone.ParcelInShip.TransportDistance;
        //        myDrone.DroneLocation.longitude -= (droneLongitude - nextStepLongitude) / myDrone.ParcelInShip.TransportDistance;
        //    }
        //}
    }
}
