using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    interface IBL
    {
        #region AddOptions

        public void AddBaseStation();
        public void AddDrone();
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
        public void BaseStationDisplay();
        public void DroneDisplay();
        public void CustomerDisplay();
        public void ParcelDisplay();

        #endregion DisplayOptions

        #region ListDisplayOptions
        public void BaseStationsListDisplay();
        public void DronesListDisplay();
        public void CustomersListDisplay();
        public void ParcelsListDisplay();
        public void ParcelsWithoutDroneListDisplay();
        public void FreeChargeSlotsListDisplay();
        #endregion ListDisplayOptions
    }
}