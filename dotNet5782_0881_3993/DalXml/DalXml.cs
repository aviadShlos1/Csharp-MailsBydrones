using System;
using DO;
using DalApi;

namespace Dal
{
    sealed class DalXml:DalApi.IDal
    {
        #region Singleton
        static readonly DalXml instance = new DalXml();
        static DalXml() { }
        DalXml() { }
        public DalXml Instance { get=>instance;  }
        #endregion Singleton
    }
}
