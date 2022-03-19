using System;
using System.IO;
using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents a folder in the filesystem
	/// </summary>
	[BundleElement("Folder")]
	public class Folder : BundleNode
	{
		/// <inheritdoc />
		protected override void BuildNode(XmlNode node)
		{
			if (Bundler.Options["GenerateEmptyFolders"].ToLower() == "true")
			{
				Directory.CreateDirectory(Path.Join(Bundler.DataRoot, NodePath.ToPath()));
			}
		}
	}
}