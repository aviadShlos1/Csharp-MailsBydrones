﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;

namespace IBL.BO
{
    public class AssignCustomerToParcel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"CustomerId: {Id}, CustomerName: {Name}";
        }
    }
}
