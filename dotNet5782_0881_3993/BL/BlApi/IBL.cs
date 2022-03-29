//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBL
    {
        #region AddOptions

        public void AddBaseStation(BaseStationBl newBaseStationBl);
        public void AddDrone(DroneToList newDroneBl,int firstChargeStation);
        public void AddCustomer(CustomerBL newCustomer);
        public void AddParcel(ParcelBl newParcel);

        #endregion AddOptions

        #region UpdateOptions

        public void UpdateDroneName(int droneId, string newModel);
        public void UpdateBaseStationData(int baseStationId, string newName, int totalChargeSlots);
        public void UpdateCustomerData(int myId, string newName, string newPhone);
        public void DroneToCharge(int myDroneId);
        public void ReleaseDroneCharge(int myDroneId);
        public void AssignParcelToDrone(int myDroneId);
        public void PickUpParcel(int droneId);
        public void SupplyParcel(int droneId);
        public void RemoveParcel(ParcelBl myParcel);

        public void sim(int droneId, Action reportProgressInSimultor, Func<bool> isTimeRun);
        #endregion UpdateOptions    

        #region DisplayOptions
        public BaseStationBl GetSingleBaseStation(int baseStationId);
        public DroneBl GetSingleDrone(int myDroneId);
        public CustomerBL GetSingleCustomer(int customerId);
        public ParcelBl GetSingleParcel(int parcelId);

        #endregion DisplayOptions

        #region ListDisplayOptions
        public List<BaseStationToList> GetBaseStationsBl(Predicate<BaseStationToList> myPredicate = null);
        public List<DroneToList> GetDronesBl(Predicate<DroneToList> myPredicate = null);
        public List<CustomerToList> GetCustomersBl();
        public List<ParcelToList> GetParcelsBl(Predicate<ParcelToList> myPredicate = null);

        //public List<ParcelToList> GetParcelsWithoutDroneBl();
        //public List<BaseStationToList> GetStationsWithFreeChargeBl();
        #endregion ListDisplayOptions
    }
}