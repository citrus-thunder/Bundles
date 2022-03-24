using System;
using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents an abstract collection of nodes with homogenous contents
	/// </summary>
	[BundleElement("List")]
	public class NodeList : BundleNode
	{
		/// <summary>
		///  XML template used to dynamically generate child elements
		/// </summary>
		/// <value></value>
		public string XmlTemplate { get; private set; }

		/// <summary>
		/// Type template used to dynamically generate child elements
		/// </summary>
		/// <value></value>
		public Type TypeTemplate { get; internal set; }

		/// <inheritdoc />
		public override bool SkipWrite => Nodes.Count < 1;

		/// <inheritdoc />
		protected override void BuildNode(XmlNode node)
		{
			XmlTemplate = node.OuterXml;
		}

		/// <inheritdoc />
		protected override void BuildChildren(XmlNode node)
		{
			// This method intentionally left blank.
			// (We don't want this Node automatically building its children!)
		}

		/// <inheritdoc />
		protected sealed override BundleNode GetChildNode(string nodeName)
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
				if (TypeTemplate == null)
				{
					var doc = new XmlDocument();
					doc.LoadXml(XmlTemplate);

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
				else
				{
					var itemNode = Activator.CreateInstance(TypeTemplate) as BundleNode;
					itemNode.Name = nodeName;
					itemNode.GenerateFields();
					AddNode(itemNode);
				}
			}

			return base.GetChildNode(nodeName);
		}
	}
}