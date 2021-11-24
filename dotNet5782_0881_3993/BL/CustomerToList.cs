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
    public class CustomerToList   // לקוח לרשימה
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int SendAndSuppliedParcels { get; set; }
        public int SendAndNotSuppliedParcels { get; set; }
        public int RecieverGotParcels { get; set; }
        public int ParcelsInWayToReciever { get; set; }
        public override string ToString()
        {
            return $"CustomerId: {Id}, CustomerName: {Name}, Phone: {Phone},SendAndSuppliedParcels: {SendAndSuppliedParcels},SendAndNotSuppliedParcels: {SendAndNotSuppliedParcels},RecieverGotParcels: {RecieverGotParcels},InTheWayParcels: {ParcelsInWayToReciever}";
        }
    }
}
