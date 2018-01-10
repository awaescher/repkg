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
			{
				Console.WriteLine("The application has to be started with two arguments: The path to start the conversion for and a mapping file to use.");
				return;
			}

			var path = args[0];
			if (!Directory.Exists(path))
			{
				Console.WriteLine("The path does not exist:" + Environment.NewLine + path);
				return;
			}

			var mappingFile = args[1];
			if (!File.Exists(mappingFile))
			{
				Console.WriteLine("The mapping file does not exist:" + Environment.NewLine + path);
				return;
			}

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
