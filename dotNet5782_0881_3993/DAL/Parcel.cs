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
        /// <summary> This struct is intended for storing the Parcel details
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategoriesDal Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Requested { get; set; } //creation
            public DateTime Scheduled { get; set; } // relative to the drone 
            public DateTime PickedUp { get; set; } // picked up time from the deliver
            public DateTime Delievered { get; set; } // customer's arrival time
            public int DroneToParcel_Id { get; set; }

            /// </summary> This method converts the values to string values
            /// <returns> The full details by string representation  </returns>
            public override string ToString()
            {
                return $"Parcel: Id:{Id}, SenderId:{SenderId}, TargetId:{TargetId}, Weight:{Weight}, Priority:{Priority}, Requested:{Requested}, " +
                    $"DroneToParcel_Id:{DroneToParcel_Id}, Scheduled:{Scheduled}, PickedUp:{PickedUp}, Delievered:{Delievered}";
            }
        }
    }
}
