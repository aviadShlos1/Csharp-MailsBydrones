using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        #region AddOptions

        public void AddBaseStation(int myId, string myBaseStationName, Location myBaseStationLocation, int myFreeChargeSlots);
        public void AddDrone(int myDroneId, string myModel, WeightCategoriesBL myDroneWeight, int myBaseStationId);
        public void AddCustomer(CustomerBL newCustomer);
        public void AddParcel(int mySenderId, int myRecieverId, WeightCategoriesBL myParcelWeight, PrioritiesBL myPriority);

        #endregion AddOptions

        #region UpdateOptions

        public void UpdateDroneName(int droneId, string newModel);
        public void UpdateBaseStationData(int baseStationId, string newName, int totalChargeSlots);
        public void UpdateCustomerData(int myId, string newName, string newPhone);
        public void DroneToCharge(int myDroneId);
        public void ReleaseDroneCharge(int myDroneId, TimeSpan chargeTime);
        public void AssignParcelToDrone(int myDroneId);
        public void PickUpParcel(int droneId);
        public void SupplyParcel(int droneId);
        #endregion UpdateOptions    

        #region DisplayOptions
        public BaseStationBl GetSingleBaseStation(int baseStationId);
        public DroneBl GetSingleDrone(int myDroneId);
        public CustomerBL GetSingleCustomer(int customerId)(int customerId);
        public ParcelBl GetSingleParcel(int parcelId);

        #endregion DisplayOptions

        #region ListDisplayOptions
        public List<BaseStationToList> GetBaseStationsBl();
        public List<DroneToList> GetDronesBl();
        public List<CustomerToList> GetCustomersBl();
        public List<ParcelToList> GetParcelsBl();
        public List<ParcelToList> GetParcelsWithoutDroneBl();
        public List<BaseStationToList> GetStationsWithFreeChargeBl();
        #endregion ListDisplayOptions
    }
}