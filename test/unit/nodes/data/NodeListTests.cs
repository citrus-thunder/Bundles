using Xunit;

using Bundles.Nodes;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class NodeListTests
	{
		private const string XML_ROOT = "unit/nodes/data/nodelist/";

		[Theory]
		[InlineData("nodelist.xml")]
		public void TestAddRemoveClearChildren(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			var bundle = bundler.Data["Test Bundle"] as Bundle;

			bundle["List/0/Name"].Value = "Test Item 0";
			bundle["List/1/Name"].Value = "Test Item 1";
			bundle["List/2/Name"].Value = "Test Item 3";

			var list = bundle["List"] as NodeList;

			list.Remove("1");

			Assert.NotNull(bundle["List/0"]);
			Assert.NotNull(bundle["List/2"]);
			Assert.Equal(2, bundle["List"].Nodes.Count);

			list.Clear();

			Assert.Equal(0, bundle["List"].Nodes.Count);
		}
	}
}