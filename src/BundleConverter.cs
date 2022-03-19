using System;

namespace Bundles
{
	/// <summary>
	/// Describes a converter class that can convert
	/// a value type to and from a string.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class BundleConverter<T>
	{
		/// <summary>
		/// Converts a generic value to a string
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public abstract string ValueToString(T input);

		/// <summary>
		/// Converts a string to a generic value
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public abstract T StringToValue(string input);
	}
}