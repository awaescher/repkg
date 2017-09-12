using System;
using System.Diagnostics;

namespace repkg.Log
{
	public class ConsoleLog : ILog
	{
		public void Log(string value)
		{
			Console.WriteLine(value);
			Debug.WriteLine(value);
		}
	}
}
