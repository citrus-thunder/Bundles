using System.IO;

using Xunit;

using Bundles.Nodes;

namespace Bundles.Test
{
	[Collection("Unique: Bundler Generation")]
	public class FolderTests
	{
		private const string XML_ROOT = "unit/nodes/data/folder/";

		[Theory]
		[InlineData("folder.xml")]
		public void TestGenerateEmptyFolder(string file)
		{
			var xmlFile = XML_ROOT + file;
			var bundler = TestConfig.GetBundler(xmlFile);

			var folder = bundler.Data["Test Folder"];

			var dirPath = Path.Join(bundler.DataRoot, folder.NodePath.ToPath());
			
			// We just created the folder in order to get path info, so we need
			// to delete it now and see if re-creating the Bundler puts it back.
			if (Directory.Exists(dirPath))
			{
				Directory.Delete(dirPath);
			}

			var b = TestConfig.GetBundler(xmlFile);

			Assert.True(Directory.Exists(dirPath));
		}
	}
}