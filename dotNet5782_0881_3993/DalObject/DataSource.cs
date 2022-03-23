//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: In this program we added xml data files

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;



namespace Dal
{
    static class DataSource
    {
        //static DataSource()
        //{
        //    #region firstInitialize
        //    Initialize();
        //    string CustomerPath = @"CustomerXml.xml";
        //    string DronePath = @"DroneXml.xml";
        //    string BaseStationPath = @"BaseStationXml.xml";
        //    string ParcelPath = @"ParcelXml.xml";
        //    string DroneChargePath = @"DroneChargeXml.xml";

        //    XMLTools.SaveListToXMLSerializer(Drones, DronePath);
        //    XMLTools.SaveListToXMLSerializer(BaseStations, BaseStationPath);
        //    XMLTools.SaveListToXMLSerializer(Customers, CustomerPath);
        //    XMLTools.SaveListToXMLSerializer(Parcels, ParcelPath);
        //    XMLTools.SaveListToXMLSerializer(DronesInCharge, DroneChargePath);

        //    #endregion firstInitialize
        //}
        /// ‹summary›Random field which will be used to rand details
        public static Random rand = new();

        #region The entities lists

        /// <This five rows below describe the initialize of the entities lists >
        internal static List<DroneDal> Drones = new();
        internal static List<BaseStationDal> BaseStations = new();
        internal static List<CustomerDal> Customers = new();
        internal static List<ParcelDal> Parcels = new();
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
            BaseStations.Add(new BaseStationDal() { Id = 0, Name = "Herzliya", Latitude = 32.164, Longitude = 34.842, FreeChargeSlots = rand.Next(0, 10) });
            BaseStations.Add(new BaseStationDal() { Id = 1, Name = "Tel Aviv", Latitude = 32.056, Longitude = 34.779, FreeChargeSlots = rand.Next(0, 10) });
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
                    Longitude = (float)((float)(rand.NextDouble() * (32.2 - 31.8)) + 31.8),// get israel range (just Gush Dan)
                    Latitude = (float)((float)(rand.NextDouble() * (35.1 - 34.6)) + 34.6)//get israel range (just Gush Dan)
                    //Latitude = rand.NextDouble() * (33.418 - 29.499) + 29.499,
                    //Longitude = rand.NextDouble() * (35.899 - 34.263) + 34.263,
                });
            }
            #endregion adding Customer details

            #region adding Parcel details
            for (int i = 0; i < 10; i++)
            {
                /// ‹summary›TimeSpan field which will be used to determine time
                List<DateTime?> dates = new List<DateTime?>() { DateTime.Now, null };
                ParcelDal myParcel = new ParcelDal()
                {
                    Id = Config.RunId++,
                    SenderId = Customers[i].Id,
                    TargetId = Customers[9 - i].Id,
                    Weight = RandomEnumValue<WeightCategoriesDal>(),
                    Priority = RandomEnumValue<Priorities>(),
                    CreatingTime = DateTime.Now,
                    AssignningTime = dates[rand.Next(0, 2)],
                    PickingUpTime = dates[rand.Next(0, 2)],
                    SupplyingTime = dates[rand.Next(0, 2)],
                    DroneToParcelId = 0
            };
                if (myParcel.SupplyingTime != null)
                {
                    myParcel.AssignningTime = DateTime.Now;
                    myParcel.PickingUpTime = DateTime.Now;
                }
                else if (myParcel.PickingUpTime != null)
                {
                    myParcel.AssignningTime = DateTime.Now;
                }
                
                if (myParcel.AssignningTime != null)
                    myParcel.DroneToParcelId = Drones[rand.Next(Drones.Count)].Id;

                Parcels.Add(myParcel);             
            }
            #endregion adding Parcel details
        }
    };
}

