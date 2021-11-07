using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInShipment   // חבילה בהעברה
    {
        int Id;
        bool ShippingStatus;
        Priorities Priority;
        WeightCategories Weight;
        AssignCustomerToParcel Sender;
        AssignCustomerToParcel Reciever;
        Location PickUpLocation;
        Location TargetLocation;
        double shippingDistance;

    }
}
