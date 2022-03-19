using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents a single data value assocated with the parent node
	/// </summary>
	[BundleElement("Field")]
	public class FieldNode : BundleNode
	{
		/// <inheritdoc />
		public override bool SkipWrite => string.IsNullOrEmpty(Value);

		/// <inheritdoc />
		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			if (!string.IsNullOrEmpty(Value))
			{
				writer.WriteAttributeString("name", Name);
			}
		}

		/// <inheritdoc />
		protected override void WriteXmlContent(XmlWriter writer)
		{
			if (!string.IsNullOrEmpty(Value))
			{
				writer.WriteString(Value);
			}
		}
	}
}