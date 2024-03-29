﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// This class presents a parcel in shipment entity
    /// </summary>
    public class ParcelInShipment  
    {
        public int Id { get; set; }
        public bool ShippingOnTheSupplyWay { get; set; } // two conditions:false if waiting for pickingUp or true if it on the way for the target 
        public WeightCategoriesBL Weight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public AssignCustomerToParcel Sender { get; set; }
        public AssignCustomerToParcel Reciever { get; set; }
        public Location PickUpLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double ShippingDistance { get; set; }
        public override string ToString()
        {

            return $"Id: {Id}, ShippingOnTheSupplyWay: {ShippingOnTheSupplyWay}, Weight: {Weight}, Priority: {Priority}\n" +
                   $"Sender: {Sender}\nReciever: {Reciever}\nPickUpLocation: {PickUpLocation}\n" +
                   $"TargetLocation: {TargetLocation}\nDistance: {ShippingDistance}";
        }
    }
}
