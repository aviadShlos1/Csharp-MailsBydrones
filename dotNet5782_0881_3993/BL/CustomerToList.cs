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
