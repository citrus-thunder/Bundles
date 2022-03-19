using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

using Xunit;

using Bundles;
using Bundles.Nodes;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class OptionsTests
	{
		private const string XML_ROOT = "unit/nodes/options/";

		[Theory]
		[InlineData("options_default.xml")]
		public void TestDefaultOptions(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			Assert.NotNull(bundler);
			Assert.NotNull(bundler.Config);

			foreach (var option in GetDefaultOptionInfo())
			{
				// Test direct access through indexer
				Assert.Equal(option.Value, bundler.Config[option.Key].Value);

				// Test access through Options view
				Assert.Equal(option.Value, bundler.Options[option.Key]);
			}
		}

		[Theory]
		[InlineData("options_custom.xml")]
		public void TestLoadCustomOptions(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			Assert.NotNull(bundler);
			Assert.NotNull(bundler.Config);

			var doc = TestConfig.GetXml(xmlFile);
			var optionNodes = doc.FirstChild.SelectNodes("./Config/Option");

			foreach (XmlNode optionNode in optionNodes)
			{
				Assert.Equal(optionNode.InnerText, bundler.Options[optionNode.Attributes["name"].Value]);
			}
		}

		[Theory]
		[InlineData("options_malformed_1.xml")]
		[InlineData("options_malformed_2.xml")]
		public void TestCatchMalformedCustomOptions(string file)
		{
			var xmlFile = XML_ROOT + file;

			var caughtException = false;
			try
			{
				var bundler = TestConfig.GetBundler(xmlFile);
			}
			catch (XmlException)
			{
				caughtException = true;
			}

			Assert.True(caughtException);
		}

		[Theory]
		[InlineData("options_named_defaults.xml")]
		public void TestPreventDefaultOptionNameOverride(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			Assert.NotNull(bundler);
			Assert.NotNull(bundler.Config);

			foreach (var option in GetDefaultOptionInfo())
			{
				Assert.True(bundler.Config.Nodes.ContainsKey(option.Key));
			}
		}

		private Dictionary<string, string> GetDefaultOptionInfo()
		{
			var res = new Dictionary<string, string>();

			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
			var type = typeof(BundleOption);

			foreach (var assembly in assemblies)
			{
				var typeList = assembly
					.GetTypes()
					.Where(t => type.IsAssignableFrom(t) && t.IsDefined(typeof(DefaultOptionAttribute), false)
					).ToList();

				foreach (var iType in typeList)
				{
					var defaultOption = iType.GetCustomAttribute<DefaultOptionAttribute>();

					var option = Activator.CreateInstance(iType) as BundleOption;
					option.Name = defaultOption.Name;
					option.Value = defaultOption.DefaultValue;

					res.Add(option.Name, option.Value);
				}
			}

			return res;
		}
	}
}