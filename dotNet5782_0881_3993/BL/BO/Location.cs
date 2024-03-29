﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DO;


namespace BO
{
    /// <summary>
    /// This class presents a location entity
    /// </summary>
    public class Location 
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            string convertLongitude = DO.CoordinatesConvert.ConvertDecimalDegreesToSexagesimal(Longitude, "Longitude");
            string convertLatitude = DO.CoordinatesConvert.ConvertDecimalDegreesToSexagesimal(Latitude, "Latitude");
            return $"Longitude: {convertLongitude}\n" +
                   $"Latitude: {convertLatitude}";
        }
    }
}
