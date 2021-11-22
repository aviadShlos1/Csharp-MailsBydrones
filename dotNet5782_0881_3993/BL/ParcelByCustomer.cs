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
        public WeightCategoriesBL Weight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public AssignCustomerToParcel SourceOrTargetMan { get; set; }
        public override string ToString()
        {
            return $"ParcelBl: CustomerId:{Id}, Weight:{Weight}, Priority:{Priority}, ParcelStatus:{Status}, SourceOrTargetMan:{SourceOrTargetMan}";
        }
    }
}
