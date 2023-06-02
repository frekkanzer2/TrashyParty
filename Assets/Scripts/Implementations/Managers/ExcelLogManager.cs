using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExcelLogManager : Singleton<ILogManager>, ILogManager
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
        Debug.Log($"{Id} | {level} | {DateTime.Now.ToString("dd/MM/yyyy - HH:mm:ss")} | {message}");
    }

}
