using System;
using System.Xml.Linq;
using DO;
using DalApi;

namespace Dal
{
    sealed class DalXml : DalApi.IDal
    {
        #region Singleton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }
        DalXml() { }
        public DalXml Instance { get => instance; }
        #endregion Singleton
        #region Customers
        XElement CustomerRoot;
        string CustomerPath
    }
}
