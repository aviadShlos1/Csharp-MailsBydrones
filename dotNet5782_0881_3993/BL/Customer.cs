using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        int Id;
        string Name;
        int Phone;
        Location CustLocation;
        List<ParcelByCustomer> ParcelsFromCustomerList;
        List<ParcelByCustomer> ParcelsToCustomerList;

    }
}
