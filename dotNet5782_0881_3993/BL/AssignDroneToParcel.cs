//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
  /// <summary>
  /// This class presents an assign drone to parcel entity
  /// </summary>
    public class AssignDroneToParcel
    {
        public int Id { get; set; }
        public double BatteryPercent { get; set; }
        public Location Current { get; set; }

        public override string ToString()
        {
            return $" CustomerId: {Id}, BatteryPercent: {BatteryPercent}, Location: {Current}";
        }

    }
}
