using System;

namespace Bundles
{
	/// <summary>
	/// Overrides the BundleField's name
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class FieldNameAttribute : Attribute
	{
		/// <summary>
		/// Create a new FieldNameAttribute
		/// </summary>
		/// <param name="name"></param>
		public FieldNameAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// This BundleField's overridden name
		/// </summary>
		/// <value></value>
		public string Name { get; }
	}
}