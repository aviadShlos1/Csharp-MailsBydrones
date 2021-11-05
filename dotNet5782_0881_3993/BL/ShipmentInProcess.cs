using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ShipmentInProcess
    {
        int Id;
        WeightCategories Weight;
        Priorities Priority;
        bool ShippingStatus;
        Location PickUpLocation;
        Location TargetLocation;
        double shippingDistance;

    }
}
