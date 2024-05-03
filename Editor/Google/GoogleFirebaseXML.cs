       /* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
    */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
#region Auto
namespace YKMoon.SDKTools.Editor.GoogleFirebaseXML
{
    [XmlRoot(ElementName = "string")]
    public class String
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "translatable")]
        public string Translatable { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "resources")]
    public class Resources
    {
        [XmlElement(ElementName = "string")]
        public List<String> String { get; set; }
        [XmlAttribute(AttributeName = "tools", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Tools { get; set; }
        [XmlAttribute(AttributeName = "keep", Namespace = "http://schemas.android.com/tools")]
        public string Keep { get; set; }
    }
}

namespace YKMoon.SDKTools.Editor.GoogleFirebaseJson
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AndroidClientInfo
    {
        public string package_name { get; set; }
    }

    public class ApiKey
    {
        public string current_key { get; set; }
    }

    public class AppinviteService
    {
        public List<object> other_platform_oauth_client { get; set; }
    }

    public class Client
    {
        public ClientInfo client_info { get; set; }
        public List<object> oauth_client { get; set; }
        public List<ApiKey> api_key { get; set; }
        public Services services { get; set; }
    }

    public class ClientInfo
    {
        public string mobilesdk_app_id { get; set; }
        public AndroidClientInfo android_client_info { get; set; }
    }

    public class ProjectInfo
    {
        public string project_number { get; set; }
        public string project_id { get; set; }
        public string storage_bucket { get; set; }
    }

    public class Root
    {
        public ProjectInfo project_info { get; set; }
        public List<Client> client { get; set; }
        public string configuration_version { get; set; }
    }

    public class Services
    {
        public AppinviteService appinvite_service { get; set; }
    }
}
#endregion

namespace YKMoon.SDKTools.Editor
{
    public static class XMLExtend
    {
        public static string GetValue(this GoogleFirebaseXML.Resources xml, string name)
        {
            foreach(var str in xml.String) {
                if(str.Name.Equals(name)) {
                    return str.Text;
                }
            }
            return null;
        }
    }
}