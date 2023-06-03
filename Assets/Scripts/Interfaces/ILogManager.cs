using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILogManager
{
    public int Id { get; set; }
    public void Initialize();
    public void Write(string message);
    public void Write(Level level, string message);
    public void Write(System.Exception exception);
    public void Write(string exceptionName, string details, string stackTrace);
    public void SaveAndClose();
    public void LogCallback(string condition, string stackTrace, LogType type);

    enum Level
    {
        Info,
        Warning,
        Important,
        Exception
    }
}
