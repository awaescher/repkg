using repkg.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace repkg.IO
{
	public class PackageFileConverter
	{
		private const string PACKAGE_PATTERN = "(?<=package\\s*id\\s*=\\s*\").*?(?=\")";
		private const string VERSION_PATTERN = "(?<=version\\s*=\\s*\")%VERSION%(?=\")";

		private readonly IPackageMap _map;

		public PackageFileConverter(IPackageMap map)
		{
			_map = map ?? throw new ArgumentNullException(nameof(map));
		}

		public ConvertedFile Convert(string file)
		{
			var result = new ConvertedFile();

			var newLines = new List<string>();
			foreach (var line in File.ReadAllLines(file))
			{
				var convertedLine = ConvertLine(line);

				var isUntouched = !convertedLine.WasTouched;
				var isUnique = !newLines.Contains(convertedLine.Value);

				if (isUntouched || isUnique)
					newLines.Add(convertedLine.Value);

				if (convertedLine.WasTouched)
					result.WasTouched = true;
			}

			result.Lines = newLines.ToArray();

			return result;
		}

		private ConvertedLine ConvertLine(string line)
		{
			var match = Regex.Match(line, PACKAGE_PATTERN);

			var result = new ConvertedLine() {
				Value = line,
				WasTouched = false
			};

			if (!match.Success)
				return result;

			var oldPackageName = match.Value;
			var item = _map.GetMapItemFor(oldPackageName);
			if (item != null)
			{
				if (item.ShouldConvertPackage)
				{
					string versionPattern = VERSION_PATTERN.Replace("%VERSION%", item.OldVersion);

					line = Regex.Replace(line, PACKAGE_PATTERN, item.NewPackageName);
					line = Regex.Replace(line, versionPattern, item.NewVersion);
				}

				if (item.ShouldRemovePackage)
				{
					// ?!
				}

				result.Value = line;
				result.WasTouched = true;
			}

			return result;
		}
	}

	public class ConvertedFile
	{
		public string[] Lines { get; set; }
		public bool WasTouched { get; set; }
	}

	class ConvertedLine
	{
		public string Value { get; set; }
		public bool WasTouched { get; set; }
	}
}
