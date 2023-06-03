using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{

    private static T _instance = default;
    private static bool isInit = false;
    public static T Instance
    {
        get
        {
            if (!isInit) throw new System.NullReferenceException("Trying to get the singleton class, but it's not attached as a component!");
            return _instance;
        }
    }

    protected void InitializeSingleton(T subclassInstance)
    {
        isInit = true;
        _instance = subclassInstance;
    }

}
