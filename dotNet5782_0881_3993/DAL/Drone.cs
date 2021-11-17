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
        /// <summary> This struct is intended for storing the Drone details
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategoriesDal DroneWeight { get; set; }
            
            /// </summary> This method converts the values to string values
            /// <returns> The full details by string representation  </returns>
            public override string ToString()
            {
                return $"Drone: Id:{Id}, Model:{Model}, DroneWeight:{DroneWeight}";
            }
        }
    }
}
