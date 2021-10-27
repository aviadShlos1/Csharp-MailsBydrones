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
        public void AddDrone(Drone newDrone)
        {
            DataSource.Drones.Add(newDrone);
        }
        public void AddCustomer(Customer newCustomer)
        {
            DataSource.Customers.Add(newCustomer);
        }
        public void AddParcel(Parcel newParcel)
        {
            DataSource.Parcels.Add(newParcel);
        }
        public void AddStation(Station newStation)
        {
            DataSource.Stations.Add(newStation);
        }
        public void ConnectDroneToParcel(Parcel p,Drone d)
        {
            p.DroneId = d.Id;
            p.Scheduled
        }
        public void PickUpParcel(Parcel p, Drone d)
        {

        }
        public void Display()
        { }
        public void ListDisplay()
        { }

    }
}
