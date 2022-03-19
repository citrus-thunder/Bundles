using System.Xml;
using System.Xml.Serialization;

using Bundles;
using Bundles.Nodes;

namespace Bundles.Test
{
	[BundleElement("Card")]
	public class BundleCard : BundleNode
	{
		public BundleField Suit { get; private set; }

		[FieldName("Value")]
		public BundleField<int> CardValue { get; private set; }

		public CustomField Color { get; private set; }
	}
}