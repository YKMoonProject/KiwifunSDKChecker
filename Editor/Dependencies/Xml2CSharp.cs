/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

// auto generate from https://xmltocsharp.azurewebsites.net/
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Xml2CSharp
{
	[XmlRoot(ElementName = "androidPackage")]
	public class AndroidPackage
	{
		[XmlAttribute(AttributeName = "spec")]
		public string Spec { get; set; }

		[XmlIgnore]
		public string name {
			get {
				var list = Spec.Split(':');
				return Spec.Replace(":"+list[list.Length - 1], "");
			}
		}
		[XmlIgnore]
		public string version {
			get {
				var list = Spec.Split(':');
				return list[list.Length - 1];
			}
		}
	}

	[XmlRoot(ElementName = "androidPackages")]
	public class AndroidPackages
	{
		[XmlElement(ElementName = "androidPackage")]
		public List<AndroidPackage> AndroidPackage { get; set; }
	}

	[XmlRoot(ElementName = "iosPod")]
	public class IosPod
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
	}

	[XmlRoot(ElementName = "iosPods")]
	public class IosPods
	{
		[XmlElement(ElementName = "iosPod")]
		public List<IosPod> IosPod { get; set; }
	}

	[XmlRoot(ElementName = "dependencies")]
	public class Dependencies
	{
		[XmlElement(ElementName = "androidPackages")]
		public AndroidPackages AndroidPackages { get; set; }
		[XmlElement(ElementName = "iosPods")]
		public IosPods IosPods { get; set; }
	}

}
