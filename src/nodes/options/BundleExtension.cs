using System;
using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Option defining the file extension used for <see cref="Bundle"/>s
	/// </summary>
	[BundleElement("BundleExtension")]
	[DefaultOption("BundleExtension", ".bundle")]
	public class BundleExtension : BundleOption
	{
		/// <inheritdoc />
		public override string Value
		{ 
			get => "." + base.Value.TrimStart('.'); 
			set => base.Value = "." + value.TrimStart('.'); 
		}
		
		/// <inheritdoc />
		protected override void BuildAttributes(XmlNode node)
		{
			base.BuildAttributes(node);

			var optionName = "BundleExtension";

			if (!Attributes.TryAdd("name", optionName))
			{
				Attributes["name"] = optionName;
			}
		}
	}
}