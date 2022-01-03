//Names: Aviad Shlosberg       314960881      
///       Evyatar Levi Ben Ston 318753993 
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
    /// This class presents a parcel to list entity
    /// </summary>
    public class ParcelToList 
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }
        public WeightCategoriesBL Weight { get; set; }
        public PrioritiesBL Priority { get; set; }
        public ParcelStatus ParcelStatus{ get; set; }
        public override string ToString()
        {
            return $"ParcelAssignId: {Id}, SenderName: {SenderName}, RecieverName: {RecieverName},Weight: {Weight},Priority: {Priority},ParcelStatus: {ParcelStatus}";
        }
    }
}
