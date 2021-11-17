//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1


using System;

namespace IDAL
{
    namespace DO
    {
        /// <summary> This struct is intended for storing the Customer details
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double CustomerLongitude { get; set; }
            public double CustomerLatitude { get; set; }
            
            /// </summary> This method converts the values to string values
            /// <returns> The full details by string representation  </returns>
            public override string ToString()
            {
                return $"Customer: Id:{Id}, Name:{Name}, Phone:{Phone}, CustomerLongitude:{CustomerLongitude}, CustomerLatitude:{CustomerLatitude}";
            }
        }
    }
}


