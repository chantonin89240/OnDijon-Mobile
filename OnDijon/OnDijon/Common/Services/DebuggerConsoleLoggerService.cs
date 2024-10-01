using System;
using System.IO;
using OnDijon.Common.Services.Interfaces;

namespace OnDijon.Common.Services
{
	public class DebuggerConsoleLoggerService : ILoggerService
	{
		public void Warning(string info = null, string callingMethod = "", string callerFilePath = "", int callerLineNumber = -1)
		{
			DoLog("WARN", info, callingMethod, callerFilePath, callerLineNumber);		}

		public void Debug(string info = null, Exception ex = null, string callingMethod = "", string callerFilePath = "", int callerLineNumber = -1)
		{
			DoLog("DEBUG", info, callingMethod, callerFilePath, callerLineNumber, ex);
		}

		public void Info(string info = null, string callingMethod = "", string callerFilePath = "", int callerLineNumber = -1)
		{
			DoLog("INFO", info, callingMethod, callerFilePath, callerLineNumber);
		}

		public void Error(string info = null, Exception ex = null, string callingMethod = "", string callerFilePath = "", int callerLineNumber = -1)
		{
			DoLog("ERROR", info, callingMethod, callerFilePath, callerLineNumber, ex);
		}

		public void Fatal(string info = null, Exception ex = null, string callingMethod = "", string callerFilePath = "", int callerLineNumber = -1)
		{ 
			DoLog("FATAL", info, callingMethod, callerFilePath, callerLineNumber, ex);
		}

		private void DoLog(string level, string info, string callingMethod, string callerFilePath, int callerLineNumber, Exception ex = null)
		{
			var log = $"{DateTime.Now.ToString()}:{level}:{GetClassNameFromFilePath(callerFilePath)}.{callingMethod}  at {callerLineNumber}: {info}";
			if (ex != null)
			{
				log += $"/r/n{ex}";
			}
			
			System.Diagnostics.Debug.WriteLine(log);

		}

		/// <summary>
		/// gets the class name from the full path
		/// </summary>
		/// <param name="filePath">full path of source file at compile time</param>
		/// <returns>class name</returns>
		private string GetClassNameFromFilePath(string filePath)
		{
			try
			{
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filePath.Replace('\\', directorySeparatorChar)));
			}
			catch
			{
				return filePath;
			}
		}

	}
}