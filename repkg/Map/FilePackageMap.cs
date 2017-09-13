using repkg.IO;
using System;
using System.Collections.Generic;
using System.Linq;
namespace repkg.Map
{
	public class FilePackageMap : IPackageMap
	{
		Dictionary<string, MapItem> _map;

		public FilePackageMap(ILineReader reader)
		{
			_map = new Dictionary<string, MapItem>();

			foreach (var line in reader.Read())
			{
				var item = TryParseMapItem(line);

				if (item != null)
					_map.Add(item.OldPackage.Name, item);
			}
		}

		public MapItem GetMapItemFor(string package)
		{
			MapItem item = null;
			_map.TryGetValue(package, out item);
			return item;
		}

		public int ItemCount => _map?.Keys.Count ?? 0;

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

			if (oldPackageInfo.Length != 2)
				throw new NotSupportedException($"Invalid format for the old package definition:\nExpected: \"Name @Version\"\nActual: \"{packages[0]}\"");

			var item = new MapItem()
			{
				OldPackage = new MapItem.Package()
				{
					Name = oldPackageInfo[0],
					Version = oldPackageInfo[1],
				},
				NewPackages = new List<MapItem.Package>()
			};

			if (!string.IsNullOrWhiteSpace(packages[1]))
			{
				foreach (var newVersion in SplitTrim(packages[1], ":"))
				{
					var newPackageInfo = SplitTrim(newVersion, "@");

					if (oldPackageInfo.Length != 2)
						throw new NotSupportedException($"Invalid format for the old package definition:\nExpected: \"Name @Version\"\nActual: \"{packages[0]}\"");

					item.NewPackages.Add(new MapItem.Package()
					{
						Name = newPackageInfo[0],
						Version = newPackageInfo[1]
					});
				}
			}

			return item;
		}

		private string[] SplitTrim(string value, string separator)
		{
			return value.Split(new string[] { separator }, StringSplitOptions.None)
						.Select(s => s.Trim())
						.ToArray();
		}
	}
}