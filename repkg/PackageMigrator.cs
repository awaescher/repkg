using repkg.IO;
using repkg.Log;
using repkg.Map;
using System;
using System.Diagnostics;

namespace repkg
{
	public class PackageMigrator
	{
		private readonly IPackageMap _map;
		private readonly PackageFileFinder _configFilesProvider;
		private readonly ILog _log;

		public PackageMigrator(IPackageMap map, PackageFileFinder configFilesProvider, ILog log)
		{
			_map = map ?? throw new ArgumentNullException(nameof(map));
			_configFilesProvider = configFilesProvider ?? throw new ArgumentNullException(nameof(configFilesProvider));
			_log = log ?? throw new ArgumentNullException(nameof(log));
		}

		public void Start()
		{
			int touchedFiles = 0;
			var watch = Stopwatch.StartNew();

			var converter = new PackageFileConverter(_map);

			foreach (var file in _configFilesProvider.Find())
			{
				var convertedFile = converter.Convert(file);

				if (convertedFile.WasTouched)
				{
					touchedFiles++;
					FileExtensions.WriteAllLinesWithoutLastNewline(file, convertedFile.Lines);
				}
			}

			watch.Stop();
			_log.Log($"Finished converting {touchedFiles} file{(touchedFiles == 1 ? "" : "s")} in {watch.Elapsed.TotalSeconds.ToString("n2")} seconds.");
		}
	}
}
