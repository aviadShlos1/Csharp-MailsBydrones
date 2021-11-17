//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        /// <summary> This struct is intended for BaseStation the Drone details
        public struct BaseStation
        {
            public int Id { get; set; }
            public string Name{ get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public int ChargeSlots { get; set; } // empty chargeSlots

            /// </summary> This method converts the values to string values
            /// <returns> The full details by string representation  </returns>
            public override string ToString()
            {
                return $" Id:{Id}, Name:{Name}, ChargeSlots:{ChargeSlots}, CustomerLongitude:{Longitude}, CustomerLatitude:{Latitude}";
            }
        }
    }
}
