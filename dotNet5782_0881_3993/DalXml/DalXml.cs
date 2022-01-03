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

        #region Customer
        XElement CustomerRoot;
        string CustomerPath = @"CustomerXml.xml";
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
    }
}
