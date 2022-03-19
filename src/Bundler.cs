// see: https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmldocument?redirectedfrom=MSDN&view=net-6.0

using System;
using System.IO;
using System.Xml;

using Bundles.Nodes;

namespace Bundles
{
	/// <summary>
	/// Root bundle element. Handles loading BundleDef definition files, <see cref="BundleNode"/> access, and filesystem path resolution.
	/// </summary>
	public class Bundler
	{
		private bool _initialized = false;

		/// <summary>
		/// Creates a new Bundler using a definition file contained in a given XmlDocument
		/// </summary>
		/// <param name="doc"></param>
		public Bundler(XmlDocument doc)
		{
			Initialize(doc);
		}

		/// <summary>
		/// Creates a new Bundler using a definition file at the provided path
		/// </summary>
		/// <param name="path"></param>
		public Bundler(string path)
		{
			var doc = new XmlDocument();
			doc.Load(path);
			Initialize(doc);
		}

		/// <summary>
		/// The application root folder's absolute path in the filesystem
		/// </summary>
		/// <remarks>
		/// Default value is <see cref="Environment.SpecialFolder.ApplicationData"/>
		/// </remarks>
		public virtual string AppRoot { get; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		/// <summary>
		/// Name of this application's root folder located immediately beneath <see cref="AppRoot"/>
		/// </summary>
		/// <remarks>
		/// To get the absolute filesystem path to this folder, use <see cref="DataRoot"/>
		/// </remarks>
		public string DataRootFolder { get; private set; } = "AppBundle";

		/// <summary>
		/// File extension used by this Bundler's <see cref="Bundle"/>s
		/// </summary>
		/// <value></value>
		public string BundleFileExtension
		{
			get => Options["BundleExtension"];
			set => Options["BundleExtension"] = value;
		}

		/// <summary>
		/// Absolute filesystem path to this Bundler's data root.
		/// </summary>
		/// <returns></returns>
		public string DataRoot => Path.Join(AppRoot, DataRootFolder.ToPath());

		/// <summary>
		/// This Bundler's root data node
		/// </summary>
		/// <value></value>
		public BundleNode Data { get; private set; }

		/// <summary>
		/// This Bundler's root configuration node
		/// </summary>
		/// <value></value>
		public BundleNode Config { get; private set; }

		/// <summary>
		/// <see cref="BundleValues"/> view of this Bundler's options
		/// </summary>
		/// <value></value>
		public BundleValues Options { get; private set; }

		/// <summary>
		/// This Bundler's service container
		/// </summary>
		/// <returns></returns>
		public BundleServiceContainer Services { get; set; } = new BundleServiceContainer();

		private void Initialize(XmlDocument doc)
		{
			if (_initialized)
			{
				throw new InvalidOperationException("Bundler has already been initialized");
			}

			var rootNode = doc.SelectSingleNode("BundleDef");

			Build(rootNode);

			_initialized = true;
		}

		/// <summary>
		/// Builds the Bundler's root elements
		/// </summary>
		/// <param name="node"></param>
		protected virtual void Build(XmlNode node)
		{
			var configNode = node.SelectSingleNode("Config");
			var dataNode = node.SelectSingleNode("Data");

			DataRootFolder = dataNode.Attributes["root"].Value;

			Config = new BundleConfig(this);
			
			Config.Build(configNode);

			Options = new BundleValues(Config);

			Data = new BundleData(this);
			Data.Build(dataNode);
		}
	}
}