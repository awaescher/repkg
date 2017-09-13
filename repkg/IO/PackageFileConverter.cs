using repkg.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			foreach (var lineString in File.ReadAllLines(file))
			{
				var convertedLine = ConvertLine(lineString);

				foreach (var line in convertedLine.Results)
				{
					var isDuplicate = newLines.Contains(line);
					var isDuplicateToRemove = convertedLine.WasTouched && isDuplicate;

					var keepLine = !isDuplicateToRemove && !convertedLine.WasEliminated;
					if (keepLine)
						newLines.Add(line);

					if (convertedLine.WasTouched)
						result.WasTouched = true;
				}
			}

			result.Lines = newLines.ToArray();

			return result;
		}

		private ConvertedLine ConvertLine(string line)
		{
			var match = Regex.Match(line, PACKAGE_PATTERN);

			var result = new ConvertedLine() {
				Results = new List<string> { line },
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
					result.Results.Clear();

					foreach (var newPackage in item.NewPackages)
					{
						string versionPattern = VERSION_PATTERN.Replace("%VERSION%", item.OldPackage.Version);

						string newLine = Regex.Replace(line, PACKAGE_PATTERN, newPackage.Name);
						newLine = Regex.Replace(newLine, versionPattern, newPackage.Version);
						result.Results.Add(newLine);
					}
				}

				if (item.ShouldRemovePackage)
					result.Results.Clear();

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
		public List<string> Results { get; set; }

		public bool WasTouched { get; set; }

		public bool WasEliminated => WasTouched && (Results?.Count < 1);
	}
}
