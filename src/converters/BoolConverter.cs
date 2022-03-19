namespace Bundles.Converters
{
	/// <summary>
	/// Converts boolean values to and from their string representation.
	/// </summary>
	public class BoolConverter : BundleConverter<bool>
	{
		/// <inheritdoc />
		public override bool StringToValue(string input)
		{
			bool.TryParse(input, out bool res);
			return res;
		}

		/// <inheritdoc />
		public override string ValueToString(bool input) => input.ToString();
	}
}