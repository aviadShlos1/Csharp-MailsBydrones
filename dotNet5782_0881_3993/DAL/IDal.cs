//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//brief: In this program we built the data access layer


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace DalApi
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

        /// Adding a new object for the base stations list
        /// </summary>
        /// <param name="newStation"></param>
        public void AddStation(BaseStationDal newStation);

        /// Adding a new object for the drones list
        /// </summary>
        /// <param name="newDrone"></param>
        public void AddDrone(DroneDal newDrone);

        /// Adding a new object for the customers list
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(CustomerDal newCustomer);

        /// Adding a new object for the parcels list
        /// </summary>
        /// <param name="newParcel"></param>
        public int AddParcel(ParcelDal newParcel);
        #endregion Add methods

        #region Update methods   
        public void UpdateDrone(DroneDal myDrone);
        public void UpdateCustomer(CustomerDal myCustomer);
       
        public void UpdateBaseStation(BaseStationDal myBaseStation);
        /// Assining a drone to a parcel by the parcel and drone id 
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int parcelId, int droneId);

        /// Picking up a parcel by the assined drone before, with given the parcel id
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickUpParcel(int parcelId);

        /// Delivering the parcel to the customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void SupplyParcel(int parcelId);

        /// Sending a drone to charge in the base station, with given the drone and station id
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void DroneToCharge(int droneId, int stationId);

        /// Realesing a drone from the charge base station
        /// </summary>
        /// <param name="droneId"></param>
        public TimeSpan DroneRelease(int droneId);
        #endregion Update methods

        #region Single display 
        /// Displaying the details for a single base station 
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>The type of the entity</returns>
        public BaseStationDal GetSingleBaseStation(int stationId);

        /// Displaying the details for a single drone 
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns>The type of the entity</returns>
        public DroneDal GetSingleDrone(int droneId);

        /// Displaying the details for a single customer 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>The type of the entity</returns>
        public CustomerDal GetSingleCustomer(int customerId);

        /// Displaying the details for a single parcel 
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns>The type of the entity</returns>
        public ParcelDal GetSingleParcel(int parcelId);
        #endregion Single display

        #region ListDisplay
        /// Displaying the base stations list which includes the details of the all base stations
        /// </summary>
        /// <returns> The ienumerable list </returns>
        public IEnumerable<BaseStationDal> GetBaseStationsList(Predicate<BaseStationDal> myPredicate = null);

        /// Displaying the drones list which includes the details of the all drones
        /// </summary>
        /// <returns> The ienumerable list </returns>
        public IEnumerable<DroneDal> GetDronesList(Predicate<DroneDal> myPredicate = null);

        /// Displaying the customers list which includes the details of the all customers
        /// </summary>
        /// <returns> The ienumerable list </returns>
        public IEnumerable<CustomerDal> GetCustomersList();

        /// Displaying the parcels list which includes the details of the all parcels
        /// </summary>
        /// <returns> The ienumerable list </returns>
        public IEnumerable<ParcelDal> GetParcelsList(Predicate<ParcelDal> myPredicate = null);

      
        /// Displaying the drones in charge
        /// </summary>
        /// <returns> The ienumerable list </returns>
        public IEnumerable<DroneChargeDal> GetDronesChargeList();
        #endregion ListDisplay
    }
}
