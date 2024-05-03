using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Text;

namespace YKMoon.SDKTools.Editor
{
    public class XMLUtility
    {
        public static T LoadXML<T>(string path)
        {
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using(FileStream stream = new FileStream(path, FileMode.Open)) {
                    return (T)serializer.Deserialize(stream);
                }
            } catch(System.Exception e) {
                UnityEngine.Debug.LogError("Exception LoadXML: " + e);

                return default(T);
            }
        }

        public static bool SaveXML<T>(T obj, string path)
        {
            try {
                XmlWriterSettings settings = new XmlWriterSettings() {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace,
                    Encoding = Encoding.UTF8,
                };
                //settings.OmitXmlDeclaration = true;
                TextWriter ms = new StreamWriter(path, false, Encoding.UTF8);
                XmlWriter writer = XmlWriter.Create(ms, settings);

                XmlSerializerNamespaces names = new XmlSerializerNamespaces();
                names.Add("","");

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj, names);
                return true;
            } catch(System.Exception e) {
                UnityEngine.Debug.LogError("Exception SaveDependencyXml: " + e);
                return false;
            }
        }

        public static bool JsonLoad<T>(string path, ref T jsonObject)
        {
            try {
                string jsonStr = GetFileStringByPath(path);
                if(string.IsNullOrEmpty(jsonStr)) {
                    jsonObject = default(T);
                    return false;
                }
                jsonObject = JsonConvert.DeserializeObject<T>(jsonStr);
                if(jsonObject == null) {
                    return false;
                }
                return true;
            } catch(System.Exception e) {
                Debug.LogException(e);
                return false;
            }
        }
        public static string GetFileStringByPath(string fullPath)
        {
            try {
                if(!File.Exists(fullPath))
                    return null;
                return Encoding.UTF8.GetString(File.ReadAllBytes(fullPath));
            } catch(Exception e) {
                Debug.LogErrorFormat("GetFileStringByPath is error : {0}", e.ToString());
            }
            return "";
        }
    }
}
