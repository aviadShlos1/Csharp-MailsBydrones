//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// This class presents a bl customer entity
    /// </summary>
    public class CustomerBL
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public List<ParcelByCustomer> ParcelsFromCustomerList { get; set; } // parcels which were received
        public List<ParcelByCustomer> ParcelsToCustomerList { get; set; } // parcels which were sent

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Phone: {Phone},Location: {Location}"
                + "ParcelsFromCustomer: " + String.Join(",", ParcelsFromCustomerList) + "ParcelsToCustomer: " + String.Join(",", ParcelsToCustomerList)  ;
        }
    }
}
