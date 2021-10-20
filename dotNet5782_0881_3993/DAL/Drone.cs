using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            int Id;
            string Model;
            WeightCategories MaxWeight;
            DroneStatuses Status;
            double Battery;
        }
    }
}
