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
        public string MyProperty { get; set; }
        public int Name { get; set; }
        public int Phone { get; set; }
        public int SendAndSuppliedParcels { get; set; }
        public int SendAndDontSuppliedParcels { get; set; }
        public int GotParcels { get; set; }
        public int InTheWayParcels { get; set; }
    }
}
