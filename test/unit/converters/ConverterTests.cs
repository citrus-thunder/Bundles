using System;
using Xunit;

using Bundles.Nodes;

namespace Bundles.Test
{
	public class ConverterTests
	{
		private const string XML_ROOT = "unit/converters/";

		[Theory]
		[InlineData("converters.xml")]
		public void TestDefaultConverters(string file)
		{
			var rand = new Random();
			var xmlFile = XML_ROOT + file;

			var sBundler = TestConfig.GetBundler(xmlFile);
			var sBundle = sBundler.Data["ConverterBundle"] as Bundle;
			var sConverterNode = sBundle["Converter"] as ConverterNode;

			var defaultString = Guid.NewGuid().ToString();
			var explicitString = Guid.NewGuid().ToString();
			var randInt = rand.Next();
			var randFloat = (float)rand.NextDouble();
			var randDouble = rand.NextDouble();
			var randBool = randInt % 2 == 0;
			var time = new DateTimeOffset(
				new DateTime(
					rand.Next(2000, 2101),
					rand.Next(1, 13),
					rand.Next(1, 29),
					rand.Next(0, 24),
					rand.Next(0, 60),
					rand.Next(0, 60)
					)
				);

			sConverterNode.DefaultString.Value = defaultString;
			sConverterNode.ExplicitString.Value = explicitString;
			sConverterNode.Int.Value = randInt;
			sConverterNode.Float.Value = randFloat;
			sConverterNode.Double.Value = randDouble;
			sConverterNode.Bool.Value = randBool;
			sConverterNode.Time.Value = time;

			sBundle.Save();

			var lBundler = TestConfig.GetBundler(xmlFile);
			var lBundle = lBundler.Data["ConverterBundle"] as Bundle;
			Assert.True(lBundle.TryLoad());
			var lConverterNode = lBundle["Converter"] as ConverterNode;

			Assert.Equal(defaultString, lConverterNode.DefaultString.Value);
			Assert.Equal(explicitString, lConverterNode.ExplicitString.Value);
			Assert.Equal(randInt, lConverterNode.Int.Value);
			Assert.Equal(randFloat, lConverterNode.Float.Value);
			Assert.Equal(randDouble, lConverterNode.Double.Value);
			Assert.Equal(randBool, lConverterNode.Bool.Value);
			Assert.Equal(time, lConverterNode.Time.Value);
		}
	}

	[BundleElement("Converter")]
	public class ConverterNode : BundleNode
	{
		public BundleField DefaultString { get; private set; }

		public BundleField<string> ExplicitString { get; private set; }

		public BundleField<int> Int { get; private set; }

		public BundleField<DateTimeOffset> Time { get; private set; }

		public BundleField<bool> Bool { get; private set; }

		public BundleField<float> Float { get; private set; }

		public BundleField<double> Double { get; private set; }
	}
}