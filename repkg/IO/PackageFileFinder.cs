using System;
using System.IO;

namespace repkg.IO
{
	public class PackageFileFinder
	{
		private readonly string _path;

		public PackageFileFinder(string path)
		{
			_path = path ?? throw new ArgumentNullException(nameof(path));
		}

		public string[] Find()
		{
			return Directory.GetFiles(_path, "*.config", SearchOption.AllDirectories);
		}
	}
}
