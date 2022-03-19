using Xunit;

using Bundles.Nodes;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class FieldTests
	{
		private const string XML_ROOT = "unit/nodes/data/field/";

		[Theory]
		[InlineData("field.xml")]
		public void TestSetFieldValue(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			var bundle = bundler.Data["Test Bundle"] as Bundle;
			string testValue;

			// Set through indexer
			testValue = "Test Data Indexer";
			bundle["Test Field"].Value = testValue;
			Assert.Equal(testValue, bundler.Data["Test Bundle/Test Field"].Value);

			// Set through BundleValue view
			testValue = "Test Bundle Indexer";
			var values = bundle.Values;
			values["Test Field"] = testValue;
			Assert.Equal(testValue, bundler.Data["Test Bundle/Test Field"].Value);

			// Test BundleValue view All property
			Assert.Equal(testValue, bundle.Values.All["Test Field"]);
		}
	}
}