using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace repkg.Map
{
	public class FilePackageMap : IPackageMap
	{
		Dictionary<string, MapItem> _map;

		public FilePackageMap(string mappingFile)
		{
			_map = new Dictionary<string, MapItem>();

			foreach (var line in File.ReadAllLines(mappingFile))
			{
				var item = TryParseMapItem(line);

				if (item != null)
					_map.Add(item.OldPackageName, item);
			}
		}

		public MapItem GetMapItemFor(string package)
		{
			MapItem item = null;
			_map.TryGetValue(package, out item);
			return item;
		}

		private MapItem TryParseMapItem(string line)
		{
			if (string.IsNullOrWhiteSpace(line))
				return null;

			var isComment = line.Trim().StartsWith("#");
			if (isComment)
				return null;

			var packages = SplitTrim(line, "|");
			if (packages.Length != 2)
				throw new NotSupportedException($"Line {line} cannot be split into two parts to map.");

			var oldPackageInfo = SplitTrim(packages[0], "@");
			var newPackageInfo = SplitTrim(packages[1], "@");

			if (oldPackageInfo.Length != 2)
				throw new NotSupportedException($"Invalid format for the old package definition:\nExpected: \"Name @Version\"\nActual: \"{packages[0]}\"");

			return new MapItem()
			{
				OldPackageName = oldPackageInfo[0],
				OldVersion = oldPackageInfo[1],
				NewPackageName = newPackageInfo.Length == 2 ? newPackageInfo[0] : "",
				NewVersion = newPackageInfo.Length == 2 ? newPackageInfo[1] : ""
			};
		}

		private string[] SplitTrim(string value, string separator)
		{
			return value.Split(new string[] { separator }, StringSplitOptions.None)
						.Select(s => s.Trim())
						.ToArray();
		}
	}
}