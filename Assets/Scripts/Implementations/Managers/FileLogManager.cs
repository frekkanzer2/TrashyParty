using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileLogManager : Singleton<ILogManager>, ILogManager
{

    private string filename = "gamelog";
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
        DateTime actualDate = DateTime.Now;
        string completeFilename = $"{filename}-{actualDate.ToString("dd.MM.yyyy.HH.mm.ss")}.txt";
        if (File.Exists(completeFilename)) File.Delete(completeFilename);
        writer = File.CreateText(completeFilename);
        writer.WriteLine($"Log of {actualDate.ToString("dd/MM/yyyy - HH:mm:ss")}");
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
}
