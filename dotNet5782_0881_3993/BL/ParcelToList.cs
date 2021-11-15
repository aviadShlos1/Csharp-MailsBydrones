using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelToList // חבילה לרשימה
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }
        public WeightCategoriesBL Weight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public ParcelStatus ParcelStatus{ get; set; }
        public override string ToString()
        {
            return $"ParcelToList: Id:{Id}, SenderName:{SenderName}, RecieverName:{RecieverName},Weight:{Weight},Priority:{Priority},ParcelStatus:{ParcelStatus}";
        }
    }
}
