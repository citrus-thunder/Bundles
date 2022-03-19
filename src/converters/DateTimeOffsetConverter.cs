using System;

namespace Bundles.Converters
{
	/// <summary>
	/// Converts DateTimeOffsets to and from their string representations
	/// </summary>
	public class DateTimeOffsetConverter : BundleConverter<DateTimeOffset>
	{
		/// <inheritdoc />
		public override DateTimeOffset StringToValue(string input)
		{
			DateTimeOffset.TryParse(input, out DateTimeOffset res);
			return res;
		}

		/// <inheritdoc />
		public override string ValueToString(DateTimeOffset input) => input.ToString();
	}
}