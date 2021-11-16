//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//Brief: The data layer methods

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject:IDAL.IDal
    {
        public double[] EnergyConsumption()
        {
            double[] ConsumptionArr = new double[5]; 
            ConsumptionArr[0] = DataSource.Config.FreeWeight;
            ConsumptionArr[1] = DataSource.Config.LightWeight;
            ConsumptionArr[2] = DataSource.Config.MediumWeight;
            ConsumptionArr[3] = DataSource.Config.HeavyWeight;
            ConsumptionArr[4] = DataSource.Config.ChargeRate;
            return ConsumptionArr;
        }
        public DalObject() { DataSource.Initialize(); }

        #region Add methods
        /// <summary>
        /// Adding a new object for the all entities
        /// </summary>
        /// <param name="newStation"></param>
        public void AddStation(BaseStation newStation)
        {
            DataSource.Stations.Add(newStation);
        }
        public void AddDrone(Drone newDrone)
        {
            DataSource.Drones.Add(newDrone);
        }
        public void AddCustomer(Customer newCustomer)
        {
            DataSource.Customers.Add(newCustomer);
        }
        public int AddParcel(Parcel newParcel)
        {
            DataSource.Parcels.Add(newParcel);
            newParcel.Id = DataSource.Config.RunId++;
            return newParcel.Id;
        }
        #endregion Add methods

        #region Update methods
        /// <summary>
        /// Assining a drone to a parcel by the parcel and drone id 
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void ConnectDroneToParcel(int parcelId, int droneId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel parcel1 = DataSource.Parcels[parcelIndex];
            parcel1.DroneToParcel_Id = droneId;
            parcel1.Assigned = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel1;
        }
        /// <summary>
        /// Picking up a parcel by the assined drone before, with given the parcel id
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickUpParcel(int parcelId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel parcel2 = DataSource.Parcels[parcelIndex];
            parcel2.PickedUp = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel2;
        }
        /// <summary>
        /// Delivering the parcel to the customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void DelieverParcel(int parcelId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel parcel3 = DataSource.Parcels[parcelIndex];
            parcel3.Supplied = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel3;
        }
        /// <summary>
        /// Sending a drone to charge in the base station, with given the drone and station id
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void DroneToCharge(int droneId, int stationId)
        {
            int stationIndex = DataSource.Stations.FindIndex(i => i.Id == stationId);
            BaseStation station1 = DataSource.Stations[stationIndex];
            station1.ChargeSlots--; // Reducing the free chargeSlots
            DataSource.Stations[stationIndex] = station1;

            DataSource.DroneCharges.Add(new DroneCharge() { DroneId = droneId, StationId = stationId });//initiate a new drone charge

            int chargeIndex = DataSource.DroneCharges.FindIndex(i => i.DroneId==droneId);
            DroneCharge charge1 = DataSource.DroneCharges[chargeIndex];
            DataSource.DroneCharges[chargeIndex] = charge1;
        }
        /// <summary>
        /// Realesing a drone from the charge base station
        /// </summary>
        /// <param name="droneId"></param>
        public void DroneRelease(int droneId)
        {
            int chargeIndex = DataSource.DroneCharges.FindIndex(i => i.DroneId == droneId);
            DroneCharge help = DataSource.DroneCharges[chargeIndex];
            int baseStationId = help.StationId;

            int stationIndex = DataSource.Stations.FindIndex(i => i.Id == baseStationId);
            BaseStation station2 = DataSource.Stations[stationIndex];
            station2.ChargeSlots++;//Increasing the number of the free charge slots
            DataSource.Stations[stationIndex] = station2;

            DataSource.DroneCharges.RemoveAt(DataSource.DroneCharges.FindIndex(x => x.DroneId == droneId));//Remove the drone from the list of the drone charges


        }
        #endregion Update methods

        #region Single display 
        /// <summary>
        /// Displaying the details for a single entity for the 4 methods below 
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>The type of the entity</returns>
        public BaseStation StationDisplay(int stationId)
        {
           return DataSource.Stations.Find(i => i.Id == stationId);    
        }
        public Drone DroneDisplay(int droneId)
        {
            return DataSource.Drones.Find(i => i.Id == droneId);
        }
        public Customer CustomerDisplay(int customerId)
        {
            return DataSource.Customers.Find(i => i.Id == customerId);
        }
        public Parcel ParcelDisplay(int parcelId)
        {
            return DataSource.Parcels.Find(i => i.Id == parcelId);
        }
        #endregion Single display

        #region ListDisplay
        /// <summary>
        /// Displaying the all list for all the entity that chosen
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseStation> StationsListDisplay()
        {
            return DataSource.Stations;
           
        }
        public IEnumerable<Drone> DronesListDisplay()
        {
            return DataSource.Drones;
        }
        public IEnumerable<Customer> CustomersListDisplay()
        {
            return DataSource.Customers;
        }
        public IEnumerable<Parcel> ParcelsListDisplay()
        {
            return DataSource.Parcels;
        }
        /// <summary>
        /// Displaying the parcel without assinged drone
        /// </summary>
        /// <returns>The list of the parcel</returns>
        public IEnumerable<Parcel> ParcelsWithoutDrone()
        {
            return DataSource.Parcels.TakeWhile(i => i.DroneToParcel_Id == 0).ToList();
        }
        /// <summary>
        /// Displaying the list of station with a free charge slots 
        /// </summary>
        /// <returns>The list of station entity</returns>
        public IEnumerable<BaseStation> FreeChargeSlotsList()
        {
            return DataSource.Stations.TakeWhile(i => i.ChargeSlots != 0).ToList();
        }
        #endregion ListDisplay
    }
}
