using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents an abstract collection of folders with homogenous contents
	/// </summary>
	[BundleElement("FolderList")]
	public class FolderList : BundleNode
	{
		private string _xml;
		
		/// <inheritdoc />
		protected override void BuildNode(XmlNode node)
		{
			_xml = node.OuterXml;
		}

		/// <inheritdoc />
		protected override void BuildChildren(XmlNode node)
		{
			// This method intentionally left blank.
			// (We don't want this Node automatically building its children!)
		}

		/// <inheritdoc />
		protected override BundleNode GetChildNode(string nodeName)
		{
			/*
			* This Node is an abstraction that represents multiple sibling folders with 
			* identical contents (think user profiles, for example).
			*
			* Since we don't know how many of these folders exist beforehand nor do we know
			* their names, we will simply create them upon access if they do not exist already.
			*  
			* This behavior is likely to be optional/configurable in the near future.
			*/

			if (!Nodes.ContainsKey(nodeName))
			{
				var doc = new XmlDocument();
				doc.LoadXml(_xml);

				var foldersNode = doc.FirstChild;

				var folderNode = doc.CreateNode("element", "Folder", null) as XmlElement;
				folderNode.SetAttribute("name", nodeName);

				foreach (XmlNode child in foldersNode.ChildNodes)
				{
					folderNode.AppendChild(child.Clone());
				}

				AddNode(folderNode);
			}

			return base.GetChildNode(nodeName);
		}
	}
}