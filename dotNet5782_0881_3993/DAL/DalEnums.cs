﻿//Names: Aviad Shlosberg       314960881      
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
        public enum WeightCategoriesDal
        {Light,Medium,Heavy}
        public enum DroneStatuses
        {Available,Maintence,Delivery}
        public enum Priorities
        { Normal,Fast,Urgent }
        public enum MainOptions
        {Add,Update,Display,ListDisplay}
    }
}
