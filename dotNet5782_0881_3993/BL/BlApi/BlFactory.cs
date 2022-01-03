//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//PR01 
//brief: In this program we define singleton classes, add factories and change the namespace names.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;

namespace BlApi
{
    /// <summary>
    /// This class creates bl class object and return the object 
    /// </summary>
    public static class BlFactory
    {
        public static BlApi.IBL GetBl()
        {
            BlApi.BL bl = BlApi.BL.Instance;
            return bl;
        }
            
    }
}
