using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        public DalObject() { DataSource.Initialize(); }

        #region Add methods
        public void AddStation(Station newStation)
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
        public void ConnectDroneToParcel(int parcelId, int droneId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel parcel1 = DataSource.Parcels[parcelIndex];
            parcel1.DroneToParcel_Id = droneId;
            parcel1.Scheduled = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel1;
        }
        public void PickUpParcel(int parcelId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel parcel2 = DataSource.Parcels[parcelIndex];
            parcel2.PickedUp = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel2;
        }
        public void DelieverParcel(int parcelId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel parcel3 = DataSource.Parcels[parcelIndex];
            parcel3.Delievered = DateTime.Now;
            DataSource.Parcels[parcelIndex] = parcel3;
        }
        public void DroneToCharge(int droneId, int stationId)
        {
            int droneIndex = DataSource.Drones.FindIndex(i => i.Id == droneId);
            Drone drone1 = DataSource.Drones[droneIndex];
            drone1.Status = (DroneStatuses)1; //DroneStatuses: free,maintence,delievery
            DataSource.Drones[droneIndex] = drone1;

            int stationIndex = DataSource.Stations.FindIndex(i => i.Id == stationId);
            Station station1 = DataSource.Stations[stationIndex];
            station1.ChargeSlots--; // reducing the free chargeSlots
            DataSource.Stations[stationIndex] = station1;

            DataSource.DroneCharges.Add(new DroneCharge() { DroneId = droneId, StationId = stationId });

            int chargeIndex = DataSource.DroneCharges.FindIndex(i => i.DroneId==droneId);
            DroneCharge charge1 = DataSource.DroneCharges[chargeIndex];
            DataSource.DroneCharges[chargeIndex] = charge1;
        }

        public void DroneRelease(int droneId)
        {
            int droneReleaseIndex = DataSource.Drones.FindIndex(i => i.Id == droneId);
            Drone drone2 = DataSource.Drones[droneReleaseIndex];
            drone2.Status = (DroneStatuses)0; //DroneStatuses: free,maintence,delievery
            DataSource.Drones[droneReleaseIndex] = drone2;

            int chargeIndex = DataSource.DroneCharges.FindIndex(i => i.DroneId == droneId);
            DroneCharge help = DataSource.DroneCharges[chargeIndex];
            int baseStationId = help.StationId;

            int stationIndex = DataSource.Stations.FindIndex(i => i.Id == baseStationId);
            Station station2 = DataSource.Stations[stationIndex];
            station2.ChargeSlots++;
            DataSource.Stations[stationIndex] = station2;

            DataSource.DroneCharges.RemoveAt(DataSource.DroneCharges.FindIndex(x => x.DroneId == droneId));


        }
        #endregion Update methods

        #region Single display 
        public Station StationDisplay(int stationId)
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
        public List<Station> StationsList()
        {
            return DataSource.Stations.Take(DataSource.Stations.Count).ToList();
        }
        public List<Drone> DronesList()
        {
            return DataSource.Drones.Take(DataSource.Drones.Count).ToList();
        }
        public List<Customer> CustomersList()
        {
            return DataSource.Customers.Take(DataSource.Customers.Count).ToList();
        }
        public List<Parcel> ParcelsList()
        {
            return DataSource.Parcels.Take(DataSource.Parcels.Count).ToList();
        }

        public List<Parcel> ParcelsWithoutDrone()
        {
            return DataSource.Parcels.TakeWhile(i => i.DroneToParcel_Id == 0).ToList();
        }
        public List<Station> FreeChargeSlotsList()
        {
            return DataSource.Stations.TakeWhile(i => i.ChargeSlots != 0).ToList();
        }
        #endregion ListDisplay
    }
}
