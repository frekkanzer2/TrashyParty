using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnityLogManager : Singleton<ILogManager>, ILogManager
{

    private void Awake()
    {
        InitializeSingleton(this);
        Initialize();
    }

    private void OnApplicationQuit()
    {
        SaveAndClose();
    }

    public int Id { get; set; }

    public void Initialize()
    {
        Id = 0;
    }

    public void SaveAndClose()
    {
    }

    public void Write(string message) => Write(ILogManager.Level.Info, message);
    public void Write(ILogManager.Level level, string message)
    {
        Id++;
        string msgToDisplay = $" | {Id} | {message}";
        if (level == ILogManager.Level.Info) Debug.Log(msgToDisplay);
        else if (level == ILogManager.Level.Warning) Debug.LogWarning(msgToDisplay);
        if (level == ILogManager.Level.Important || level == ILogManager.Level.Exception) Debug.LogError(msgToDisplay);
    }

    public void Write(Exception exception)
    {
        Id++;
        Debug.LogError($" | {Id} | Thrown exception {exception.GetType()} with stack trace in the next log");
        Debug.LogError($"{exception.StackTrace}");
    }

    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        throw new NotImplementedException();
    }

    public void Write(string exceptionName, string details, string stackTrace)
    {
        throw new NotImplementedException();
    }
}
