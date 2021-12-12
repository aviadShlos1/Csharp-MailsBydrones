//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//brief: In this program we built the data access layer


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace DalApi
//{
    namespace DO
    {
        /// <summary> This struct is intended for storing the DroneChargeDal details
        public struct DroneChargeDal
        {
            public int DroneId { get; set; }
            public int StationId { get; set; }
            public DateTime StartChargeTime { get; set; } //Dan Zilberstein said to add it. This field gives us the starting charge time. 

            /// </summary> This method converts the values to string values
            /// <returns> The full details by string representation  </returns>
            public override string ToString()
            {
                return $"DroneChargeDal: DroneToParcelId, StationId";
            }
        }
    }
//}