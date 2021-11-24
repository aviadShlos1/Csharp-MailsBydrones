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
    public class ParcelBl 
    {
        public int ParcelId { get; set; }
        public AssignCustomerToParcel Sender { get; set; }
        public AssignCustomerToParcel Reciever { get; set; }
        public WeightCategoriesBL ParcelWeight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public DroneInShipment DroneAssignToParcel { get; set; }
        public DateTime CreatingTime { get; set; }
        public DateTime AssignningTime { get; set; }
        public DateTime PickingUpTime { get; set; }
        public DateTime SupplyingTime { get; set; }
        public override string ToString()
        {
            return $"ParcelId: {ParcelId}, AssignSenderToParcel: {Sender}, AssignRecieverToParcel: {Reciever}, ParcelWeight: {ParcelWeight}, Priority: {Priority}, DroneInShipment: {DroneAssignToParcel}, " +
                $"CreatingTime: {CreatingTime}, AssignningTime: {AssignningTime}, PickingUpTime: {PickingUpTime}, SupplyingTime: {SupplyingTime}";
        }
    }
}
