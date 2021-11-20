using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerBL
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public Location CustomerLocation { get; set; }
        public List<ParcelByCustomer> ParcelsFromCustomerList { get; set; }
        public List<ParcelByCustomer> ParcelsToCustomerList { get; set; }

        public override string ToString()
        {
            return $"CustomerBL: CustomerId:{CustomerId}, CustomerName:{CustomerName}, Phone:{CustomerPhone},Location:{CustomerLocation}"
                + "ParcelsFromCustomer:" + String.Join(",", ParcelsFromCustomerList) + "ParcelsToCustomer:" + String.Join(",", ParcelsToCustomerList)  ;
        }
    }
}
