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
    /// This class presents a parcel bl entity
    /// </summary>
    public class ParcelBl
    {
        public int ParcelId { get; set; }
        public AssignCustomerToParcel Sender { get; set; }
        public AssignCustomerToParcel Reciever { get; set; }
        public WeightCategoriesBL ParcelWeight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public DroneInShipment DroneAssignToParcel { get; set; }
        public DateTime? CreatingTime { get => CreatingTime; set => CreatingTime = null; }
        public DateTime? AssignningTime { get => AssignningTime; set => AssignningTime = null; }
        public DateTime? PickingUpTime { get => PickingUpTime; set => PickingUpTime = null; }
        public DateTime? SupplyingTime { get => SupplyingTime; set => SupplyingTime = null; }
        public override string ToString()
        {
            return $"ParcelId: {ParcelId}, AssignSenderToParcel: {Sender}, AssignRecieverToParcel: {Reciever}, ParcelWeight: {ParcelWeight}, Priority: {Priority}, DroneInShipment: {DroneAssignToParcel}, " +
                $"CreatingTime: {CreatingTime}, AssignningTime: {AssignningTime}, PickingUpTime: {PickingUpTime}, SupplyingTime: {SupplyingTime}";
        }
    }
}
