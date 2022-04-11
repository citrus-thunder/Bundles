using Bundles.Nodes;

namespace Bundles
{
	/// <summary>
	/// View encapsulating a NodeList containing a homogenous list of
	/// BundleNodes
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BundleList<T>: IBundleList where T : BundleNode
	{
		/// <summary>
		/// Create a new BundleList
		/// </summary>
		/// <param name="node"></param>
		/// <param name="listName"></param>
		public BundleList(BundleNode node, string listName)
		{
			Name = listName;
			Node = node;
		}

		/// <summary>
		/// Access a member of the encapsulated list by its name
		/// </summary>
		public T this[string index] => List[index] as T;
		
		/// <summary>
		/// A reference to the encapsulated NodeList
		/// </summary>
		public NodeList List => Node[Name] as NodeList;

		/// <summary>
		/// The name of the NodeList to reference
		/// </summary>
		/// <value></value>
		public string Name { get; private set; }

		/// <summary>
		/// Reference to the BundleNode which owns the NodeList exposed by this view.
		/// </summary>
		/// <value></value>
		public BundleNode Node { get; private set; }

		/// <summary>
		/// Clears all items from this BundleList
		/// </summary>
		public void Clear() => List.Clear();

		/// <summary>
		/// Removes the item by the given name from this BundleList
		/// </summary>
		/// <param name="nodeName"></param>
		public void Remove(string nodeName) => List.Remove(nodeName);
	}
}