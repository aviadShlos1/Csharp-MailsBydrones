using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    struct DataSource
    {
        internal static IDAL.DO.Drone[] Drones = new IDAL.DO.Drone[10];
        internal static IDAL.DO.Station[] Stations = new IDAL.DO.Station[5];
        internal static IDAL.DO.Customer[] Customers = new IDAL.DO.Customer[100];
        internal static IDAL.DO.Parcel[] Parcels = new IDAL.DO.Parcel[1000];
        internal struct Config 
        {
            internal static int DroneIndex = 0;
            internal static int StationIndex = 0;
            internal static int CustomerIndex = 0;
            internal static int ParcelIndex = 0;
            public int RunId;
        }

        public static Random r = new();
        public static T RandomEnumValue <T> ()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(r.Next(v.Length));
        }

        public static void Initialize()
        {
            for (int i = 0; i < 5; i++)
            {
                Drones[i] = new Drone()
                {
                    Id = r.Next(100, 1000),  // 3 digits
                    Model = "model:" + i.ToString(),
                    MaxWeight = RandomEnumValue <WeightCategories>(),
                    Status = RandomEnumValue <DroneStatuses>(),
                    Battery = r.NextDouble() * 100
                };

            }

            Stations[0] = new Station() { Id = Config.StationIndex++, Name = "Haifa Drone Station", Lattitude = 32.794044, Longitude = 34.989571, ChargeSlots = 4 };
            Stations[1] = new Station() { Id = Config.StationIndex++, Name = "Tel Aviv Central Station", Lattitude = 32.056312, Longitude = 34.779888, ChargeSlots = 3 };
           
            for (int i = 0; i < 10; i++)
            {
                Customers[i].Id = r.Next(100000000, 1000000000); // 9 digits for valid id
                Customers[i].Phone = $"0{r.Next(50, 59)}{r.Next(1000000, 10000000)}";
            }

            for (int i = 0; i < 10; i++)
            {
                Parcels[i].Id = r.Next(1000, 10000); // 9 digits for valid id
            }

        }
    };
}
