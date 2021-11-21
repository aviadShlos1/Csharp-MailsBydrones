using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
using DalObject;

namespace IDAL
{
    public interface IDal
    {
        public double[] EnergyConsumption()
        {
            double[] ConsumptionArr = new double[5];
            ConsumptionArr[0] = DataSource.Config.FreeWeightConsumption;
            ConsumptionArr[1] = DataSource.Config.LightWeightConsumption;
            ConsumptionArr[2] = DataSource.Config.MediumWeightConsumption;
            ConsumptionArr[3] = DataSource.Config.HeavyWeightConsumption;
            ConsumptionArr[4] = DataSource.Config.ChargeRate;
            return ConsumptionArr;
        }
     
 
        #region Add methods
        /// <summary>
        /// Adding a new object for the all entities
        /// </summary>
        /// <param name="newStation"></param>
        public void AddStation(BaseStation newStation)
        {
            DataSource.BaseStations.Add(newStation);
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
            parcel1.DroneToParcelId = droneId;
            parcel1.AssignningTime = DateTime.Now;
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
            parcel2.PickingUpTime = DateTime.Now;
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
            parcel3.SupplyingTime = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel3;
        }
        /// <summary>
        /// Sending a drone to charge in the base station, with given the drone and station id
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void DroneToCharge(int droneId, int stationId)
        {
            int stationIndex = DataSource.BaseStations.FindIndex(i => i.Id == stationId);
            BaseStation station1 = DataSource.BaseStations[stationIndex];
            station1.FreeChargeSlots--; // Reducing the free chargeSlots
            DataSource.BaseStations[stationIndex] = station1;

            DataSource.DronesInCharge.Add(new DroneCharge() { DroneId = droneId, StationId = stationId });//initiate a new drone charge

            int chargeIndex = DataSource.DronesInCharge.FindIndex(i => i.DroneId == droneId);
            DroneCharge charge1 = DataSource.DronesInCharge[chargeIndex];
            DataSource.DronesInCharge[chargeIndex] = charge1;
        }
        /// <summary>
        /// Realesing a drone from the charge base station
        /// </summary>
        /// <param name="droneId"></param>
        public void DroneRelease(int droneId)
        {
            int chargeIndex = DataSource.DronesInCharge.FindIndex(i => i.DroneId == droneId);
            DroneCharge help = DataSource.DronesInCharge[chargeIndex];
            int baseStationId = help.StationId;

            int stationIndex = DataSource.BaseStations.FindIndex(i => i.Id == baseStationId);
            BaseStation station2 = DataSource.BaseStations[stationIndex];
            station2.FreeChargeSlots++;//Increasing the number of the free charge slots
            DataSource.BaseStations[stationIndex] = station2;

            DataSource.DronesInCharge.RemoveAt(DataSource.DronesInCharge.FindIndex(x => x.DroneId == droneId));//Remove the drone from the list of the drone charges


        }
        #endregion Update methods

        #region Single display 
        /// <summary>
        /// Displaying the details for a single entity for the 4 methods below 
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>The type of the entity</returns>
        public BaseStation GetBaseStation(int stationId)
        {
            return DataSource.BaseStations.Find(i => i.Id == stationId);
        }
        public Drone GetDrone(int droneId)
        {
            return DataSource.Drones.Find(i => i.Id == droneId);
        }
        public Customer GetCustomer(int customerId)
        {
            return DataSource.Customers.Find(i => i.Id == customerId);
        }
        public Parcel GetParcel(int parcelId)
        {
            return DataSource.Parcels.Find(i => i.Id == parcelId);
        }
        #endregion Single display

        #region ListDisplay
        /// <summary>
        /// Displaying the all list for all the entity that chosen
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseStation> GetBaseStationsList()
        {
            return DataSource.BaseStations;
            //return DataSource.BaseStations.Take(DataSource.BaseStations.Count).ToList();
        }
        public IEnumerable<Drone> GetDronesList()
        {
            return DataSource.Drones;
        }
        public IEnumerable<Customer> GetCustomersList()
        {
            return DataSource.Customers;
        }
        public IEnumerable<Parcel> GetParcelsList()
        {
            return DataSource.Parcels;
        }
        /// <summary>
        /// Displaying the parcel without assinged drone
        /// </summary>
        /// <returns>The list of the parcel</returns>
        public IEnumerable<Parcel> GetParcelsWithoutDrone()
        {
            return DataSource.Parcels.TakeWhile(i => i.DroneToParcelId == 0).ToList();
        }
        /// <summary>
        /// Displaying the list of station with a free charge slots 
        /// </summary>
        /// <returns>The list of station entity</returns>
        public IEnumerable<BaseStation> GetStationsWithFreeCharge()
        {
            return DataSource.BaseStations.TakeWhile(i => i.FreeChargeSlots != 0).ToList();
        }
        public IEnumerable<DroneCharge> GetDronesChargeList()
        {
            return DataSource.DronesInCharge;
        }
        #endregion ListDisplay
    }
}
