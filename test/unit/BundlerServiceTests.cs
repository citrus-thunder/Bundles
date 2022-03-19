using Xunit;

using Bundles.Nodes;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class BundlerServiceTests
	{
		private const string XML_ROOT = "unit/services/";

		[Theory]
		[InlineData("services.xml")]
		public void TestAddGetRemoveServices(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			var testName = "Service Name";
			var testID = 2;
			
			bundler.Services.AddService<TestServiceA>();
			var serviceA = bundler.Services.GetService<TestServiceA>();
			serviceA.Name = testName;

			var serviceA2 = bundler.Services.GetService<TestServiceA>();
			Assert.Equal(testName, serviceA2.Name);

			var b = new TestServiceB{ID = testID};
			bundler.Services.AddService(b);
			var serviceB = bundler.Services.GetService(b.GetType()) as TestServiceB;
			Assert.Equal(testID, serviceB.ID);

			bundler.Services.RemoveService<TestServiceA>();

			Assert.Null(bundler.Services.GetService<TestServiceA>());
		}

		private class TestServiceA
		{
			public string Name { get; set; }
		}

		private class TestServiceB
		{
			public int ID { get; set; }
		}
	}
}