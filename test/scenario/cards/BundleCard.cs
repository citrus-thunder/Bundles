using System.Xml;
using System.Xml.Serialization;

using Bundles;
using Bundles.Nodes;

namespace Bundles.Test.Scenarios.Cards
{
	public enum CardSuit {Clubs, Diamonds, Hearts, Spades}

	[BundleElement("Card")]
	public class BundleCard : BundleNode
	{
		public BundleField<CardSuit> Suit { get; private set; }

		[FieldName("Value")]
		public BundleField<int> CardValue { get; private set; }

		public CustomField Color { get; private set; }
	}

	public class SuitConverter : BundleConverter<CardSuit>
	{
		public override CardSuit StringToValue(string input)
		{
			CardSuit res;
			switch (input)
			{
				default:
				case "Clubs":
					res = CardSuit.Clubs;
					break;
				case "Diamonds":
					res = CardSuit.Diamonds;
					break;
				case "Hearts":
					res = CardSuit.Hearts;
					break;
				case "Spades":
					res = CardSuit.Spades;
					break;
			}
			return res;
		}

		public override string ValueToString(CardSuit input)
		{
			string res = "";
			switch (input)
			{
				case CardSuit.Clubs:
					res = "Clubs";
					break;
				case CardSuit.Diamonds:
					res = "Diamonds";
					break;
				case CardSuit.Hearts:
					res = "Hearts";
					break;
				case CardSuit.Spades:
					res = "Spades";
					break;
			}
			return res;
		}
	}
}