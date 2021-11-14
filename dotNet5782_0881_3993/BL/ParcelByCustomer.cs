using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelByCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public AssignCustomerToParcel SourceOrTargetMan { get; set; }
        public override string ToString()
        {
            return $"Parcel: Id:{Id}, Weight:{Weight}, Priority:{Priority}, Status:{Status}, SourceOrTargetMan:{SourceOrTargetMan}";
        }
    }
}
