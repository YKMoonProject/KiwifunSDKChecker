/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace YKMoon.SDKTools.Editor.GvhXML
{
    [XmlRoot(ElementName = "projectSetting")]
    public class ProjectSetting
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "projectSettings")]
    public class ProjectSettings
    {
        [XmlElement(ElementName = "projectSetting")]
        public List<ProjectSetting> ProjectSetting { get; set; }

        public string GetValue(string name)
        {
            foreach(var set in ProjectSetting) {
                if(!string.IsNullOrEmpty(set.Name) && set.Name.Equals(name)) {
                    return set.Value;
                }
            }
            return "";
        }
        public bool SetValue(string name, string value)
        {
            foreach(var set in ProjectSetting) {
                if(!string.IsNullOrEmpty(set.Name) && set.Name.Equals(name)) {
                    set.Value = value;
                    return true;
                }
            }
            return false;
        }
    }
}