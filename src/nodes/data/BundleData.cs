using System;
using System.Xml;
using System.IO;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents a <see cref="Bundler"/>'s root data node
	/// </summary>
	[BundleElement("Data")]
	public class BundleData : BundleNode
	{
		/// <summary>
		/// Creates a new BundleData node associated with the given <see cref="Bundler"/>
		/// </summary>
		/// <param name="bundler"></param>
		public BundleData(Bundler bundler)
		{
			Bundler = bundler;
		}
		
		/// <inheritdoc />
		public override string NodePath => "/";

		/// <inheritdoc />
		protected override void BuildNode(XmlNode node)
		{
			if (Bundler.Options["GenerateEmptyFolders"].ToLower() == "true")
			{
				Directory.CreateDirectory(Bundler.DataRoot);
			}
		}
	}
}