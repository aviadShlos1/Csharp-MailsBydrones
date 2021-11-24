//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//brief: In this program we built the data access layer

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
        /// ‹summary›Random field which will be used to rand details
        public static Random rand = new();

        #region The entities lists

        /// <This five rows below describe the initialize of the entities lists >
        internal static List<DroneDal> Drones = new(10) ;
        internal static List<BaseStationDal> BaseStations = new(5);
        internal static List<CustomerDal> Customers = new(100);
        internal static List<ParcelDal> Parcels = new(1000);
        internal static List<DroneCharge> DronesInCharge = new();
        #endregion The entities lists

        /// <summary> Updating the parcel amount </summary>
        internal static class Config
        {
            public static int RunId = 0;//This parameter will be updated both in Initialize and Add methods
            public static double FreeWeightConsumption = 0.06; // The power consumption for free weight drone. 
            public static double LightWeightConsumption = 0.08;
            public static double MediumWeightConsumption = 0.09;
            public static double HeavyWeightConsumption = 0.11;
            public static double ChargeRate = 50; // 50 percent for hour
        }

        private static int DroneId = default;

        /// ‹summary›This method allows us to rand objects from the enum class
        /// ‹return› A random value that exist in the enum class.‹/returns›
        public static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(rand.Next(v.Length));
        }
     
        //‹summary›This method initializes the entities details.
        public static void Initialize()
        {
            #region adding Drone details
            for (int i = 0; i < 5; i++)
            {
                Drones.Add(new DroneDal()
                {
                    Id = rand.Next(1000, 10000),
                    Model = "model:" + i.ToString(),
                    DroneWeight = RandomEnumValue<WeightCategoriesDal>()
                });
            }
            #endregion adding Drone details

            #region adding Station details
            BaseStations.Add(new BaseStationDal() { Id = 0, Name = "Herzliya DroneDal BaseStationDal", Latitude = 32.16472, Longitude = 34.84250, FreeChargeSlots = 4 });
            BaseStations.Add(new BaseStationDal() { Id = 1, Name = "Tel Aviv DroneDal BaseStationDal", Latitude = 32.056312, Longitude = 34.779888, FreeChargeSlots = 3 });
            #endregion adding Station details

            #region adding Customer details
            string[] CustomerName = { "Aviad", "Avi", "Evyatar", "Dan", "Gad", "Gal", "John", "Mike", "Eli", "Michael" };
            for (int i = 0; i < 10; i++)
            {

                Customers.Add(new CustomerDal()
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = CustomerName[rand.Next(CustomerName.Length)],
                    Phone = $"05{ rand.Next(2, 9) }{ rand.Next(1000000, 10000000) }",
                    CustomerLatitude = rand.NextDouble() * (33.4188709641265 - 29.49970431757609) + 29.49970431757609,
                    CustomerLongitude = rand.NextDouble() * (35.89927249423983 - 34.26371323423407) + 34.26371323423407,
                });
            }
            #endregion adding Customer details

            #region adding Parcel details
            for (int i = 0; i < 10; i++)
            {
                /// ‹summary›TimeSpan field which will be used to determine time
                TimeSpan time = new TimeSpan(0, rand.Next(0, 60), rand.Next(0, 60));
                Parcels.Add(new ParcelDal()
                {
                    Id = rand.Next(0, 1000),
                    SenderId = rand.Next(100000000, 1000000000),
                    TargetId = rand.Next(100000000, 1000000000),
                    Weight = RandomEnumValue<WeightCategoriesDal>(),
                    Priority = RandomEnumValue<Priorities>(),
                    CreatingTime = DateTime.Now,
                    AssignningTime = DateTime.Now + time,
                    PickingUpTime = DateTime.Now + time + time,
                    SupplyingTime = DateTime.Now + time + time + time,
                    DroneToParcelId = 0,
                });
                Config.RunId++;
            }
            #endregion adding Parcel details
        }
    };
     
}

