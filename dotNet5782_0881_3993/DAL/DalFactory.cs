using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace DAL
{
    public static class DalFactory
    {
        public static DalApi.IDal GetDal(string str)
        {
            DalObject.DalObject dalObject = new();

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
