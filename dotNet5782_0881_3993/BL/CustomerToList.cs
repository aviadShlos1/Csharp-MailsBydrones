//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    /// <summary>
    /// This class presents a customer to list entity
    /// </summary>
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SendAndSuppliedParcels { get; set; } // a number of parcels which were sent and supplied
        public int SendAndNotSuppliedParcels { get; set; } // a number of parcels which were sent but weren't supplied
        public int RecieverGotParcels { get; set; } // a number of parcels which were recieved
        public int ParcelsInWayToReciever { get; set; } // a number of parcels which will be recieved (in the way)
        public override string ToString()
        {
            return $"CustomerId: {Id}, CustomerName: {Name}, Phone: {Phone},SendAndSuppliedParcels: {SendAndSuppliedParcels},SendAndNotSuppliedParcels: {SendAndNotSuppliedParcels},RecieverGotParcels: {RecieverGotParcels},InTheWayParcels: {ParcelsInWayToReciever}";
        }
    }
}
