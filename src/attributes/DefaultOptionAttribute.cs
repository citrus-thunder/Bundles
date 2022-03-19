using System;

namespace Bundles
{
	/// <summary>
	/// Declares this type a default BundleOption and defines its default value
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DefaultOptionAttribute : Attribute
	{
		/// <summary>
		/// Create a new DefaultOptionAttribute
		/// </summary>
		/// <param name="optionName"></param>
		/// <param name="defaultValue"></param>
		public DefaultOptionAttribute(string optionName, string defaultValue)
		{
			Name = optionName;
			DefaultValue = defaultValue;
		}

		/// <summary>
		/// Name of the option
		/// </summary>
		/// <value></value>
		public string Name { get; private set; }

		/// <summary>
		/// The option's default value
		/// </summary>
		/// <value></value>
		public string DefaultValue { get; private set; }
	}
}