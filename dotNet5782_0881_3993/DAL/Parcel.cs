using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{

    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime Requested { get; set; } //creation
            public DateTime Scheduled { get; set; } // relative to the drone 
            public DateTime PickedUp { get; set; } // picked up time from the deliver
            public DateTime Delievered { get; set; } // customer's arrival time
            public int DroneId { get; set; }
            public override string ToString()
            {
                return $"Parcel: Id:{Id}, SenderId{SenderId}, TargetId{TargetId}, Weight{Weight}, Priority{Priority}, Requested{Requested}, " +
                    $"DroneId{DroneId}, Scheduled{Scheduled}, PickedUp{PickedUp}, Delievered{Delievered}";
            }
        }
    }
}
