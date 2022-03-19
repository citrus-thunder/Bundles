using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bundles.Nodes;
namespace Bundles
{
	/// <summary>
	/// Represents a two-way view of a <see cref="BundleNode"/>'s
	/// child <see cref="FieldNode"/>s' values.
	/// </summary>
	public class BundleValues
	{
		/// <summary>
		/// Creates a new BundleValues view
		/// </summary>
		/// <param name="node"></param>
		public BundleValues(BundleNode node)
		{
			Node = node;
		}

		/// <summary>
		/// The <see cref="BundleNode"/> this view is for.
		/// </summary>
		/// <value></value>
		public BundleNode Node { get; private set; }

		/// <summary>
		/// A key-value list of all values in the view
		/// </summary>
		/// <returns></returns>
		public ReadOnlyDictionary<string, string> All => GetAll();

		/// <summary>
		/// Accesses a field's value within this view by the field's name.
		/// </summary>
		/// <value></value>
		public string this[string id] 
		{
			get => Node[id].Value;
			set => Node[id].Value = value;
		}

		/// <summary>
		/// Retrieves a read-only key/value list of all values in the view.
		/// </summary>
		/// <param name="includeEmpty"></param>
		/// <returns></returns>
		public ReadOnlyDictionary<string, string> GetAll(bool includeEmpty = false)
		{
			var res = new Dictionary<string, string>();

			foreach (var n in Node.Nodes.Values)
			{
				if (n is FieldNode field)
				{
					if (!String.IsNullOrEmpty(n.Value) || includeEmpty)
					{
						res.Add(n.Name, n.Value);
					}
				}
			}

			return new ReadOnlyDictionary<string, string>(res);
		}
	}
}