//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: In this program we added xml data files
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DalApi
{
     
    // Dal Factory Class according to the second architecture (bonus)

    /// <summary>
    /// The function creates Dal tier implementation object according to Dal type
    /// as appears in "dal" element in the configuration file dal-config.xml.>
    /// The configuration file also includes element "dal-packages" with list
    /// of available packages (.dll files) per Dal type.<br/>
    /// Each Dal package must use "Dal" namespace and it must includes internal access
    /// singleton class with the same name as package's name.<br/>
    /// The singleton class must includes public static property called "Instance"
    /// which must contains the single instance of the class.
    /// </summary>
    /// <returns>Dal tier implementation object</returns>
    public class DalFactory
    {
        public static IDal GetDal()
        {
            // gets dal implementation name from dal-config.xml according to <data> element
            string dalType = DalConfig.DalName;
            // brings package name (dll file name) for the dal name (above) from the list of packages in dal-config.xml
            string dalPkg = DalConfig.DalPackages[dalType];
            if (dalPkg == null)
                throw new DalConfigException($"Package {dalType} is not found in packages list in dal-config.xml");

            try { Assembly.Load(dalPkg); }// Load into CLR the dal implementation assembly according to dll file name (taken above)
            catch (Exception)
            { throw new DalConfigException("Failed to load the dal-config.xml file"); }


            // Get concrete Dal implementation's class metadata object
            //    first element:
            //    full class name: namespace = "Dal" or as specified in the "namespace" attribute in the config file,
            //    class name = package name or as specified in the "class" attribute in the config file
            //    the last requirement (class name = package name) is not mandatory in general - but this is the way it
            //    is configured per the implementation here, otherwise we'd need to add class name in addition to package
            //    name in the dal-config.xml file - which is clearly a good option.
            //    NB: the class may not be public - it will still be found... Our approach that the implemntation class
            //        should hold "internal" access permission (which is actually the default access permission)
            //    second element:
            //    the package name = assembly name (as above)
            Type type = Type.GetType($"DalXml.{dalPkg}, {dalPkg}");
            if (type == null) // If the type is not found - the implementation is not correct - it looks like the class name is wrong...
                throw new DalConfigException($"Class {dalPkg} was not found in the {dalPkg}.dll");


            // Get concrete Dal implementation's Instance
            // Get property info for public static property named "Instance" (in the dal implementation class- taken above)
            // If the property is not found or it's not public or not static then it is not properly implemented
            // as a Singleton...
            // Get the value of the property Instance (get function is automatically called by the system)
            // Since the property is static - the object parameter is irrelevant for the GetValue() function and we can use null
            IDal dal = (IDal)type.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public).GetValue(null);
            if (dal == null)// If the instance property is not initialized (i.e. it does not hold a real instance reference)
                throw new DalConfigException($"Class {dalPkg} is not a singleton or wrong propertry name for Instance");

            // now it looks like we have appropriate dal implementation instance
            return dal;
        }
    }
}

