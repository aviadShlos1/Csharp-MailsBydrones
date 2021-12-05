//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil1
//brief: In this program we built the data access layer



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        /// <summary> This struct is intended for storing the ParcelDal details
        public struct ParcelDal
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategoriesDal Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime? CreatingTime { get => CreatingTime; set => CreatingTime = null; }//creation
            public DateTime? AssignningTime { get => AssignningTime; set => AssignningTime = null; } // relative to the drone 
            public DateTime? PickingUpTime { get => PickingUpTime; set => PickingUpTime = null; }// picked up time from the deliver
            public DateTime? SupplyingTime { get => SupplyingTime; set => SupplyingTime = null; }// customer's arrival time
            public int DroneToParcelId { get; set; }

            /// </summary> This method converts the values to string values
            /// <returns> The full details by string representation  </returns>
            public override string ToString()
            {
                return $"ParcelDal: Id:{Id}, SenderId:{SenderId}, TargetId:{TargetId}, Weight:{Weight}, Priority:{Priority}, CreatingTime:{CreatingTime}, " +
                    $"DroneToParcelId:{DroneToParcelId}, AssignningTime:{AssignningTime}, PickingUpTime:{PickingUpTime}, SupplyingTime:{SupplyingTime}";
            }
        }
    }
}
