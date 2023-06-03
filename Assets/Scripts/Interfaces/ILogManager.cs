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
    public void SaveAndClose();

    enum Level
    {
        Info,
        Warning,
        Important,
        Exception
    }
}
