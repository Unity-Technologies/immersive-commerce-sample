using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EnableDetailedLogging
{
    static EnableDetailedLogging()
    {
        // Enable full stack traces for all log types
        Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
        Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
        Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);

        Debug.Log("Detailed logging enabled with full stack traces");
    }
}
