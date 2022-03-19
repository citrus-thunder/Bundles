using System.IO;

namespace Bundles
{
	/// <summary>
	/// Extension Methods for Bundles classes and interfaces
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Converts a NodePath-syntax string to a local-system-compatible path.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string ToPath(this string source) => Path.Join(source.Split('/'));
	}
}