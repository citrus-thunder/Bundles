using Bundles;
using Bundles.Nodes;

namespace Bundles.Test.Scenarios.RPG
{
	[BundleElement("Item")]
	public class ItemNode : BundleNode
	{
		[FieldName("Name")]
		public BundleField ItemName { get; private set; }

		public BundleField Description { get; private set; }

		public BundleField<int> Count { get; private set; }
	}
}