//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: In this program we added xml data files
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
    sealed class DalXml : IDal
    {
        #region Singleton
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }

        #endregion Singleton
        //#region Singelton

        //static DalXml()// static ctor to ensure instance init is done just before first usage
        //{
        //    //DataSource.Initialize();
        //}
        //private DalXml() //private  
        //{ }

        //internal static DalXml Instance { get; } = new DalXml();// The public Instance property to use

        //#endregion Singelton


        public static string DronePath = @"DroneXml.xml";
        public static string BaseStationPath = @"BaseStationXml.xml";
        public static string ParcelPath = @"ParcelXml.xml";
        public static string DroneChargePath = @"DroneChargeXml.xml";
        public static string CustomerPath = @"CustomerXml.xml";
        //public static string Consumption = @"ConfigDetails.xml";
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
        public void AddCustomer(CustomerDal customer)
        {

            XElement element = XMLTools.LoadListFromXMLElement(CustomerPath);
            XElement customerItem = (from cus in element.Elements()
                                     where cus.Element("Id").Value == customer.Id.ToString()
                                     select cus).FirstOrDefault();
            if (customerItem != null)
            {
                throw new AlreadyExistException(customer.Id);
            }
            XElement id = new XElement("Id", customer.Id);
            XElement name = new XElement("Name", customer.Name);
            XElement phone = new XElement("Phone", customer.Phone);
            XElement longitude = new XElement("Longitude", customer.Longitude);
            XElement latitude = new XElement("Latitude", customer.Latitude);
            

            XElement cust = new XElement("CustomerDal", id, name, phone,longitude,latitude);
            element.Add(cust);
            XMLTools.SaveListToXMLElement(element, CustomerPath);
        }
        public void UpdateCustomer(CustomerDal customer)
        {
            XElement CustomerRoot = XMLTools.LoadListFromXMLElement(CustomerPath);
            XElement CustomerElement = (from item in CustomerRoot.Elements()
                                        where Convert.ToInt32(item.Element("Id").Value) == customer.Id
                                        select item).FirstOrDefault();

            CustomerElement.Element("Name").Value = customer.Name;
            CustomerElement.Element("Phone").Value = customer.Phone;

            XMLTools.SaveListToXMLElement(CustomerRoot, CustomerPath);
        }
        public CustomerDal GetSingleCustomer(int id)
        {
            XElement CustomerRoot = XMLTools.LoadListFromXMLElement(CustomerPath);
            CustomerDal Customer;
           
                Customer = (from item in CustomerRoot.Elements()
                            where Convert.ToInt32(item.Element("Id").Value) == id
                            select new CustomerDal()
                            {
                                Id = Convert.ToInt32(item.Element("Id").Value),
                                Name = item.Element("Name").Value,
                                Phone = item.Element("Phone").Value,
                                Longitude = Convert.ToDouble(item.Element("Longitude").Value),
                                Latitude = Convert.ToDouble(item.Element("Latitude").Value)
                            }
                       ).FirstOrDefault();

            if (Customer.Id!=0)
            {
                return Customer;
            }
            else
            {
                throw new NotExistException(Customer.Id);
            }
            
        }

        public IEnumerable<CustomerDal> GetCustomersList()
        {
            XElement CustomerRoot = XMLTools.LoadListFromXMLElement(CustomerPath);
            IEnumerable<CustomerDal> Customers;
            try
            {
                Customers = (from item in CustomerRoot.Elements()
                             select new CustomerDal()
                             {
                                 Id = Convert.ToInt32(item.Element("Id").Value),
                                 Name = item.Element("Name").Value,
                                 Phone = item.Element("Phone").Value,
                                 Longitude = Convert.ToDouble(item.Element("Longitude").Value),
                                 Latitude = Convert.ToDouble(item.Element("Latitude").Value)
                             }).ToList();
            }
            catch
            {
                Customers = null;
            }
            return Customers;
        }
       

        #region implemen by serializer
        //public void AddCustomer(CustomerDal newCustomer)
        //{
        //    List<CustomerDal> Customers = XMLTools.LoadListFromXMLSerializer<CustomerDal>(CustomerPath);
        //    int existIndex = Customers.FindIndex(x => x.Id == newCustomer.Id);
        //    if (existIndex != -1)
        //    {
        //        throw new AlreadyExistException(newCustomer.Id);
        //    }
        //    Customers.Add(newCustomer);
        //    XMLTools.SaveListToXMLSerializer<CustomerDal>(Customers, CustomerPath);
        //}
        //public void UpdateCustomer(CustomerDal myCustomer)
        //{
        //    List<CustomerDal> Customers = XMLTools.LoadListFromXMLSerializer<CustomerDal>(CustomerPath);
        //    CustomerDal tempCustomer = Customers.FirstOrDefault(x => x.Id == myCustomer.Id);
        //    int index = Customers.IndexOf(tempCustomer);
        //    Customers[index] = myCustomer;
        //    XMLTools.SaveListToXMLSerializer<CustomerDal>(Customers, CustomerPath);
        //}
        //public CustomerDal GetSingleCustomer(int customerId)
        //{
        //    List<CustomerDal> Customers = XMLTools.LoadListFromXMLSerializer<CustomerDal>(CustomerPath);
        //    int customerIndex = Customers.FindIndex(i => i.Id == customerId);
        //    if (customerIndex == -1)
        //    {
        //        throw new NotExistException(customerId);
        //    }
        //    return Customers.Find(i => i.Id == customerId);
        //}
        //public IEnumerable<CustomerDal> GetCustomersList()
        //{
        //    List<CustomerDal> Customers = XMLTools.LoadListFromXMLSerializer<CustomerDal>(CustomerPath);
        //    return Customers;
        //}
        #endregion implemen by serializer
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
            //loading the "ConfigDetails" file to update the runId
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
