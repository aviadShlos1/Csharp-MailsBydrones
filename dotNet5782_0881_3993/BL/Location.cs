using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Location // מיקום
    {
        public double longitude{ get; set; }
        public double latitude { get; set; }
        public override string ToString()
        {
            return $"Drone: longitude:{longitude},  latitude:{latitude}";
        }
    }
}
