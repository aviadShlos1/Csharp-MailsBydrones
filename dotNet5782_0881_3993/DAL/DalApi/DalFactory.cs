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
    /// <summary>
    /// Static Factory class for creating Dal tier implementation object according to
    /// configuration in file config.xml
    /// </summary>
    public static class DalFactory
    {
        /// <summary>
        /// The function creates Dal tier implementation object according to Dal type
        /// as appears in "dal" element in the configuration file config.xml.<br/>
        /// The configuration file also includes element "dal-packages" with list
        /// of available packages (.dll files) per Dal type.<br/>
        /// Each Dal package must use "Dal" namespace and it must include internal access
        /// singleton class with the same name as package's name.<br/>
        /// The singleton class must include public static property called "Instance"
        /// which must contain the single instance of the class.
        /// </summary>
        /// <returns>Dal tier implementation object</returns>
        public static IDal GetDal()
        {
            // get dal implementation name from config.xml according to <data> element
            string dalType = DalConfig.DalName;
            // bring package name (dll file name) for the dal name (above) from the list of packages in config.xml
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

