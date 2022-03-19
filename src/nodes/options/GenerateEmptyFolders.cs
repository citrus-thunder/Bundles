using System;
using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Option defining whether <see cref="Folder"/>s automatically create
	/// their filesystem entities when instantiated
	/// </summary>
	[BundleElement("GenerateEmptyFolders")]
	[DefaultOption("GenerateEmptyFolders", "false")]
	public class GenerateEmptyFolders : BundleOption
	{
		/// <inheritdoc />
		protected override void BuildAttributes(XmlNode node)
		{
			base.BuildAttributes(node);

			var optionName = "GenerateEmptyFolders";

			if (!Attributes.TryAdd("name", optionName))
			{
				Attributes["name"] = optionName;
			}
		}
	}
}