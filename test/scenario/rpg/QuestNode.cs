using Bundles;
using Bundles.Nodes;

namespace Bundles.Test.Scenarios.RPG
{
	[BundleElement("Quest")]
	public class QuestNode : BundleNode
	{
		[FieldName("Name")]
		public BundleField QuestName { get; private set; }

		public BundleField Description { get; private set; }
	}
}