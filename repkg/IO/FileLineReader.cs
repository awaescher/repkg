using System;
using System.IO;

namespace repkg.IO
{
	public class FileLineReader : ILineReader
	{
		private readonly string _file;

		public FileLineReader(string file)
		{
			if (string.IsNullOrEmpty(file))
				throw new ArgumentNullException(nameof(file));

			_file = file;
		}

		public string[] Read() => File.ReadAllLines(_file);
	}
}
