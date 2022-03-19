using System;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Bundles.Nodes
{
	/// <summary>
	/// Represents a <see cref="Bundler"/>'s root options node
	/// </summary>
	[BundleElement("Config")]
	public class BundleConfig : BundleNode
	{
		/// <summary>
		/// Creates a new BundleConfig
		/// </summary>
		/// <param name="bundler"></param>
		public BundleConfig(Bundler bundler)
		{
			Bundler = bundler;
		}

		/// <inheritdoc />
		public override string NodePath => "/";

		/// <inheritdoc />
		protected override void BuildAttributes(XmlNode node)
		{
			if (node != null)
			{
				base.BuildAttributes(node);
			}
		}

		/// <inheritdoc />
		protected override void BuildNode(XmlNode node)
		{
			if (node != null)
			{
				base.BuildNode(node);
			}
		}

		/// <inheritdoc />
		protected override void BuildChildren(XmlNode node)
		{
			if (node != null)
			{
				base.BuildChildren(node);
			}

			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList<Assembly>();;
			var type = typeof(BundleOption);

			foreach (var assembly in assemblies)
			{
				var typeList = assembly
					.GetTypes()
					.Where(t => type.IsAssignableFrom(t) && t.IsDefined(typeof(DefaultOptionAttribute), false)
					).ToList();

				foreach (var iType in typeList)
				{
					var defaultOption = iType.GetCustomAttribute<DefaultOptionAttribute>();
					
					var option = Activator.CreateInstance(iType) as BundleOption;
					option.Name = defaultOption.Name;
					option.Value = defaultOption.DefaultValue;

					if (!Nodes.ContainsKey(option.Name))
					{
						AddNode(option);
					}
				}
			}
		}
	}
}