using System;
using System.IO;
using System.Reflection;
using System.Xml;

using Bundles;

namespace Bundles.Test
{
	public static class TestConfig
	{
		public const bool FAIL_NYI = true;

		public static Bundler GetBundler(string path)
		{
			return new Bundler(GetXmlPath(path));
		}

		public static XmlDocument GetXml(string path)
		{
			var doc = new XmlDocument();
			doc.Load(GetXmlPath(path));
			return doc;
		}

		public static string GetXmlPath(string path) => Path.Join(AppDomain.CurrentDomain.BaseDirectory, "bundle", path.ToPath());
	}
}