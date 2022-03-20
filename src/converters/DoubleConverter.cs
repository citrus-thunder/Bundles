namespace Bundles.Converters
{
	/// <summary>
	/// Converts doubles to and from their string representations
	/// </summary>
	public class DoubleConverter : BundleConverter<double>
	{
		/// <inheritdoc />
		public override double StringToValue(string input)
		{
			double.TryParse(input, out double res);
			return res;
		}

		/// <inheritdoc />
		public override string ValueToString(double input) => input.ToString();
	}
}