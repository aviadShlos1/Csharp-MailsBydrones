using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public class BL
    {
        public List<DroneToList> DronesList { get; set; }

        //public BL() { }; 







        partial class AddOptions
        {
            public void AddBaseStation() ;
            public void AddDrone();
            public void AddCustomer();
            public void AddParcel();
        }



        partial class UpdateOptions

        {
            public void UpdateDroneName();
            public void UpdateBaseStationData();
            public void UpdateCustomerData();
            public void DroneToCharge();
            public void ReleaseDroneCharge();
            public void AssignParcelToDrone();
            public void PickUpParcel();
            public void SupplyParcel();
        }



        partial class DisplayOptions
        {
        public void BaseStationDisplay();
        public void DroneDisplay();
        public void CustomerDisplay();
        public void ParcelDisplay();
        }


        partial class ListDisplayOptions
        {
        public void BaseStationsListDisplay();
        public void DronesListDisplay();
        public void CustomersListDisplay();
        public void ParcelsListDisplay();
        public void ParcelsWithoutDroneListDisplay();
        public void FreeChargeSlotsListDisplay();
        }
        
    }

    
}
