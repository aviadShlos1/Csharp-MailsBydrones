//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil2
//brief: In this program we built the logic business layer
using System;

namespace BO
{
    /// <summary>
    /// This class presents an assign customer entity
    /// </summary>
    public class AssignCustomerToParcel
    {
        public int CustId { get; set; }
        public string CustName { get; set; }

        public override string ToString()
        {
            return $"CustId: {CustId}, CustName: {CustName}";
        }
    }
}
