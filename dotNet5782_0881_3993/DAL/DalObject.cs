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

namespace DalObject
{
    public class DalObject:DalApi.IDal
    {
        //static readonly DalObject instance = new DalObject();
        //internal static DalObject Instance { get => instance; }
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
        public DalObject() 
        { 
            DataSource.Initialize();  //call the first initialize in the dal ctor
        }

        #region Add methods
        /// <summary>
        /// Adding a new object for the all entities
        /// </summary>
        /// <param name="newStation"></param>
        public void AddStation(BaseStationDal newStation)
        {
            int check = DataSource.BaseStations.FindIndex(x=>x.Id==newStation.Id);
            if (check != -1) 
            {
                throw new AlreadyExistException(newStation.Id);
            }
            DataSource.BaseStations.Add(newStation);
        }
        public void AddDrone(DroneDal newDrone)
        {
            int existIndex = DataSource.Drones.FindIndex(x => x.Id == newDrone.Id);
            if (existIndex != -1)
            {
                throw new AlreadyExistException(newDrone.Id);
            }
            DataSource.Drones.Add(newDrone);
        }
        public void AddCustomer(CustomerDal newCustomer)
        {
            int existIndex = DataSource.Customers.FindIndex(x => x.Id == newCustomer.Id);
            if (existIndex != -1)
            {
                throw new AlreadyExistException(newCustomer.Id);
            }
            DataSource.Customers.Add(newCustomer);
        }
        public int AddParcel(ParcelDal newParcel)
        {  
            newParcel.Id = ++(DataSource.Config.RunId); 
            DataSource.Parcels.Add(newParcel);
            return newParcel.Id;
        }
        #endregion Add methods

        #region Update methods
        public void UpdateDrone(DroneDal myDrone)
        {
            DroneDal tempDrone= DataSource.Drones.FirstOrDefault(x => x.Id == myDrone.Id);
            int index = DataSource.Drones.IndexOf(tempDrone);
            DataSource.Drones[index] = myDrone;
        }
        public void UpdateCustomer(CustomerDal myCustomer)
        {
            CustomerDal tempCustomer = DataSource.Customers.FirstOrDefault(x => x.Id == myCustomer.Id);
            int index = DataSource.Customers.IndexOf(tempCustomer);
            DataSource.Customers[index] = myCustomer;
        }

        public void UpdateBaseStation(BaseStationDal myBaseStation)
        {
            BaseStationDal tempBaseStation = DataSource.BaseStations.FirstOrDefault(x => x.Id == myBaseStation.Id);
            int index = DataSource.BaseStations.IndexOf(tempBaseStation);
            DataSource.BaseStations[index] = myBaseStation;
        }

        /// <summary>
        /// Assining a drone to a parcel by the parcel and drone id 
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(parcelId);
            }
            ParcelDal parcel1 = DataSource.Parcels[parcelIndex];
            parcel1.DroneToParcelId = droneId;
            parcel1.AssignningTime = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel1;
        }
        /// <summary>
        /// Picking up a parcel by the assined drone before, with given the parcel id
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickUpParcel(int droneId)
        {
            
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.DroneToParcelId == droneId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(droneId);
            }
            ParcelDal parcel2 = DataSource.Parcels[parcelIndex];
            parcel2.PickingUpTime = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel2;
        }
        /// <summary>
        /// Delivering the parcel to the customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void SupplyParcel(int droneId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.DroneToParcelId == droneId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(droneId);
            }
            ParcelDal parcel3 = DataSource.Parcels[parcelIndex];
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
            int chargeIndex = DataSource.DronesInCharge.FindIndex(i => i.DroneId==droneId);
            if (droneId == -1)
            {
                throw new NotExistException(droneId);
            }
            int stationIndex = DataSource.BaseStations.FindIndex(i => i.Id == stationId);
            BaseStationDal station1 = DataSource.BaseStations[stationIndex];
            station1.FreeChargeSlots--; // Reducing the free chargeSlots
            DataSource.BaseStations[stationIndex] = station1;

            DataSource.DronesInCharge.Add(new DroneChargeDal() { DroneId = droneId, StationId = stationId, StartChargeTime=DateTime.Now });//initiate a new drone charge
        }
        /// <summary>
        /// Realesing a drone from the charge base station
        /// </summary>
        /// <param name="droneId"></param>
        public TimeSpan DroneRelease(int droneId)
        {
            
            int chargeIndex = DataSource.DronesInCharge.FindIndex(i => i.DroneId == droneId);
            if (chargeIndex == -1)
            {
                throw new NotExistException(droneId);
            }
            DroneChargeDal myDroneRelease = DataSource.DronesInCharge[chargeIndex];
            int baseStationId = myDroneRelease.StationId;

            int stationIndex = DataSource.BaseStations.FindIndex(i => i.Id == baseStationId);
            BaseStationDal station2 = DataSource.BaseStations[stationIndex];
            station2.FreeChargeSlots++;//Increasing the number of the free charge slots
            DataSource.BaseStations[stationIndex] = station2;
            DateTime releaseTime = DateTime.Now;
            TimeSpan totalCharge = releaseTime - myDroneRelease.StartChargeTime; 
            DataSource.DronesInCharge.RemoveAt(DataSource.DronesInCharge.FindIndex(x => x.DroneId == droneId));//Remove the drone from the list of the drone charges
            return totalCharge;
        }
        #endregion Update methods

        #region Single display 
        /// <summary>
        /// Displaying the details for a single entity for the 4 methods below 
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns>The type of the entity</returns>
        public BaseStationDal GetSingleBaseStation(int stationId)
        {
            int stationIndex = DataSource.BaseStations.FindIndex(i => i.Id == stationId);
            if (stationIndex == -1)
            {
                throw new NotExistException(stationId);
            }
            return DataSource.BaseStations.Find(i => i.Id == stationId);    
        }
        public DroneDal GetSingleDrone(int droneId)
        {
            int droneIndex = DataSource.Drones.FindIndex(i => i.Id == droneId);
            if (droneIndex == -1)
            {
                throw new NotExistException(droneId);
            }
            return DataSource.Drones.Find(i => i.Id == droneId);
        }
        public CustomerDal GetSingleCustomer(int customerId)
        {
            int customerIndex = DataSource.Customers.FindIndex(i => i.Id == customerId);
            if (customerIndex == -1)
            {
                throw new NotExistException(customerId);
            }
            return DataSource.Customers.Find(i => i.Id == customerId);
        }
        public ParcelDal GetSingleParcel(int parcelId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(parcelId);
            }
            return DataSource.Parcels.Find(i => i.Id == parcelId);
        }
        #endregion Single display

        #region ListDisplay
        /// <summary>
        /// Displaying the all list for all the entity that chosen
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseStationDal> GetBaseStationsList(Predicate<BaseStationDal> myPredicate=null )
        {

            return DataSource.BaseStations.FindAll(x => myPredicate == null? true : myPredicate(x)).ToList();
        }
        public IEnumerable<DroneDal> GetDronesList(Predicate<DroneDal> myPredicate = null)
        {
            return DataSource.Drones.FindAll(x => myPredicate == null ? true : myPredicate(x)).ToList();
        }
        public IEnumerable<CustomerDal> GetCustomersList()
        {
            return DataSource.Customers;
        }
        public IEnumerable<ParcelDal> GetParcelsList(Predicate<ParcelDal> myPredicate = null)
        {
            return DataSource.Parcels.FindAll(x => myPredicate == null ? true : myPredicate(x)).ToList();
        }
        public IEnumerable<DroneChargeDal> GetDronesChargeList()
        {
            return DataSource.DronesInCharge;
        }
        #endregion ListDisplay
    }
}
