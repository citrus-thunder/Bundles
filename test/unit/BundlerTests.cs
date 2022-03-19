using System;
using System.Xml;
using System.IO;

using Xunit;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class BundlerTests
	{
		private const string XML_ROOT = "unit/bundler/";

		[Theory]
		[InlineData("bundler.xml")]
		[InlineData("bundler_noconfig.xml")]
		public void GetFromTestConfig(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);
			ValidateBundler(bundler, xmlFile);
		}

		[Theory]
		[InlineData("bundler.xml")]
		[InlineData("bundler_noconfig.xml")]
		public void ConstructFromPath(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = new Bundler(TestConfig.GetXmlPath(xmlFile));
			ValidateBundler(bundler, xmlFile);
		}

		[Theory]
		[InlineData("bundler.xml")]
		[InlineData("bundler_noconfig.xml")]
		public void ConstructFromXmlDocument(string file)
		{
			var xmlFile = XML_ROOT + file;
			var doc = TestConfig.GetXml(xmlFile);
			var bundler = new Bundler(doc);
			ValidateBundler(bundler, xmlFile);
		}

		private void ValidateBundler(Bundler bundler, string xmlPath)
		{
			Assert.NotNull(bundler);
			Assert.NotNull(bundler.Data);
			Assert.NotNull(bundler.Config);
		}
	}
}