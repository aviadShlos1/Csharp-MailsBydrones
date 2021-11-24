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
    /// This class presents a parcel by customer entity
    /// </summary>
    public class ParcelByCustomer
    {
        public int Id { get; set; }
        public WeightCategoriesBL Weight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public AssignCustomerToParcel SourceOrTargetMan { get; set; } // This field can contain the source or the targed, corresponding to the situation
        public override string ToString()
        {
            return $"ParcelId: {Id}, Weight: {Weight}, Priority: {Priority}, ParcelStatus: {Status}, SourceOrTargetMan: {SourceOrTargetMan}";
        }
    }
}
