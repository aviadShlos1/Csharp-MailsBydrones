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
            int Id;
            int senderId;
            int targetId;
            WeightCategories Weight;
            Priorities Priority;
            DateTime Requested;
            int DroneId;
            DateTime Scheduled;
            DateTime PickedUp;
            DateTime Delievered;
        }
    }
}
