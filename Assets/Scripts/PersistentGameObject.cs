using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentGameObject : MonoBehaviour
{
    public static PersistentGameObject i;

    void Awake()
    {
        if (!i)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

}
