using System;
using System.IO;

namespace repkg.IO
{
	public static class FileExtensions
	{
		public static void WriteAllLinesWithoutLastNewline(string path, params string[] lines)
		{
			// thanks to Tiago Freitas Leal & Virtlink
			// https://stackoverflow.com/a/42034211/704281

			if (path == null)
				throw new ArgumentNullException(nameof(path));
			if (lines == null)
				throw new ArgumentNullException(nameof(lines));

			using (var stream = File.OpenWrite(path))
			{
				stream.SetLength(0);
				using (var writer = new StreamWriter(stream))
				{
					if (lines.Length > 0)
					{
						for (var i = 0; i < lines.Length - 1; i++)
							writer.WriteLine(lines[i]);

						writer.Write(lines[lines.Length - 1]);
					}
				}
			}
		}
	}
}
