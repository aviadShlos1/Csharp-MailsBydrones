using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public Location CustLocation { get; set; }
        public List<ParcelByCustomer> ParcelsFromCustomerList { get; set; }
        public List<ParcelByCustomer> ParcelsToCustomerList { get; set; }

        public override string ToString()
        {
            return $"Customer: Id:{Id}, Name:{Name}, Phone:{Phone},Location:{CustLocation}";
        }
    }
}
