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
        public struct Station
        {
            public int Id { get; set; }
            public string Name{ get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int ChargeSlots { get; set; } // empty chargeSlots
            public override string ToString()
            {
                return $" Id:{Id}, Name:{Name}, ChargeSlots:{ChargeSlots}, Longitude:{Longitude}, Lattitude:{Lattitude}";
            }
        }
    }
}
