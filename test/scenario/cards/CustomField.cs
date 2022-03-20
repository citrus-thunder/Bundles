using Bundles;

namespace Bundles.Test
{
	public enum CardColor {Red, Black}

	public class CustomField : BundleField<CardColor>
	{
		public CustomField(BundleNode parent, string fieldName) : base(parent, fieldName)
		{

		}

		public string TestProperty => "True";
	}

	public class CardColorConverter : BundleConverter<CardColor>
	{
		public override CardColor StringToValue(string input)
		{
			CardColor res;
			switch (input)
			{
				default:
				case "Red":
					res = CardColor.Red;
					break;
				case "Black":
					res = CardColor.Black;
					break;
			}
			return res;
		}

		public override string ValueToString(CardColor input)
		{
			string res = "";
			switch (input)
			{
				case CardColor.Red:
					res = "Red";
					break;
				case CardColor.Black:
					res = "Black";
					break;
			}
			return res;
		}
	}
}