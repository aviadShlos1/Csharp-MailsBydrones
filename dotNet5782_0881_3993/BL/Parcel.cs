using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel // חבילה
    {
        public int ParcelId { get; set; }
        public AssignCustomerToParcel Sender { get; set; }
        public AssignCustomerToParcel Reciever { get; set; }
        public WeightCategories ParcelWeight { get; set; }
        public Priorities Priority { get; set; }
        public DroneInShipment DInShip { get; set; }
        public DateTime Creating { get; set; }
        public DateTime Assignning { get; set; }
        public DateTime PickingUp { get; set; }
        public DateTime Supplying { get; set; }
        public override string ToString()
        {
            return $"Parcel: ParcelId:{ParcelId}, Sender:{Sender}, Reciever:{Reciever}, ParcelWeight:{ParcelWeight}, Priority:{Priority}, Creating:{Creating}, " +
                $"DroneInShipment:{DInShip}, Assignning:{Assignning}, PickingUp:{PickingUp}, Supplying:{Supplying}";
        }
    }
}
