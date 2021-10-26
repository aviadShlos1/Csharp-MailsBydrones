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

        public static Random r = new();

        
        public static void Initialize()
        {
            for (int i = 0; i < 5; i++)
            {
                drones[i].Id = r.Next(100, 1000);   // 3 digits
                drones[i] = new Drone()
                {
                    Id = r.Next(100, 1000),  // 3 digits
                    Model = "mod" + i.ToString(),
                    MaxWeight = RandomEnumValue<WeightCategories>(),
                    Status = RandomEnumValue<DroneStatusCategories>(),
                    Battery = r.NextDouble() * 100
                };

            }

            for (int i = 0; i < 2; i++)
            {
                stations[i].Id = r.Next(1000, 10000); // 4 digits 
            }

            for (int i = 0; i < 10; i++)
            {
                customers[i].Id = r.Next(100000000, 1000000000); // 9 digits for valid id
                customers[i].Phone = $"0{r.Next(50, 59)}{r.Next(1000000, 10000000)}";
            }

            for (int i = 0; i < 10; i++)
            {
                parcels[i].Id = r.Next(1000, 10000); // 9 digits for valid id
            }

        }
    };
}
