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

        private const double kmh = 3600;

        public Simulator(BL bl, int droneId, Action reportProgressInSimulator, Func<bool> isTimeRun)
        {
            IDal dalAccess = DalFactory.GetDal();
            var dal = blAccess;
            double distance, battery;

            DroneToList myDrone = blAccess.GetDronesBl().First(x => x.DroneId == droneId);

            while (!isTimeRun()) //while the cancellation wasn't executed
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
                            if (myDrone.BatteryPercent<100)
                            {
                                blAccess.DroneToCharge(droneId);
                                reportProgressInSimulator();
                            }
                        }
                        break;

                    case DroneStatusesBL.Maintaince:
                     
                        while (myDrone.BatteryPercent < 100)
                        {
                            myDrone.BatteryPercent += 3; // every second increase in 3 percent
                            if (myDrone.BatteryPercent > 100) // if we pass the 100 percent
                            {
                                myDrone.BatteryPercent = 100;
                            }
                            reportProgressInSimulator();
                            Thread.Sleep(500);
                        }

                        blAccess.ReleaseDroneCharge(droneId);
                        reportProgressInSimulator();
                        break;

                    case DroneStatusesBL.Shipment:
                        DroneBl DroneInProgress = blAccess.GetSingleDrone(droneId); // we create droneBl entity because of the needed field parcelInShip

                        if (blAccess.GetSingleParcel(DroneInProgress.ParcelInShip.Id).PickingUpTime == null)
                        {
                            battery = myDrone.BatteryPercent;
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
