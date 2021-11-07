using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel // חבילה
    {
        int ParcelId;
        AssignCustomerToParcel Sender;
        AssignCustomerToParcel Reciever;
        WeightCategories ParcelWeight;
        Priorities Priority;
        DroneInShipment DInShip;
        DateTime Creating;
        DateTime Assignning;
        DateTime PickingUp;
        DateTime Supplying;


    }
}
