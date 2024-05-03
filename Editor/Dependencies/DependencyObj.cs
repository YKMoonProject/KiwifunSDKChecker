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
    public class DependencyObj
    {
        public string path { get; private set; }
        public Xml2CSharp.Dependencies xml { get; private set; }
        public DependencyObj(string path)
        {
            this.path = path;
        }

        private void SetXml(Xml2CSharp.Dependencies xml)
        {
            this.xml = xml;
        }

        public void Load()
        {
            xml = XMLUtility.LoadXML<Xml2CSharp.Dependencies>(path);
            SetXml(xml);
        }

        public void Save()
        {
            XMLUtility.SaveXML(xml, path);
        }

        public Dictionary<string, string> GetAndroidPkgInfo()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach(var pkg in xml.AndroidPackages.AndroidPackage) {
                string name = pkg.name;
                string version = pkg.version;
                result.Add(name, version);
            }
            return result;
        }

        public Dictionary<string, string> GetIOSPodsInfo()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if(xml.IosPods != null) {
                foreach(var pod in xml.IosPods.IosPod) {
                    string name = pod.Name;
                    string version = pod.Version;
                    result.Add(name, version);
                }
            }
            return result;
        }
    }
}