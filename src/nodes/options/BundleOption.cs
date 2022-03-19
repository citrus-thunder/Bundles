using System;
using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents an optional parameter used by a <see cref="Bundler"/>
	/// </summary>
	[BundleElement("Option")]
	public class BundleOption : BundleNode
	{
		/// <inheritdoc />
		protected override void BuildNode(XmlNode node)
		{
			var name = "";
			var value = "";

			if (Attributes.TryGetValue("name", out name))
			{
				if (!Attributes.TryGetValue("value", out value))
				{
					value = node.InnerText;
				}

				if (value == String.Empty)
				{
					throw new XmlException("Option node must have either a 'value' attribute or inner text");
				}

				Value = value;
			}
			else
			{
				throw new XmlException("Option node must have a 'name' attribute");
			}
		}
	}
}