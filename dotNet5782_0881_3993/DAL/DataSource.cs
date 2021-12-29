//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//brief: In this program we built the data access layer

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    static class DataSource
    {
        /// ‹summary›Random field which will be used to rand details
        public static Random rand = new();

        #region The entities lists

        /// <This five rows below describe the initialize of the entities lists >
        internal static List<DroneDal> Drones = new(10) ;
        internal static List<BaseStationDal> BaseStations = new(5);
        internal static List<CustomerDal> Customers = new(100);
        internal static List<ParcelDal> Parcels = new(1000);
        internal static List<DroneChargeDal> DronesInCharge = new();
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

        //private static int DroneId = default;

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
                    Model = i.ToString(),
                    DroneWeight = RandomEnumValue<WeightCategoriesDal>() 
                });
            }
            #endregion adding Drone details

            #region adding Station details
            BaseStations.Add(new BaseStationDal() { Id = 0, Name = "Herzliya", Latitude = 32.164, Longitude = 34.842, FreeChargeSlots = 8/*rand.Next(0, 10)*/ });
            BaseStations.Add(new BaseStationDal() { Id = 1, Name = "Tel Aviv", Latitude = 32.056, Longitude = 34.779, FreeChargeSlots = 8/*rand.Next(0, 10)*/ });
            #endregion adding Station details

            #region adding Customer details
            string[] CustomerName = { "Aviad", "Avi", "Evyatar", "Dan", "Gad", "Gal", "John", "Mike", "Eli", "Michael" };
            for (int i = 0; i < 10; i++)
            {

                Customers.Add(new CustomerDal()
                {
                    Id = rand.Next(100000000, 1000000000),
                    Name = CustomerName[i],
                    Phone = $"05{ rand.Next(2, 9) }{ rand.Next(1000000, 10000000) }",
                    Latitude = rand.NextDouble() * (33.418 - 29.499) + 29.499,
                    Longitude = rand.NextDouble() * (35.899 - 34.263) + 34.263,
                });
            }
            #endregion adding Customer details

            #region adding Parcel details
            for (int i = 0; i < 10; i++)
            {
                /// ‹summary›TimeSpan field which will be used to determine time
                TimeSpan time = new TimeSpan(0, rand.Next(0, 24), rand.Next(0, 60), 0);
                ParcelDal myParcel = new ParcelDal()
                {
                    Id = Config.RunId++,
                    SenderId = Customers[rand.Next(0,10)].Id,
                    TargetId = Customers[rand.Next(0,10)].Id,
                    Weight = RandomEnumValue<WeightCategoriesDal>(),
                    Priority = RandomEnumValue<Priorities>(),
                    CreatingTime = DateTime.Now,
                    AssignningTime = null,
                    PickingUpTime = null,
                    SupplyingTime = null,
                    DroneToParcelId = 0,
                };


                // lists 107-127: rand parcels details and rand assign drone , in that the system simulates real time system 
                if (rand.Next(2) == 1)
                {
                    myParcel.AssignningTime = myParcel.CreatingTime + time;
                    myParcel.DroneToParcelId = Drones[rand.Next(Drones.Count)].Id;
                    if (rand.Next(2) == 1)
                    {
                        myParcel.PickingUpTime = myParcel.AssignningTime + time;
                        if (rand.Next(2) == 1)
                            myParcel.SupplyingTime = myParcel.PickingUpTime + time;
                        else
                            myParcel.SupplyingTime = null;
                    }
                    else
                        myParcel.PickingUpTime = myParcel.SupplyingTime = null;
                }
                else
                {
                    myParcel.AssignningTime = myParcel.PickingUpTime = myParcel.SupplyingTime = null;
                    myParcel.DroneToParcelId = 0;
                }
                Parcels.Add(myParcel);
            }
            #endregion adding Parcel details
        }
    };    
}

