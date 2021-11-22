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
        public void AddCustomer();
        public void AddParcel();

        #endregion AddOptions

        #region UpdateOptions

        public void UpdateDroneName();
        public void UpdateBaseStationData();
        public void UpdateCustomerData();
        public void DroneToCharge();
        public void ReleaseDroneCharge();
        public void AssignParcelToDrone();
        public void PickUpParcel();
        public void SupplyParcel();
        #endregion UpdateOptions    

        #region DisplayOptions
        public void GetSingleBaseStation();
        public void GetSingleDrone();
        public void GetSingleCustomer();
        public void GetSingleParcel();

        #endregion DisplayOptions

        #region ListDisplayOptions
        public void GetBaseStationsList();
        public void GetDronesList();
        public void GetCustomersList();
        public void GetParcelsList();
        public void ParcelsWithoutDroneListDisplay();
        public void FreeChargeSlotsListDisplay();
        #endregion ListDisplayOptions
    }
}