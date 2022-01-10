//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: In this program we added xml data files
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal
{
    class XMLTools
    {
        static string dir = @"xml\";
        static XMLTools()
        { 
            //checks if the file directory exists
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir); 
        }

        // Region for save and load function with are implement by serializer
        #region SaveLoadWithXmlSerializer
        /// <summary>
        /// function for save data in a xml file using serializer implementation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">the list to save</param>
        /// <param name="filePath">the file path</param>
        public static void SaveListToXMLSerializer<T>(List<T> list, string filePath)
        {
            try
            {
                FileStream file = new FileStream(dir + filePath, FileMode.Create);
                XmlSerializer x = new XmlSerializer(list.GetType());
                x.Serialize(file, list);
                file.Close();
            }
            catch (Exception ex)
            {
                throw new DalApi.DalConfigException($"fail to create xml file:{filePath}", ex);
            }
        }
        /// <summary>
        /// function for load data from a xml file using serializer implementation
        /// </summary>
        /// <typeparam name="T">the type of the list</typeparam>
        /// <param name="filePath">the file path</param>
        /// <returns></returns>
        public static List<T> LoadListFromXMLSerializer<T>(string filePath)
        {
            try
            {

                if (File.Exists(dir + filePath))
                {
                    List<T> list;
                    XmlSerializer x = new XmlSerializer(typeof(List<T>));
                    FileStream file = new FileStream(dir + filePath, FileMode.Open);
                    list = (List<T>)x.Deserialize(file);
                    file.Close();
                    return list;
                }
                else
                    return new List<T>();
            }
            catch (Exception ex)
            {

                throw new DalApi.DalConfigException($"fail to load xml file:{filePath}", ex);
            }
        }

        #endregion

        // Region for save and load function with are implement by XElement
        #region SaveLoadWithXElement

        /// <summary>
        /// function for save data to a xml file using XElement implementation
        /// </summary>
        /// <param name="rootElem"></param>
        /// <param name="filePath"></param>
        public static void SaveListToXMLElement(XElement rootElem, string filePath)
        {
            try
            {
                rootElem.Save(dir + filePath);
            }
            catch (Exception ex)
            {
                throw new DalApi.DalConfigException($"fail to create xml file:{filePath}", ex);
            }
        }
        /// <summary>
        /// function for load data from a xml file using XElement implementation
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static XElement LoadListFromXMLElement(string filePath)
        {
            try
            {

                if (File.Exists(dir + filePath))
                {
                    return XElement.Load(dir + filePath);
                }
                else
                { 
                    XElement rootElem = new XElement(dir + filePath);
                    rootElem.Save(dir + filePath);
                    return rootElem;
                }

            }
            catch (Exception ex)
            {

                throw new DalApi.DalConfigException($"fail to load xml file:{filePath}", ex);
            }
        }
        #endregion
    }
}
