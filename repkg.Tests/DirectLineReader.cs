using repkg.IO;

namespace repkg.Tests
{
	internal class DirectLineReader : ILineReader
	{
		private string[] _lines;

		public DirectLineReader(string[] lines)
		{
			_lines = lines;
		}

		public string[] Read() => _lines;
	}
}