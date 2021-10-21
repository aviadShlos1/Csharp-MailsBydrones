using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    struct DataSource
    {
        internal static IDAL.DO.Drone[] drones = new IDAL.DO.Drone[10];
        internal static IDAL.DO.Station[] stations = new IDAL.DO.Station[5];
        internal static IDAL.DO.Customer[] customers = new IDAL.DO.Customer[100];
        internal static IDAL.DO.Parcel[] parcels = new IDAL.DO.Parcel[1000];
        internal struct Config 
        {
            internal static int indexDrone = 0;
            internal static int indexStation = 0;
            internal static int indexCustomer = 0;
            internal static int indexParcel = 0;
            public int RunId;
        }
    }
}
