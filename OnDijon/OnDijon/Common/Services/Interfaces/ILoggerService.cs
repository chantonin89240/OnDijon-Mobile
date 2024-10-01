using System;
using System.Runtime.CompilerServices;

namespace OnDijon.Common.Services.Interfaces
{
	public interface ILoggerService
	{
	/// <summary>
    /// Log Warnings with the specified info, callingMethod, callerFilePath, callerLineNumber
    /// </summary>
    /// <param name="info">Info.</param>
    /// <param name="callingMethod">Calling method.</param>
    /// <param name="callerFilePath">Caller file path.</param>
    /// <param name="callerLineNumber">Caller line number.</param>
    void Warning(
      string info = null,
      [CallerMemberName] string callingMethod = "",
      [CallerFilePath] string callerFilePath = "",
      [CallerLineNumber] int callerLineNumber = -1);

    /// <summary>
    /// Log Debug with the specified info, exception, callingMethod, callerFilePath, callerLineNumber 
    /// </summary>
    /// <param name="info">Info.</param>
    /// <param name="ex">Ex.</param>
    /// <param name="callingMethod">Calling method.</param>
    /// <param name="callerFilePath">Caller file path.</param>
    /// <param name="callerLineNumber">Caller line number.</param>
    void Debug(
      string info = null,
      Exception ex = null,
      [CallerMemberName] string callingMethod = "",
      [CallerFilePath] string callerFilePath = "",
      [CallerLineNumber] int callerLineNumber = -1);

    /// <summary>
    /// Log Info with the specified info, callingMethod, callerFilePath, callerLineNumber 
    /// </summary>
    /// <param name="info">Info.</param>
    /// <param name="callingMethod">Calling method.</param>
    /// <param name="callerFilePath">Caller file path.</param>
    /// <param name="callerLineNumber">Caller line number.</param>
    void Info(
      string info = null,
      [CallerMemberName] string callingMethod = "",
      [CallerFilePath] string callerFilePath = "",
      [CallerLineNumber] int callerLineNumber = -1);

    /// <summary>
    /// Log Errors with the specified info, exception, callingMethod, callerFilePath, callerLineNumber
    /// </summary>
    /// <param name="info">Info.</param>
    /// <param name="ex">Ex.</param>
    /// <param name="callingMethod">Calling method.</param>
    /// <param name="callerFilePath">Caller file path.</param>
    /// <param name="callerLineNumber">Caller line number.</param>
    void Error(
      string info = null,
      Exception ex = null,
      [CallerMemberName] string callingMethod = "",
      [CallerFilePath] string callerFilePath = "",
      [CallerLineNumber] int callerLineNumber = -1);

    /// <summary>
    /// Log Crashes with the specified info, exception, callingMethod, callerFilePath, callerLineNumber 
    /// </summary>
    /// <param name="info">Info.</param>
    /// <param name="ex">Ex.</param>
    /// <param name="callingMethod">Calling method.</param>
    /// <param name="callerFilePath">Caller file path.</param>
    /// <param name="callerLineNumber">Caller line number.</param>
    void Fatal(
      string info = null,
      Exception ex = null,
      [CallerMemberName] string callingMethod = "",
      [CallerFilePath] string callerFilePath = "",
      [CallerLineNumber] int callerLineNumber = -1);
  }
}