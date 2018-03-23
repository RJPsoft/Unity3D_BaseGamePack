using System;
using UnityEngine;

public static class LogManager
{
    #region Public Methods

    public static void Log(string message)
    {
        LogIfDebug(() => Debug.Log(message));
    }

    public static void Log(string objectName, string message)
    {
        LogIfDebug(() => Debug.Log(string.Format("Object: {0} -> Message: {1}", objectName, message)));
    }

    public static void LogError(string message)
    {
        LogIfDebug(() => Debug.LogError(message));
    }

    public static void LogWarning(string message)
    {
        LogIfDebug(() => Debug.LogWarning(message));
    }

    private static void LogIfDebug(Action log)
    {
        if (Debug.isDebugBuild)
        {
            log.Invoke();
        }
    }

    #endregion Public Methods
}