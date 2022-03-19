using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Bundles
{
	/// <summary>
	/// Represents an automatically-generated <see cref="Nodes.FieldNode"/>
	/// </summary>
	public class BundleField<T> : IBundleField
	{
		/// <summary>
		/// BundleConverter instance responsible for converting this
		/// fields values to and from their string representation
		/// </summary>
		public static readonly BundleConverter<T> Converter;

		static BundleField()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();
			var type = typeof(BundleConverter<T>);

			foreach (var assembly in assemblies)
			{
				var c = assembly
					.GetTypes()
					.Where(t => type.IsAssignableFrom(t))
					.FirstOrDefault();

				if (c != null)
				{
					var converter = Activator.CreateInstance(c) as BundleConverter<T>;
					Converter = converter;
					break;
				}
			}
		}

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
		public T Value
		{
			get => Converter.StringToValue(Node[Name].Value);//Node[Name].Value;
			set => Node[Name].Value = Converter.ValueToString(value);//Node[Name].Value = value;
		}
	}

	/// <summary>
	/// Basic BundleField which represents a string value
	/// </summary>
	public class BundleField : BundleField<string>
	{
		/// <summary>
		/// Create a new BundleField
		/// </summary>
		/// <param name="node"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public BundleField(BundleNode node, string name) : base(node, name)
		{

		}
	}
}