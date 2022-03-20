using Xunit;

using Bundles.Nodes;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class CardScenarioTests
	{
		private const string XML_ROOT = "scenario/cards/";

		[Theory]
		[InlineData("cards.xml")]
		public void RunScenario(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			var player = bundler.Data["Save/0/PlayerData"] as Bundle;
			var cards = bundler.Data["Save/0/DeckData"] as Bundle;

			var playerInfo = player.Values;
			var card = cards["Cards/0"] as BundleCard;

			card.Suit.Value = CardSuit.Spades;
			card.CardValue.Value = 6;
			card.Color.Value = CardColor.Black;
		}
	}
}