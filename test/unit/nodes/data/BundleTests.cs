using System.Xml;
using System.IO;
using System.Collections.Generic;

using Xunit;

using Bundles.Nodes;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class BundleTests
	{
		private const string XML_ROOT = "unit/nodes/data/bundle/";

		[Theory]
		[InlineData("bundle_simple.xml")]
		[InlineData("bundle_multiple.xml")]
		public void TestBuildBundle(string file)
		{
			var xmlFile = XML_ROOT + file;
			var doc = TestConfig.GetXml(xmlFile);
			var bundler = TestConfig.GetBundler(xmlFile);
			var bundleNodes = doc.SelectNodes("/BundleDef/Data/Bundle");

			foreach (XmlNode node in bundleNodes)
			{
				bundler.Data.Nodes.ContainsKey(node.Attributes["name"].Value);
			}
		}



		[Theory]
		[InlineData("bundle_full.xml")]
		public void TestSaveLoadBundle(string file)
		{
			var xmlFile = XML_ROOT + file;

			var playerName = "John Doe";
			var playerExperience = "50";

			var items = new List<(string id, string name, string count)>
			{
				("1", "Potion", "10"),
				("2", "Copper Coin", "8"),
				("3", "Silver Coin", "3"),
				("4", "Gold Coin", "1")
			};

			// Save Bundler
			var sBundler = TestConfig.GetBundler(xmlFile);
			var sPlayerData = sBundler.Data["Save Data/File 1/Player Data"] as Bundle;

			// Set values directly through indexer
			sPlayerData["Name"].Value = playerName;
			sPlayerData["EXP"].Value = playerExperience;

			foreach (var item in items)
			{
				// Set values through BundleValues view
				var values = sPlayerData[$"Items/{item.id}"].Values;
				values["Name"] = item.name;
				values["Count"] = item.count;
			}

			sPlayerData.Save();

			// Load Bundler
			var lBundler = TestConfig.GetBundler(xmlFile);
			var lPlayerData = lBundler.Data["Save Data/File 1/Player Data"] as Bundle;

			Assert.True(lPlayerData.TryLoad());

			// Check Equality
			var pInfo = lPlayerData.Values;

			Assert.Equal(playerName, pInfo["Name"]);
			Assert.Equal(playerExperience, pInfo["EXP"]);

			foreach (var item in items)
			{
				var values = lPlayerData[$"Items/{item.id}"].Values;
				Assert.Equal(item.name, values["Name"]);
				Assert.Equal(item.count, values["Count"]);
			}
		}

		[Theory]
		[InlineData("bundle_full.xml")]
		public void TestSaveLoadFromStream(string file)
		{
			var xmlFile = XML_ROOT + file;

			var playerName = "John Doe";
			var playerExperience = "50";

			var items = new List<(string id, string name, string count)>
			{
				("1", "Potion", "10"),
				("2", "Copper Coin", "8"),
				("3", "Silver Coin", "3"),
				("4", "Gold Coin", "1")
			};

			// Save Bundler
			var sBundler = TestConfig.GetBundler(xmlFile);
			var sPlayerData = sBundler.Data["Save Data/File 1/Player Data"] as Bundle;

			// Set values directly through indexer
			sPlayerData["Name"].Value = playerName;
			sPlayerData["EXP"].Value = playerExperience;

			foreach (var item in items)
			{
				// Set values through BundleValues view
				var values = sPlayerData[$"Items/{item.id}"].Values;
				values["Name"] = item.name;
				values["Count"] = item.count;
			}

			// Load Bundler
			var lBundler = TestConfig.GetBundler(xmlFile);
			var lPlayerData = lBundler.Data["Save Data/File 1/Player Data"] as Bundle;

			using (var s = new MemoryStream())
			{
				sPlayerData.SaveToStream(s);
				s.Position = 0;
				lPlayerData.LoadFromStream(s);
			}

			// Check Equality
			var pInfo = lPlayerData.Values;

			Assert.Equal(playerName, pInfo["Name"]);
			Assert.Equal(playerExperience, pInfo["EXP"]);

			foreach (var item in items)
			{
				var values = lPlayerData[$"Items/{item.id}"].Values;
				Assert.Equal(item.name, values["Name"]);
				Assert.Equal(item.count, values["Count"]);
			}
		}
	}
}