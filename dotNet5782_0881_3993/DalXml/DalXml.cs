using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using DO;
using DalApi;

namespace DalXml
{
    sealed class DalXml:DalApi.IDal
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

        public double[] EnergyConsumption()
        {
            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"ConfigDetails.xml");
            double[] temp = 
                { 
                double.Parse(config[0]), 
                double.Parse(config[1]), 
                double.Parse(config[2]),
                double.Parse(config[3]),
                double.Parse(config[4])};
            return temp;
        }

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
        public IEnumerable<CustomerDal> GetCustomersList()
        {
            LoadData();
            IEnumerable<CustomerDal> Customers;
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
        public CustomerDal GetSingleCustomer(int id)
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
            List<BaseStationDal> BaseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            int check = BaseStations.FindIndex(x => x.Id == newStation.Id);
            if (check != -1)
            {
                throw new AlreadyExistException(newStation.Id);
            }
            BaseStations.Add(newStation);
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(BaseStations, BaseStationPath);
        }
        public void UpdateBaseStation(BaseStationDal myBaseStation)
        {
            List<BaseStationDal> BaseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            BaseStationDal tempBaseStation = BaseStations.FirstOrDefault(x => x.Id == myBaseStation.Id);
            int index = BaseStations.IndexOf(tempBaseStation);
            BaseStations[index] = myBaseStation;
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(BaseStations, BaseStationPath);
        }
        public BaseStationDal GetSingleBaseStation(int stationId)
        {
            List<BaseStationDal> BaseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            int stationIndex = BaseStations.FindIndex(i => i.Id == stationId);
            if (stationIndex == -1)
            {
                throw new NotExistException(stationId);
            }
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(BaseStations, BaseStationPath);
            return BaseStations.Find(i => i.Id == stationId);
        }
        public IEnumerable<BaseStationDal> GetBaseStationsList(Predicate<BaseStationDal> myPredicate = null)
        {
            List<BaseStationDal> BaseStations = XMLTools.LoadListFromXMLSerializer<BaseStationDal>(BaseStationPath);
            XMLTools.SaveListToXMLSerializer<BaseStationDal>(BaseStations, BaseStationPath);
            return BaseStations.Where(x => myPredicate == null ? true : myPredicate(x));
        }
        #endregion

        #region Parcel
        public int AddParcel(ParcelDal newParcel)
        {
            List<ParcelDal> Parcels = XMLTools.LoadListFromXMLSerializer<ParcelDal>(ParcelPath);
            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"ConfigDetails.xml");
            int runParcelId = int.Parse(config[5]);
            newParcel.Id = runParcelId++;
            XMLTools.SaveListToXMLSerializer<string>(config, @"ConfigDetails.xml");
        
            Parcels.Add(newParcel);
            XMLTools.SaveListToXMLSerializer<ParcelDal>(Parcels, ParcelPath);
            return newParcel.Id;
        }
        /// <summary>
        /// Assining a drone to a parcel by the parcel and drone id 
        /// </summary>
        /// <param name="parcelId"></param>
        /// <param name="droneId"></param>
        public void AssignParcelToDrone(int parcelId, int droneId)
        {
            List<ParcelDal> Parcels = XMLTools.LoadListFromXMLSerializer<ParcelDal>(ParcelPath);
            int parcelIndex = Parcels.FindIndex(i => i.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(parcelId);
            }
            ParcelDal parcel1 = Parcels[parcelIndex];
            parcel1.DroneToParcelId = droneId;
            parcel1.AssignningTime = DateTime.Now;
            Parcels[parcelIndex] = parcel1;
            XMLTools.SaveListToXMLSerializer<ParcelDal>(Parcels, ParcelPath);
        }
        /// <summary>
        /// Picking up a parcel by the assined drone before, with given the parcel id
        /// </summary>
        /// <param name="parcelId"></param>
        public void PickUpParcel(int droneId)
        {
            List<ParcelDal> Parcels = XMLTools.LoadListFromXMLSerializer<ParcelDal>(ParcelPath);
            int parcelIndex = Parcels.FindIndex(i => i.DroneToParcelId == droneId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(droneId);
            }
            ParcelDal parcel2 = Parcels[parcelIndex];
            parcel2.PickingUpTime = DateTime.Now;
            Parcels[parcelIndex] = parcel2;
            XMLTools.SaveListToXMLSerializer<ParcelDal>(Parcels, ParcelPath);
        }
        /// <summary>
        /// Delivering the parcel to the customer
        /// </summary>
        /// <param name="parcelId"></param>
        public void SupplyParcel(int droneId)
        {
            List<ParcelDal> Parcels = XMLTools.LoadListFromXMLSerializer<ParcelDal>(ParcelPath);
            int parcelIndex = Parcels.FindIndex(i => i.DroneToParcelId == droneId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(droneId);
            }
            ParcelDal parcel3 = Parcels[parcelIndex];
            parcel3.SupplyingTime = DateTime.Now;
            Parcels[parcelIndex] = parcel3;
            XMLTools.SaveListToXMLSerializer<ParcelDal>(Parcels, ParcelPath);
        }
        public void RemoveParcel(ParcelDal myParcel)
        {
            List<ParcelDal> Parcels = XMLTools.LoadListFromXMLSerializer<ParcelDal>(ParcelPath);
            Parcels.Remove(myParcel);
            XMLTools.SaveListToXMLSerializer<ParcelDal>(Parcels, ParcelPath);
        }
        public ParcelDal GetSingleParcel(int parcelId)
        {
            List<ParcelDal> Parcels = XMLTools.LoadListFromXMLSerializer<ParcelDal>(ParcelPath);
            int parcelIndex = Parcels.FindIndex(i => i.Id == parcelId);
            if (parcelIndex == -1)
            {
                throw new NotExistException(parcelId);
            }
            return Parcels.Find(i => i.Id == parcelId);
        }
        public IEnumerable<ParcelDal> GetParcelsList(Predicate<ParcelDal> myPredicate = null)
        {
            List<ParcelDal> Parcels = XMLTools.LoadListFromXMLSerializer<ParcelDal>(ParcelPath);
            return Parcels.Where(x => myPredicate == null ? true : myPredicate(x));
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
