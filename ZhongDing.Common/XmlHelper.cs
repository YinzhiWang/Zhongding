using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ZhongDing.Common
{
    /// <summary>
    /// 类：XML处理
    /// </summary>
    public class XmlHelper
    {

        /// <summary>
        /// The pretty print
        /// </summary>
        private static bool _PrettyPrint;

        /// <summary>
        /// Gets or sets a value indicating whether [pretty print].
        /// </summary>
        /// <value><c>true</c> if [pretty print]; otherwise, <c>false</c>.</value>
        public static bool PrettyPrint
        {
            get { return _PrettyPrint; }
            set { _PrettyPrint = value; }
        }

        /// <summary>
        /// Gets the namespaces.
        /// </summary>
        /// <returns>XmlSerializerNamespaces.</returns>
        public static XmlSerializerNamespaces GetNamespaces()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            return ns;
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="Obj">The obj.</param>
        /// <param name="ObjType">Type of the obj.</param>
        /// <returns>System.String.</returns>
        public static string ToXml(object Obj, System.Type ObjType)
        {
            XmlSerializer ser = null;
            ser = new XmlSerializer(ObjType);

            MemoryStream memStream = null;
            memStream = new MemoryStream();
            XmlTextWriter xmlWriter = null;
            xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
            if (XmlHelper.PrettyPrint)
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 1;
                xmlWriter.IndentChar = Convert.ToChar(9);
            }
            xmlWriter.Namespaces = true;
            ser.Serialize(xmlWriter, Obj, XmlHelper.GetNamespaces());
            xmlWriter.Close();
            //memStream.Close();
            string xml = null;
            xml = Encoding.UTF8.GetString(memStream.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));

            return xml;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="Xml">The XML.</param>
        /// <param name="ObjType">Type of the obj.</param>
        /// <returns>System.Object.</returns>
        public static object FromXml(string Xml, System.Type ObjType)
        {
            XmlSerializer ser = null;
            ser = new XmlSerializer(ObjType);
            StringReader stringReader = null;
            stringReader = new StringReader(Xml);
            XmlTextReader xmlReader = null;
            xmlReader = new XmlTextReader(stringReader);
            object obj = null;
            obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            //stringReader.Close();
            return obj;
        }


        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="Obj">The obj.</param>
        /// <param name="ObjType">Type of the obj.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="theNamespaces">The namespaces.</param>
        public static void SaveToXml(object Obj, System.Type ObjType, string filePath, XmlSerializerNamespaces theNamespaces = null)
        {
            if (string.IsNullOrEmpty(filePath) || Obj == null)
                return;

            XmlSerializer ser = null;
            ser = new XmlSerializer(ObjType);

            FileStream fileStream = null;
            fileStream = new FileStream(filePath, FileMode.CreateNew);

            XmlTextWriter xmlWriter = null;
            xmlWriter = new XmlTextWriter(fileStream, Encoding.UTF8);
            if (XmlHelper.PrettyPrint)
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.Indentation = 1;
                xmlWriter.IndentChar = Convert.ToChar(9);
            }
            xmlWriter.Namespaces = true;

            if (theNamespaces == null)
            {
                theNamespaces = GetNamespaces();
            }

            ser.Serialize(xmlWriter, Obj, XmlHelper.GetNamespaces());
            xmlWriter.Close();
            //fileStream.Close();
        }

        /// <summary>
        /// Serializes the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="obj">The obj.</param>
        public static void Serialize(string filePath, object obj)
        {
            if (string.IsNullOrEmpty(filePath) || obj == null)
            {
                return;
            }

            try
            {
                XmlSerializerFactory xmlSerializerFactory = new XmlSerializerFactory();
                XmlSerializer xmlSerializer = xmlSerializerFactory.CreateSerializer(obj.GetType(), obj.GetType().Name);
                Stream stream = new FileStream(filePath, FileMode.Create);
                xmlSerializer.Serialize(stream, obj);
                stream.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Serializes the two.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>System.String.</returns>
        public static string SerializeTwo(object obj)
        {
            if (obj == null)
            {
                return "";
            }

            try
            {
                string returnStr = "";
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                MemoryStream ms = new MemoryStream();
                XmlTextWriter xtw = null;
                StreamReader sr = null;
                try
                {
                    xtw = new System.Xml.XmlTextWriter(ms, Encoding.UTF8);
                    xtw.Formatting = System.Xml.Formatting.Indented;
                    serializer.Serialize(xtw, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    sr = new StreamReader(ms);
                    returnStr = sr.ReadToEnd();
                }
                catch
                {
                    //throw ex;
                }
                finally
                {
                    if (xtw != null)
                    {
                        xtw.Close();
                    }
                    if (sr != null)
                    {
                        //sr.Close();
                    }
                    //ms.Close();
                }
                return returnStr;
            }
            catch
            {
            }
            return "";
        }
    }
}
