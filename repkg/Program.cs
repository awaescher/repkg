using repkg.IO;
using repkg.Log;
using repkg.Map;
using System;
using System.IO;

namespace repkg
{
	static class Program
	{
		public static void Main(string[] args)
		{
			if (args?.Length != 2)
				throw new ArgumentException("The application has to be started with two arguments: The path to start the conversion for and a mapping file to use.");

			var path = args[0];
			if (!Directory.Exists(path))
				throw new ArgumentException("The path does not exist:" + Environment.NewLine + path);

			var mappingFile = args[1];
			if (!File.Exists(mappingFile))
				throw new ArgumentException("The mapping file does not exist:" + Environment.NewLine + path);

			Console.WriteLine("Starting package file conversion ...");
			Console.WriteLine("Path: " + args[0]);

			var converter = new PackageMigrator(
					new FilePackageMap(new FileLineReader(mappingFile)),
					new PackageFileFinder(path),
					new ConsoleLog()
				);

			converter.Start();

			Console.WriteLine("Press any key to quit.");
			Console.ReadKey();
		}
	}
}
