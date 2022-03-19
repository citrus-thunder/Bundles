using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bundles.Nodes
{
	/// <summary>
	/// A Bundle represents a complete data entity that can be loaded and saved to a
	/// predetermined location on the filesystem or to/from a given stream
	/// </summary>
	[BundleElement("Bundle")]
	public class Bundle : BundleNode
	{
		/// <summary>
		/// Location in the filesystem to which this bundle will save and attempt to load from
		/// </summary>
		/// <returns></returns>
		public virtual string SavePath => Path.Join(Bundler.DataRoot, Parent.NodePath.ToPath(), Attributes["file"]);

		/// <summary>
		/// XmlSettings reference used by the Bundle's default XmlSerializer
		/// </summary>
		/// <value></value>
		public XmlWriterSettings XmlWriterSettings
		{
			get => m_xmlWriterSettings;
			set => m_xmlWriterSettings = value;
		}
		private XmlWriterSettings m_xmlWriterSettings = new XmlWriterSettings()
		{
			OmitXmlDeclaration = true,
			Indent = true,
			CloseOutput = true
		};

		/// <inheritdoc />
		protected sealed override void Attach(BundleNode parentNode)
		{
			// Sealed method: This logic is fundamental to this class' capabilities
			// and should not be overridden.

			if (Bundler == null)
			{
				return;
			}

			var fileName = Name;
			if (Attributes.TryGetValue("file", out string file))
			{
				fileName = file;
			}

			fileName += Bundler.BundleFileExtension;

			if (!Attributes.TryAdd("file", fileName))
			{
				Attributes["file"] = fileName;
			}
		}

		/// <inheritdoc />
		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			writer.WriteAttributeString("name", Name);
		}

		/// <summary>
		/// Save the Bundle to the location specified by <see cref="SavePath"/>
		/// using the Bundle's default <see cref="XmlSerializer"/>
		/// </summary>
		public void Save()
		{
			Directory.CreateDirectory(Path.GetDirectoryName(SavePath));

			var XmlSettings = new XmlWriterSettings()
			{
				OmitXmlDeclaration = true,
				Indent = true,
				CloseOutput = true
			};

			var s = new XmlSerializer(GetType());

			using (var writer = XmlWriter.Create(SavePath, XmlWriterSettings))
			{
				s.Serialize(writer, this);
				writer.Flush();
				writer.Close();
			}
		}

		/// <summary>
		/// Save the Bundle to the provided <see cref="Stream"/>
		/// </summary>
		/// <param name="stream"></param>
		public virtual void SaveToStream(Stream stream)
		{
			var s = new XmlSerializer(GetType());
			s.Serialize(stream, this);
		}

		/// <summary>
		/// Load the Bundle's information from a local file specified by <see cref="SavePath"/>
		/// using the Bundle's default <see cref="XmlSerializer"/>
		/// </summary>
		public void Load()
		{
			if (!File.Exists(SavePath))
			{
				throw new IOException($"Error loading bundle: File not found at {SavePath}");
			}

			var s = new XmlSerializer(GetType());

			using (var reader = XmlReader.Create(SavePath))
			{
				var b = s.Deserialize(reader) as BundleNode;
				Merge(b);
			}
		}

		/// <summary>
		/// Load the Bundle's information from the given <see cref="Stream"/>
		/// </summary>
		/// <param name="stream"></param>
		public virtual void LoadFromStream(Stream stream)
		{
			var s = new XmlSerializer(GetType());
			var b = s.Deserialize(stream) as BundleNode;
			Merge(b);
		}

		/// <summary>
		/// Attempt to load the Bundle's information from a local file specified by <see cref="SavePath"/>
		/// using the Bundle's default <see cref="XmlSerializer"/>
		/// </summary>
		/// <returns><c>true</c> if success, <c>false</c> if failure</returns>
		public bool TryLoad()
		{
			try
			{
				Load();
				return true;
			}
			catch (IOException)
			{
				return false;
			}
		}
	}
}