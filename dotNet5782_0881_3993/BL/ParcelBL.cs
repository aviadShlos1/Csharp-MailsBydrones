using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelBL // חבילה
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
            return $"ParcelBL: ParcelId:{ParcelId}, AssignSenderToParcel:{Sender}, AssignRecieverToParcel:{Reciever}, ParcelWeight:{ParcelWeight}, Priority:{Priority}, DroneInShipment:{DroneAssignToParcel}, " +
                $"CreatingTime:{CreatingTime}, AssignningTime:{AssignningTime}, PickingUpTime:{PickingUpTime}, SupplyingTime:{SupplyingTime}";
        }
    }
}
