using Bundles;

namespace Bundles.Test
{
	public class CustomField : BundleField
	{
		public CustomField(BundleNode parent, string fieldName) : base(parent, fieldName)
		{

		}

		public string TestProperty => "True";
	}
}