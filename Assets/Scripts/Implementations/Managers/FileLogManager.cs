using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileLogManager : Singleton<ILogManager>, ILogManager
{

    private string filename = "gamelogs/gamelog";
    private StreamWriter writer = null;

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
        if (!Directory.Exists("gamelogs"))
        {
            Directory.CreateDirectory("gamelogs");
        }
        DateTime actualDate = DateTime.Now;
        string completeFilename = $"{filename}-{actualDate.ToString("dd.MM.yyyy.HH.mm.ss")}.txt";
        if (File.Exists(completeFilename)) File.Delete(completeFilename);
        writer = File.CreateText(completeFilename);
        writer.WriteLine($"Log of {actualDate.ToString("dd/MM/yyyy - HH:mm:ss")}");
        Application.logMessageReceived -= LogCallback;
        Application.logMessageReceived += LogCallback;
    }

    public void SaveAndClose()
    {
        writer.Close();
    }

    public void Write(string message) => Write(ILogManager.Level.Info, message);
    public void Write(ILogManager.Level level, string message)
    {
        Id++;
        writer.WriteLine($"{Id} | {level} | {DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss")} | {message}");
    }

    public void Write(Exception exception)
    {
        Id++;
        writer.WriteLine($"{Id} | {ILogManager.Level.Exception} | {DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss")} | Thrown exception {exception.GetType()}");
        writer.WriteLine($"--- STACK TRACE ---");
        writer.WriteLine($"{exception.StackTrace}");
        writer.WriteLine($"--- END OF STACK TRACE ---");
    }

    //Called when there is an exception
    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        Write(condition.Split(':')[0], condition.Split(':')[1].TrimStart(), stackTrace);
    }

    public void Write(string exceptionName, string details, string stackTrace)
    {
        Id++;
        writer.WriteLine($"{Id} | {ILogManager.Level.Exception} | {DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss")} | Thrown exception {exceptionName} with error \"{details}\"");
        writer.WriteLine($"--- STACK TRACE ---");
        writer.WriteLine($"{stackTrace.TrimEnd()}");
        writer.WriteLine($"--- END OF STACK TRACE ---");
    }
}
