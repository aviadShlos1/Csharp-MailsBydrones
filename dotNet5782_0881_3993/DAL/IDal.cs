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
        #region Add methods
        public void AddStation(BaseStationDal newStation);
        public void AddDrone(DroneDal newDrone);
        public void AddCustomer(CustomerDal newCustomer);
        public int AddParcel(ParcelDal newParcel);
        #endregion Add methods

        #region Update methods        
        public void ConnectDroneToParcel(int parcelId, int droneId);    
        public void PickUpParcel(int parcelId);
        public void DelieverParcel(int parcelId);
        public void DroneToCharge(int droneId, int stationId);
        public void DroneRelease(int droneId);
        #endregion Update methods

        #region Single display 
        public BaseStationDal GetSingleBaseStation(int stationId);
        public DroneDal GetSingleDrone(int droneId);
        public CustomerDal GetSingleCustomer(int customerId);
        public ParcelDal GetSingleParcel(int parcelId);
        #endregion Single display

        #region ListDisplay
        public IEnumerable<BaseStationDal> GetBaseStationsList();
        public IEnumerable<DroneDal> GetDronesList();
        public IEnumerable<CustomerDal> GetCustomersList();
        public IEnumerable<ParcelDal> GetParcelsList();
        public IEnumerable<ParcelDal> GetParcelsWithoutDrone();
        public IEnumerable<BaseStationDal> GetStationsWithFreeCharge();
        public IEnumerable<DroneCharge> GetDronesChargeList();     
        #endregion ListDisplay
    }
}
