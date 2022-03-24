using Bundles;
using Bundles.Nodes;

namespace Bundles.Test.Scenarios.RPG
{
	[BundleElement("PlayerData")]
	public class PlayerData : Bundle
	{
		[FieldName("Name")]
		public BundleField PlayerName { get; private set; }
		public BundleField<int> Currency { get; private set; }
		public BundleField<int> Experience { get; private set; }
		public BundleField<int> HP { get; private set; }

		public BundleList<QuestNode> Quests { get; private set; }

		[FieldName("Inventory")]
		public BundleList<ItemNode> Items { get; private set; }
	}
}