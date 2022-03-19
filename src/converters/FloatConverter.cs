namespace Bundles.Converters
{
	/// <summary>
	/// Converts floats to and from their string representations
	/// </summary>
	public class FloatConverter : BundleConverter<float>
	{
		/// <inheritdoc />
		public override float StringToValue(string input)
		{
			float.TryParse(input, out float res);
			return res;
		}

		/// <inheritdoc />
		public override string ValueToString(float input) => input.ToString();
	}
}