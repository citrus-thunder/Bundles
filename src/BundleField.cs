namespace Bundles
{
	/// <summary>
	/// Represents an automatically-generated <see cref="Nodes.FieldNode"/>
	/// </summary>
	public class BundleField
	{
		/// <summary>
		/// Create a new BundleField
		/// </summary>
		/// <param name="node"></param>
		/// <param name="name"></param>
		public BundleField(BundleNode node, string name)
		{
			Name = name;
			Node = node;
		}

		/// <summary>
		/// This field's parent Node
		/// </summary>
		/// <value></value>
		public BundleNode Node { get; private set; }

		/// <summary>
		/// This field's Name, as it appears in the parent node's NodePath
		/// </summary>
		/// <value></value>
		public string Name { get; private set; }

		/// <summary>
		/// This field's value
		/// </summary>
		/// <value></value>
		public string Value
		{
			get => Node[Name].Value;
			set => Node[Name].Value = value;
		}
	}
}