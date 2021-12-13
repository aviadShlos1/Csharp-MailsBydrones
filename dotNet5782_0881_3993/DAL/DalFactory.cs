//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//PR01 
//brief: In this program we define singleton classes, add factories and change the namespace names.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace DalApi
{
    /// <summary>
    /// This class creates dal class object and return the object 
    /// </summary>
    public static class DalFactory
    {
        public static DalApi.IDal GetDal(string str)
        {
            DalApi.DalObject dalObject = DalApi.DalObject.Instance;   

            if (str=="DalObject")
            {
                return dalObject;
            }
            else if(str=="DalXml")
            {
                return dalObject;
            }
            else
            {
                throw new CannotCreateTheObject(str);    
            }
            
        }
    }
}
