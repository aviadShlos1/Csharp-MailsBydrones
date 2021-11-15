using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInShipment   // חבילה בהעברה
    {
        public int Id { get; set; }
        public bool ShippingStatus { get; set; } // two conditions: waiting for pickingUp or in the way for the target 
        public WeightCategoriesBL Weight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public AssignCustomerToParcel Sender { get; set; }
        public AssignCustomerToParcel Reciever { get; set; }
        public Location PickUpLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double ShippingDistance { get; set; }
        public override string ToString()
        {
            return $"ParcelInShipment: Id:{Id}, ShippingStatus:{ShippingStatus},Weight:{Weight},Priority:{Priority},AssignSenderToParcel:{Sender},AssignRecieverToParcel:{Reciever}, PickUpLocation:{PickUpLocation},TargetLocation:{TargetLocation}, ShippingDistance:{ShippingDistance}";
        }
    }
}
