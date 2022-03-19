using System;

namespace Bundles
{
	/// <summary>
	/// Describes the XML Element Name that will be associated with this type
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class BundleElementAttribute : Attribute
	{
		/// <summary>
		/// Create a new BundleElementAttribute, associating this type
		/// with the given XML Element Name
		/// </summary>
		/// <param name="elementName"></param>
		public BundleElementAttribute(string elementName)
		{
			ElementName = elementName;
		}

		/// <summary>
		/// XML Element Name that this type is associated with
		/// </summary>
		/// <value></value>
		public string ElementName { get; private set; }
	}
}