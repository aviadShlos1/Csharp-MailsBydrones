using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    class DalObject
    {
        DalObject() { DataSource.Initialize(); }

        #region Add methods
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
        public void AddStation(Station newStation)
        {
            DataSource.Stations.Add(newStation);
        }
        #endregion Add methods

        #region Update methods
        public void ConnectDroneToParcel(int parcelId, int droneId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel temp = DataSource.Parcels[parcelIndex];
            temp.DroneId = droneId;
            temp.Scheduled = DateTime.Now;
            DataSource.Parcels[parcelIndex] = temp;
        }
        public void PickUpParcel(int parcelId)
        {
            int parcelIndex = DataSource.Parcels.FindIndex(i => i.Id == parcelId);
            Parcel temp = DataSource.Parcels[parcelIndex];
            temp.PickedUp = DateTime.Now;
            DataSource.Parcels[parcelIndex] = temp;
        }

        public void Display()
        { }
        public void ListDisplay()
        { }

    }
}
