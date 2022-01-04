////Names: Aviad Shlosberg       314960881      
////       Evyatar Levi Ben Ston 318753993 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal()
        {
            string dalType = DalConfig.DalName;
            string dalPkg = DalConfig.DalPackages[dalType];
            if (dalPkg == null) throw new DalConfigException($"Package {dalType} is not found in packages list in dal-config.xml") ;
            
            try { Assembly.Load(dalPkg); }
            catch (Exception) { throw new DalConfigException("Failed to load the dal-config.xml file"); }
            
            Type type = Type.GetType($"Dal.{dalPkg}, {dalPkg}");
            if (type == null) 
                throw new DalConfigException($"Class {dalPkg} was not found in the {dalPkg}.dll");
            
            IDal dal = (IDal)type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null);
            if (dal == null) 
                throw new DalConfigException($"Class {dalPkg} is not a singleton or wrong property name for Instance");
            
            return dal;
        }
    }
}

