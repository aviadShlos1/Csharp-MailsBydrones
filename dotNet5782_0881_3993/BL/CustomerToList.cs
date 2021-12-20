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
    /// This class presents a customer to list entity
    /// </summary>
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SentAndSuppliedParcels { get; set; } // a number of parcels which were sent and supplied
        public int SentAndNotSuppliedParcels { get; set; } // a number of parcels which were sent but weren't supplied
        public int RecieverGotParcels { get; set; } // a number of parcels which were recieved
        public int ParcelsInWayToReciever { get; set; } // a number of parcels which will be recieved (in the way)
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Phone: {Phone},SentAndSuppliedParcels: {SentAndSuppliedParcels},SentAndNotSuppliedParcels: {SentAndNotSuppliedParcels},RecieverGotParcels: {RecieverGotParcels},InTheWayParcels: {ParcelsInWayToReciever}";
        }
    }
}
