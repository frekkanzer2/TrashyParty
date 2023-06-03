using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggerLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        this.gameObject.AddComponent<UnityLogManager>();
        Singleton<ILogManager>.Instance.Write("Initialized Unity Logger as default logger");
#else
        this.gameObject.AddComponent<FileLogManager>();
        Singleton<ILogManager>.Instance.Write("Initialized file logger as default logger destination");
#endif
        Log.Logger = Singleton<ILogManager>.Instance;
    }
}
