namespace Bundles.Converters
{
	/// <summary>
	/// Converts integers to and from their string representations
	/// </summary>
	public class IntConverter : BundleConverter<int>
	{
		/// <inheritdoc />
		public override string ValueToString(int input) => input.ToString();

		/// <inheritdoc />
		public override int StringToValue(string input)
		{
			int.TryParse(input, out int res);
			return res;
		}
	}
}