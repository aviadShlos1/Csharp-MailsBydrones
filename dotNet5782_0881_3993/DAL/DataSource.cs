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
        public static Random rand = new();
        
        #region The entities lists
        // pay attention - we chose to work with lists, so we don't need to initialize the sizes
        internal static List<Drone> Drones = new List<Drone>(10) ;
        internal static List<Station> Stations = new List<Station>(5);
        internal static List<Customer> Customers = new List<Customer>(100);
        internal static List<Parcel> Parcels = new List<Parcel>(1000);
        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();
        #endregion The entities lists
        /// <summary> Updating the package amount </summary>
        internal struct Config
        {
            public static int RunId = 0;//This parameter will be updated both in Initialize and Add methods
        }

        private static int DroneId;

        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rand.Next(v.Length));
        }

        public static void Initialize()
        {
            #region adding Drone details
            for (int i = 0; i < 5; i++)
            {
                Drones.Add(new Drone()
                {
                    Id =rand.Next(1000,10000),
                    Model = "model:" + i.ToString(),
                    MaxWeight = RandomEnumValue<WeightCategories>(),
                    Status = RandomEnumValue<DroneStatuses>(),
                    Battery = rand.NextDouble() * 100
                });
            }
            #endregion adding Drone details

            #region adding Station details
            Stations[0] = new Station() { Id = 0 , Name = "Ramat-Gan Drone Station", Lattitude = 32.07028, Longitude = 34.82472, ChargeSlots = 4 };
            Stations[1] = new Station() { Id = 1, Name = "Tel Aviv Drone Station", Lattitude = 32.056312, Longitude = 34.779888, ChargeSlots = 3 };
            #endregion adding Station details

            #region adding Customer details
            string[] CustomerName = { "Aviad", "Avi", "Evyatar", "Dan", "Gad", "Gal", "John", "Mike", "Eli", "Michael" };
            for (int i = 0; i < 10; i++)
            {

                Customers.Add(new Customer()
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = CustomerName[rand.Next(CustomerName.Length)],
                    Phone = $"05{ rand.Next(2, 9) }{ rand.Next(1000000, 10000000) }",
                    Lattitude = rand.NextDouble() * (33.4188709641265 - 29.49970431757609) + 29.49970431757609,
                    Longitude = rand.NextDouble() * (35.89927249423983 - 34.26371323423407) + 34.26371323423407,
                });
            }
            #endregion adding Customer details

            #region adding Parcel details
            for (int i = 0; i < 10; i++)
            {
                TimeSpan time = new TimeSpan(0, rand.Next(0, 60), rand.Next(0, 60));
                Parcels.Add (new Parcel()
                {
                    Id = rand.Next(0, 1000),
                    SenderId = rand.Next(100000000, 1000000000),
                    TargetId = rand.Next(100000000, 1000000000),
                    Weight = RandomEnumValue<WeightCategories>(),
                    Priority = RandomEnumValue<Priorities>(),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now + time,
                    PickedUp = DateTime.Now + time + time,
                    Delievered = DateTime.Now + time + time + time,
                    DroneToParcel_Id = 0,
                });
                Config.RunId++;
            }
            #endregion adding Parcel details
        }
    };
     
}

