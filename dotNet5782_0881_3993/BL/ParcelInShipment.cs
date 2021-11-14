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
        public bool ShippingStatus { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public AssignCustomerToParcel Sender { get; set; }
        public AssignCustomerToParcel Reciever { get; set; }
        public Location PickUpLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double shippingDistance { get; set; }

    }
}
