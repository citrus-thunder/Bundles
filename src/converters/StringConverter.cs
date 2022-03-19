namespace Bundles.Converters
{
	/// <summary>
	/// "Converts" string values to and from their string representations
	/// </summary>
	/// <remarks>
	/// Though this doesn't actually do any conversion, it ensures BundleFields
	/// can handle plain strings correctly no matter how they're defined
	/// </remarks>
	public class StringConverter : BundleConverter<string>
	{
		/// <inheritdoc />
		public override string StringToValue(string input) => input;

		/// <inheritdoc />
		public override string ValueToString(string input) => input;
	}
}