using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Bundles.Nodes;

namespace Bundles
{
	/// <summary>
	/// Represents a node within the filesystem layout described by a <see cref="Bundler"/>'s
	/// definition file.
	/// </summary>
	public class BundleNode : IXmlSerializable
	{
		/// <summary>
		/// Contains BundleValue types and XML element names with which they are associated.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, Type> NodeTypes = new Dictionary<string, Type>();

		static BundleNode()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
			var type = typeof(BundleNode);

			foreach (var assembly in assemblies)
			{
				var typeList = assembly
					.GetTypes()
					.Where(t => type.IsAssignableFrom(t) && t.IsDefined(typeof(BundleElementAttribute), false)
					).ToList();

				foreach (var iType in typeList)
				{
					var e = iType.GetCustomAttribute<BundleElementAttribute>();
					NodeTypes.TryAdd(e.ElementName, iType);
				}
			}
		}

		/// <summary>
		/// Retrieves a descendant of this node via relative NodePath.
		/// </summary>
		/// <returns></returns>
		public BundleNode this[string bundleName] => Get(bundleName);

		/// <summary>
		/// This BundleNode type's XML element name.
		/// </summary>
		/// <returns></returns>
		public string ElementName => GetType().GetCustomAttribute<BundleElementAttribute>().ElementName;

		/// <summary>
		/// This node's Name as it appears in the NodePath.
		/// </summary>
		/// <value></value>
		public string Name
		{
			get
			{
				Attributes.TryGetValue("name", out string res);
				return res;
			}
			set
			{
				if (!Attributes.TryAdd("name", value))
				{
					Attributes["name"] = value;
				}
			}
		}

		/// <summary>
		/// This node's stored Value.
		/// </summary>
		/// <value></value>
		public virtual string Value
		{
			get
			{
				Attributes.TryGetValue("value", out string res);
				return res;
			}
			set
			{
				if (!Attributes.TryAdd("value", value))
				{
					Attributes["value"] = value;
				}
			}
		}

		/// <summary>
		/// <see cref="BundleValues"/> view of this node's child <see cref="FieldNode"/>s.
		/// </summary>
		/// <value></value>
		public BundleValues Values
		{
			get
			{
				if (m_Values == null)
				{
					m_Values = new BundleValues(this);
				}
				return m_Values;
			}
		}
		private BundleValues m_Values;

		/// <summary>
		/// Describes this node's position in the <see cref="Bundler"/>'s node tree.
		/// </summary>
		/// <returns></returns>
		public virtual string NodePath => $"{Parent?.NodePath?.TrimEnd('/')}/{Attributes["name"]}";

		/// <summary>
		/// If true, this node will skip writing its contents to XML upon serialization.
		/// </summary>
		/// <value></value>
		public virtual bool SkipWrite { get; } = false;

		/// <summary>
		/// This node's attributes as imported from XML, defined in the Bundler's definition file,
		/// or explicitly set after the node's initialization.
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// This node's descendant nodes.
		/// </summary>
		/// <returns></returns>
		public Dictionary<string, BundleNode> Nodes { get; set; } = new Dictionary<string, BundleNode>();

		/// <summary>
		/// This node's parent node.
		/// </summary>
		/// <value></value>
		public BundleNode Parent { get; protected set; }

		/// <summary>
		/// The <see cref="Bundler"/> which owns the node tree that contains this node.
		/// </summary>
		/// <value></value>
		public Bundler Bundler { get; internal set; }

		XmlSchema IXmlSerializable.GetSchema() => null;

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			var doc = new XmlDocument();
			doc.Load(reader);
			ReadXml(doc.FirstChild);
		}

		void IXmlSerializable.WriteXml(XmlWriter writer) => WriteXml(writer);

		private void ReadXml(XmlNode node)
		{
			ReadXmlAttributes(node);
			ReadXmlContent(node);
		}

		/// <summary>
		/// Import attributes deserialized from XML
		/// </summary>
		/// <param name="node"></param>
		protected virtual void ReadXmlAttributes(XmlNode node)
		{
			var e = node as XmlElement;
			foreach (XmlAttribute att in e.Attributes)
			{
				Attributes.TryAdd(att.Name, att.Value);
			}
		}

		/// <summary>
		/// Import content deserialized from XML.
		/// </summary>
		/// <param name="node"></param>
		protected virtual void ReadXmlContent(XmlNode node)
		{
			var element = node as XmlElement;

			foreach (XmlNode child in node.ChildNodes)
			{
				if (child.NodeType == XmlNodeType.Text)
				{
					if (!Attributes.TryAdd("value", child.InnerText))
					{
						Attributes["value"] = child.InnerText;
					}
				}

				if (child is XmlElement childElement)
				{
					var s = new XmlSerializer(Bundle.NodeTypes[childElement.Name], new XmlRootAttribute(childElement.Name));
					var nodeName = childElement.Attributes["name"].Value;
					if (Nodes.ContainsKey(childElement.Attributes["name"].Value))
					{
						using (XmlNodeReader reader = new XmlNodeReader(childElement))
						{
							var newNode = s.Deserialize(reader) as BundleNode;
							Nodes[nodeName].Merge(newNode);
						}
					}
					else
					{
						using (XmlNodeReader reader = new XmlNodeReader(childElement))
						{
							var newNode = AddNode(childElement);
							var clone = s.Deserialize(reader);
							newNode.Merge(clone as BundleNode);
						}
					}
				}
			}
		}

		private void WriteXml(XmlWriter writer)
		{
			WriteXmlAttributes(writer);
			WriteXmlContent(writer);
		}

		/// <summary>
		/// Write this node's attributes to XML.
		/// </summary>
		/// <param name="writer"></param>
		protected virtual void WriteXmlAttributes(XmlWriter writer)
		{
			foreach (var att in Attributes)
			{
				writer.WriteAttributeString(att.Key, att.Value);
			}
		}

		/// <summary>
		/// Write this node's contents to XML.
		/// </summary>
		/// <param name="writer"></param>
		protected virtual void WriteXmlContent(XmlWriter writer)
		{
			foreach (var node in Nodes.Values)
			{
				if (node.SkipWrite)
				{
					continue;
				}

				var t = node.GetType();
				var elementAtt = t.GetCustomAttribute<BundleElementAttribute>().ElementName;
				var s = new XmlSerializer(t, new XmlRootAttribute(elementAtt));
				s.Serialize(writer, node);
			}
		}

		/// <summary>
		/// Called when this node is attached to a parent node.
		/// </summary>
		/// <param name="parentNode"></param>
		protected virtual void Attach(BundleNode parentNode)
		{
			// To be overridden in derived classes.
		}

		/// <summary>
		/// Called when a child node is added to this node.
		/// </summary>
		/// <param name="childNode"></param>
		protected virtual void AddChild(BundleNode childNode)
		{
			// To be overridden in derived classes.
		}

		/// <summary>
		/// Adds a node as a child to this node, as described in a given
		/// XmlNode
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public BundleNode AddNode(XmlNode node)
		{
			if (NodeTypes.ContainsKey(node.Name))
			{
				var newNode = Activator.CreateInstance(NodeTypes[node.Name]) as BundleNode;
				newNode.Bundler = Bundler;
				newNode.Parent = this;
				newNode.BuildAttributes(node);
				newNode.GenerateFields();
				newNode.BuildNode(node);
				if (!Nodes.TryAdd(newNode.Name, newNode))
				{
					Nodes[newNode.Name] = newNode;
				}
				newNode.Attach(this);
				AddChild(newNode);
				newNode.BuildChildren(node);

				return newNode;
			}
			else
			{
				throw new InvalidOperationException($"Unable to create BundleNode. Unrecognized node type: {node.Name}");
			}
		}

		/// <summary>
		/// Adds the given node as a child to this node.
		/// </summary>
		/// <param name="newNode"></param>
		public void AddNode(BundleNode newNode)
		{
			if (!Nodes.TryAdd(newNode.Name, newNode))
			{
				Nodes[newNode.Name] = newNode;
			}

			newNode.Bundler = Bundler;
			newNode.Parent = this;
			newNode.Attach(this);
			AddChild(newNode);
		}

		/// <summary>
		/// Merges the given node's attributes and content into this node.
		/// </summary>
		/// <param name="node"></param>
		protected void Merge(BundleNode node)
		{
			foreach (var att in node.Attributes)
			{
				Attributes.TryAdd(att.Key, att.Value);
			}
			foreach (var n in node.Nodes.Values)
			{
				if (!node.Nodes.ContainsKey(n.Name))
				{
					AddNode(n);
				}
				else
				{
					Nodes[n.Name] = n;
					n.Parent = this;
					n.Bundler = Bundler;
				}
			}
		}

		/// <summary>
		/// Builds node's attributes and content from a given XmlNode
		/// </summary>
		/// <param name="node"></param>
		public void Build(XmlNode node)
		{
			BuildAttributes(node);
			GenerateFields();
			BuildNode(node);
			BuildChildren(node);
		}

		private void GenerateFields()
		{
			foreach (var prop in GetType().GetProperties())
			{
				var T = prop.PropertyType;
				if (typeof(IBundleField).IsAssignableFrom(T))
				{
					var att = prop.GetCustomAttribute<FieldNameAttribute>();
					var fieldName = prop.Name;

					if (att != null)
					{
						fieldName = att.Name;
					}

					AddField(fieldName);

					prop.SetValue(this, Activator.CreateInstance(T, this, fieldName));
				}
			}
		}

		/// <summary>
		/// Builds the node's attributes as described in the given XmlNode
		/// </summary>
		/// <param name="node"></param>
		protected virtual void BuildAttributes(XmlNode node)
		{
			foreach (XmlAttribute attribute in node.Attributes)
			{
				if (!Attributes.TryAdd(attribute.Name.ToLower(), attribute.Value))
				{
					Attributes[attribute.Name.ToLower()] = attribute.Value;
				}
			}
		}

		/// <summary>
		/// Called when this node is being built
		/// </summary>
		/// <param name="node"></param>
		/// <remarks>
		/// Called after this node's properties and attributes are defined, but
		/// before its children are built.
		/// </remarks>
		protected virtual void BuildNode(XmlNode node)
		{
		// To be overridden in derived classes.
		}

		/// <summary>
		/// Builds this node's child nodes as described in the given XmlNode.
		/// </summary>
		/// <param name="node"></param>
		protected virtual void BuildChildren(XmlNode node)
		{
			foreach (XmlNode childNode in node.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					AddNode(childNode);
				}
			}
		}

		/// <summary>
		/// Adds a <see cref="FieldNode"/> as a child to this node.
		/// </summary>
		/// <param name="name">The new field's name</param>
		/// <param name="value">The new field's value</param>
		/// <returns></returns>
		public FieldNode AddField(string name, string value = null)
		{
			FieldNode res = null;

			if (!Nodes.ContainsKey(name))
			{
				var node = new FieldNode()
				{
					Name = name,
					Value = value
				};

				Nodes.Add(name, node);
				node.Attach(this);
				AddChild(node);
				res = node;
			}

			return res;
		}

		/// <summary>
		/// Retrieves one of this node's child nodes.
		/// </summary>
		/// <param name="nodeName">Name of the child node</param>
		/// <returns></returns>
		protected virtual BundleNode GetChildNode(string nodeName)
		{
			Nodes.TryGetValue(nodeName, out BundleNode res);
			return res;
		}

		/// <summary>
		/// Retrieves one of this node's descendants using NodePath syntax.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public BundleNode Get(string path) => Get<BundleNode>(path);

		/// <summary>
		/// Retrieves one of this node's descendants using NodePath syntax.
		/// </summary>
		/// <param name="path"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Get<T>(string path) where T : BundleNode, new()
		{
			var pattern = @"(?<node>[\w ]+|[^/])\/?(?<subpath>.*)";
			var match = Regex.Match(path, pattern);
			if (match.Success)
			{
				var node = match.Groups["node"].Value;
				var subPath = match.Groups["subpath"].Value;

				if (subPath == string.Empty)
				{
					if (GetChildNode(node) is T tRes)
					{
						return tRes;
					}
					else
					{
						return default(T);
					}
				}
				else
				{
					return GetChildNode(node).Get<T>(subPath);
				}
			}
			else
			{
				return default(T);
			}
		}
	}
}