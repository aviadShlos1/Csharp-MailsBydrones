using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using DO;
using DalApi;

namespace Dal
{
    sealed class DalXml /*:*//* DalApi.IDal*/
    {
        #region Singleton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }
        public DalXml()
        {
            if (!File.Exists(CustomerPath))
                CreateFiles();
            else
                LoadData();
        }
        public DalXml Instance { get=>instance;  }
        #endregion Singleton

        string CustomerPath = @"CustomerXml.xml";
        string DronePath = @"DroneXml.xml";
        string BaseStationPath = @"BaseStationXml.xml";
        string ParcelPath = @"ParcelXml.xml";
        string DroneChargePath = @"DroneChargeXml.xml";

        #region Customer
        XElement CustomerRoot;
        private void CreateFiles()
        {
            CustomerRoot = new XElement("Customers");
            CustomerRoot.Save(CustomerPath);
        }
        private void LoadData()
        {
            try
            {
                CustomerRoot = XElement.Load(CustomerPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }
        public void SaveCustomerListLinq(List<CustomerDal> Customers)
        {
            var xList = from item in Customers
                    select new XElement("Customer",
                                                new XElement("id", item.Id),
                                               new XElement("name", item.Name),
                                               new XElement("phone", item.Phone),
                                               new XElement("location",
                                                 new XElement("longitude", item.Longitude),
                                                 new XElement("latitude", item.Latitude)
                                                 )
                                               );
            CustomerRoot = new XElement("Customers", xList);
            CustomerRoot.Save(CustomerPath);
        }
        public List<CustomerDal> GetCustomerList()
        {
            LoadData();
            List<CustomerDal> Customers;
            try
            {
                Customers = (from item in CustomerRoot.Elements()
                            select new CustomerDal()
                            {
                                Id = Convert.ToInt32(item.Element("id").Value),
                                Name = item.Element("name").Value,
                                Phone=item.Element("phone").Value,
                                Longitude = Convert.ToDouble(item.Element("location").Element("longitude").Value),
                                Latitude = Convert.ToDouble(item.Element("location").Element("latitude").Value)
                            }).ToList();
            }
            catch
            {
                Customers = null;
            }
            return Customers;
        }
        public CustomerDal GetStudent(int id)
        {
            LoadData();
            CustomerDal Customer;
            try
            {
                Customer = (from item in CustomerRoot.Elements()
                           where Convert.ToInt32(item.Element("id").Value) == id
                           select new CustomerDal()
                           {
                               Id = Convert.ToInt32(item.Element("id").Value),
                               Phone = item.Element("phone").Value,
                               Longitude = Convert.ToDouble(item.Element("location").Element("longitude").Value),
                               Latitude = Convert.ToDouble(item.Element("location").Element("latitude").Value)
                           }).FirstOrDefault();
            }
            catch
            {
                Customer = default;
            }
            return Customer;
        }
        public void AddCustomer(CustomerDal customer)
        {
            XElement id = new XElement("id", customer.Id);
            XElement name = new XElement("name", customer.Name);
            XElement phone = new XElement("phone", customer.Phone);
            XElement longitude = new XElement("longitude", customer.Longitude);
            XElement latitude = new XElement("latitude", customer.Latitude);
            XElement location = new XElement("location", longitude, latitude);

            XElement cust = new XElement("Customer", id ,name, phone, location);
            CustomerRoot.Add(cust);
            CustomerRoot.Save(CustomerPath);
        }
        public bool RemoveCustomer(int id)
        {
            XElement CustomerElement;
            try
            {
                CustomerElement = (from item in CustomerRoot.Elements()
                                  where Convert.ToInt32(item.Element("id").Value) == id
                                  select item).FirstOrDefault();
                CustomerElement.Remove();
                CustomerRoot.Save(CustomerPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void UpdateCustomer(CustomerDal customer)
        {
            XElement StudentElement = (from item in CustomerRoot.Elements()
                                       where Convert.ToInt32(item.Element("id").Value) == customer.Id
                                       select item).FirstOrDefault();

            StudentElement.Element("name").Value = customer.Name;
            StudentElement.Element("phone").Value = customer.Phone;

            CustomerRoot.Save(CustomerPath);
        }

        #endregion

        #region Drone
        public void AddDrone(DroneDal newDrone)
        {
            List<DroneDal> Drones= XMLTools.LoadListFromXMLSerializer<DroneDal>(DronePath);
            int existIndex = Drones.FindIndex(x => x.Id == newDrone.Id);
            if (existIndex != -1)
            {
                throw new AlreadyExistException(newDrone.Id);
            }
            Drones.Add(newDrone);
            XMLTools.SaveListToXMLSerializer<DroneDal>(Drones, DronePath);
        }
        public void UpdateDrone(DroneDal myDrone)
        {
            List<DroneDal> drones = XMLTools.LoadListFromXMLSerializer<DroneDal>(DronePath);
            DroneDal tempDrone = drones.FirstOrDefault(x => x.Id == myDrone.Id);
            int index = drones.IndexOf(tempDrone);
            drones[index] = myDrone;
            XMLTools.SaveListToXMLSerializer<DroneDal>(drones, DronePath);
        }
        
        /// <summary>
        /// Sending a drone to charge in the base station, with given the drone and station id
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void DroneToCharge(int droneId, int stationId)
        {
            List<DroneDal> drones = XMLTools.LoadListFromXMLSerializer<DroneDal>(DronePath);
            int chargeIndex = DataSource.DronesInCharge.FindIndex(i => i.DroneId == droneId);
            if (droneId == -1)
            {
                throw new NotExistException(droneId);
            }
            int stationIndex = DataSource.BaseStations.FindIndex(i => i.Id == stationId);
            BaseStationDal station1 = DataSource.BaseStations[stationIndex];
            station1.FreeChargeSlots--; // Reducing the free chargeSlots
            DataSource.BaseStations[stationIndex] = station1;

            DataSource.DronesInCharge.Add(new DroneChargeDal() { DroneId = droneId, StationId = stationId, StartChargeTime = DateTime.Now });//initiate a new drone charge
            XMLTools.SaveListToXMLSerializer<DroneDal>(drones, DronePath);
        }
        /// <summary>
        /// Realesing a drone from the charge base station
        /// </summary>
        /// <param name="droneId"></param>
        public TimeSpan DroneRelease(int droneId)
        {
            List<DroneDal> drones = XMLTools.LoadListFromXMLSerializer<DroneDal>(DronePath);
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
            XMLTools.SaveListToXMLSerializer<DroneDal>(drones, DronePath);
        }
        public DroneDal GetSingleDrone(int droneId)
        {
            List<DroneDal> drones = XMLTools.LoadListFromXMLSerializer<DroneDal>(DronePath);
            int droneIndex = drones.FindIndex(i => i.Id == droneId);
            if (droneIndex == -1)
            {
                throw new NotExistException(droneId);
            }
            return drones.Find(i => i.Id == droneId);
        }
        public IEnumerable<DroneDal> GetDronesList(Predicate<DroneDal> myPredicate = null)
        {
            List<DroneDal> drones = XMLTools.LoadListFromXMLSerializer<DroneDal>(DronePath);
            return drones.Where(x => myPredicate == null ? true : myPredicate(x));
        }
        #endregion

        #region BaseStation
        public void AddStation(BaseStationDal newStation)
        {
            List<BaseStationDal> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            int check = baseStations.FindIndex(x => x.Id == newStation.Id);
            if (check != -1)
            {
                throw new AlreadyExistException(newStation.Id);
            }
            baseStations.Add(newStation);
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(baseStations, BaseStationPath);
        }
        public void UpdateBaseStation(BaseStationDal myBaseStation)
        {
            List<BaseStationDal> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            BaseStationDal tempBaseStation = baseStations.FirstOrDefault(x => x.Id == myBaseStation.Id);
            int index = baseStations.IndexOf(tempBaseStation);
            baseStations[index] = myBaseStation;
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(baseStations, BaseStationPath);
        }
        public BaseStationDal GetSingleBaseStation(int stationId)
        {
            List<BaseStationDal> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            int stationIndex = baseStations.FindIndex(i => i.Id == stationId);
            if (stationIndex == -1)
            {
                throw new NotExistException(stationId);
            }
            return baseStations.Find(i => i.Id == stationId);
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(baseStations, BaseStationPath);
        }
        public IEnumerable<BaseStationDal> GetBaseStationsList(Predicate<BaseStationDal> myPredicate = null)
        {
            List<BaseStationDal> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            return baseStations.FindAll(x => myPredicate == null ? true : myPredicate(x)).ToList();
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(baseStations, BaseStationPath);
        }
        #endregion

        #region Parcel
        public int AddParcel(ParcelDal newParcel)
        {
            newParcel.Id = ++(DataSource.Config.RunId);
            DataSource.Parcels.Add(newParcel);
            return newParcel.Id;
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
        public void RemoveParcel(ParcelDal myParcel)
        {
            DataSource.Parcels.Remove(myParcel);
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
        public IEnumerable<ParcelDal> GetParcelsList(Predicate<ParcelDal> myPredicate = null)
        {
            return DataSource.Parcels.FindAll(x => myPredicate == null ? true : myPredicate(x)).ToList();
        }
        #endregion

        #region DroneInCharge
        public void SendDroneToCharge(int droneId, int stationId)
        {
            List<DroneChargeDal> DronesInCharge = XMLTools.LoadListFromXMLSerializer<DroneChargeDal>(DroneChargePath);
            int chargeIndex = DronesInCharge.FindIndex(i => i.DroneId == droneId);
            if (droneId == -1)
                throw new NotExistException(droneId);
            List<BaseStationDal> BaseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            int stationIndex = BaseStations.FindIndex(i => i.Id == stationId);
            BaseStationDal station1 = BaseStations[stationIndex];
            station1.FreeChargeSlots--; // Reducing the free chargeSlots
            BaseStations[stationIndex] = station1;
            DronesInCharge.Add(new DroneChargeDal() { DroneId = droneId, StationId = stationId, StartChargeTime = DateTime.Now });//initiate a new drone charge
            XMLTools.SaveListToXMLSerializer(DronesInCharge, DroneChargePath);
        }
        /// <summary>
        /// Realesing a drone from the charge base station
        /// </summary>
        /// <param name="droneId"></param>
        public TimeSpan DroneToRelease(int droneId)
        {
            List<DroneChargeDal> DronesInCharge = XMLTools.LoadListFromXMLSerializer<DroneChargeDal>(DroneChargePath);
            int chargeIndex = DronesInCharge.FindIndex(i => i.DroneId == droneId);
            if (chargeIndex == -1)
                throw new NotExistException(droneId);
            DroneChargeDal myDroneRelease = DronesInCharge[chargeIndex];
            int baseStationId = myDroneRelease.StationId;
            List<BaseStationDal> BaseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            int stationIndex = BaseStations.FindIndex(i => i.Id == baseStationId);
            BaseStationDal station2 = BaseStations[stationIndex];
            station2.FreeChargeSlots++;//Increasing the number of the free charge slots
            BaseStations[stationIndex] = station2;
            DateTime releaseTime = DateTime.Now;
            TimeSpan totalCharge = releaseTime - myDroneRelease.StartChargeTime;
            DronesInCharge.RemoveAt(DronesInCharge.FindIndex(x => x.DroneId == droneId));//Remove the drone from the list of the drone charges
            XMLTools.SaveListToXMLSerializer(DronesInCharge, DroneChargePath);
            return totalCharge;
        }

        public IEnumerable<DroneChargeDal> GetDronesChargeList()
        {
            List<DroneChargeDal> DronesInCharge = XMLTools.LoadListFromXMLSerializer<DroneChargeDal>(DroneChargePath);
            return DronesInCharge;
        }
        #endregion
    }
}
