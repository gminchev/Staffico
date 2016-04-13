using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Common
{
    /// <summary>
    /// Helper for xml serialization
    /// </summary>
    public static class XmlSerializationHelper
    {
        /// <summary>
        /// Serializes to XML file.
        /// </summary>
        /// <param name="objectToSerialize">object to serialize</param>
        /// <param name="filename">The filename.</param>
        public static void SerializeToXmlFile(object objectToSerialize, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType());

            #region Check Directory
            {
                string dir = Path.GetDirectoryName(filename);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            #endregion

            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                serializer.Serialize(stream, objectToSerialize);
                stream.Flush();
                stream.Close();
            }
        }

        /// <summary>
        /// Serializes to XML string.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns></returns>
        public static string SerializeToXml(object objectToSerialize)
        {
            StringWriter stringWriter = new StringWriter(new StringBuilder());
            XmlSerializer xmlSerializer = new XmlSerializer(objectToSerialize.GetType());
            xmlSerializer.Serialize(stringWriter, objectToSerialize);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Serializes to XML chunk string.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns></returns>
        public static string SerializeToXmlChunk(object objectToSerialize)
        {
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };

            using (StringWriter stringWriter = new StringWriter(new StringBuilder()))
            using (var writer = XmlWriter.Create(stringWriter, settings))
            {
                var xmlSerializer = new XmlSerializer(objectToSerialize.GetType());
                xmlSerializer.Serialize(writer, objectToSerialize, emptyNamepsaces);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Deserializes an object from XML file.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static object DeserializeFromXmlFile(Type type, string filename)
        {
            if (!File.Exists(filename))
            {
                throw new IOException(string.Format("File '{0}' does not exist", filename));
            }

            XmlSerializer serializer = new XmlSerializer(type);

            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    return serializer.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    // The serialization error messages are cryptic at best.Give a hint at what happened
                    throw new InvalidOperationException(string.Format("Failed to create object from '{0}'", filename), ex);
                }
            }
        }

        /// <summary>
        /// Deserializes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="serializationText">The serialization text.</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string serializationText)
        {
            StringReader stringWriter = new StringReader(serializationText);
            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(stringWriter);
        }

        ///<summary>
        /// Deserialize seriazible object from string
        ///</summary>
        ///<param name="sourceXml"></param>
        ///<typeparam name="T"></typeparam>
        ///<returns></returns>
        public static T FromXML<T>(string sourceXml)
            where T : class, new()
        {
            T result;

            if (!string.IsNullOrEmpty(sourceXml))
            {
                try
                {
                    // Create an instance of the XmlSerializer class;
                    // specify the type of object to be deserialized.
                    var serializer = new XmlSerializer(typeof(T));
                    using (var sr = new StringReader(sourceXml))
                    {
                        using (XmlReader xrRoot = XmlReader.Create(sr))
                        {
                            object deserialized = serializer.Deserialize(xrRoot);
                            result = deserialized as T;
                        }
                    }
                }
                catch (Exception)
                {
                    result = new T();
                }
            }
            else
            {
                result = new T();
            }

            return result;
        }

        ///<summary>
        /// Serialize serializable object to XML.
        ///</summary>
        ///<returns></returns>
        public static string ToXml(object obj)
        {
            var sbHelper = new StringBuilder();

            //  Serialize data
            using (XmlWriter writer = XmlWriter.Create(sbHelper))
            {
                var xrModule = new XmlSerializer(obj.GetType());
                xrModule.Serialize(writer, obj);
                writer.Close();
            }

            return sbHelper.ToString();
        }

        /// <summary>
        /// Serialize Object from generic type
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string SerializeObject<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, obj);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Deserialized Object to generic type
        /// </summary>
        /// <param name="xml"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeserializedObject<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (TextReader strReader = new StringReader(xml))
            {
                using (XmlReader reader = XmlReader.Create(strReader))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
        }
    }
}
