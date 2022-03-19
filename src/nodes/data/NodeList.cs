using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents an abstract collection of nodes with homogenous contents
	/// </summary>
	[BundleElement("List")]
	public class NodeList : BundleNode
	{
		private string _xml;

		/// <inheritdoc />
		public override bool SkipWrite => Nodes.Count < 1;

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
			* This Node is an abstraction that represents multiple sibling nodes with 
			* identical definitions.
			*
			* Since we don't know how many of these child nodes exist beforehand nor do we know
			* their names, we will simply create them upon access if they do not exist already.
			*  
			* This behavior is likely to be optional/configurable in the near future.
			*/

			if (!Nodes.ContainsKey(nodeName))
			{
				var doc = new XmlDocument();
				doc.LoadXml(_xml);

				var listNode = doc.FirstChild;

				if (!Attributes.TryGetValue("of", out string itemName))
				{
					itemName = "ListItem";
				}

				var itemNode = doc.CreateNode("element", itemName, null) as XmlElement;
				itemNode.SetAttribute("name", nodeName);

				foreach (XmlNode child in listNode.ChildNodes)
				{
					itemNode.AppendChild(child.Clone());
				}

				AddNode(itemNode);
			}

			return base.GetChildNode(nodeName);
		}
	}
}