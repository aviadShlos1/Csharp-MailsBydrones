//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//brief: In this program we built the data access layer


using System;
using DalApi;


    namespace DO
    {
        /// <summary> This struct is intended for storing the CustomerDal details
        public struct CustomerDal
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            
            /// </summary> This method converts the values to string values
            /// <returns> The full details by string representation  </returns>
            public override string ToString()
            {
            string convertLongitude = DO.CoordinatesConvert.ConvertDecimalDegreesToSexagesimal(Longitude, "Longitude");
            string convertLatitude = DO.CoordinatesConvert.ConvertDecimalDegreesToSexagesimal(Longitude, "Longitude");
            return $"CustomerDal: Id:{Id}, Name:{Name}, Phone:{Phone}, Longitude:{convertLongitude}, Latitude:{convertLatitude}";
            }
        }
    }



