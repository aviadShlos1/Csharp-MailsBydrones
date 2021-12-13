using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace DalApi
{
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
