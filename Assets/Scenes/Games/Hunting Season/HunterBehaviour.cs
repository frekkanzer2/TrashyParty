using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBehaveour : MonoBehaviour
{
    
    public GameObject bullet;
    public Vector3 objcoords;

    IEnumerator Funzione(float tempo)
    {
        yield return new WaitForSeconds(tempo);

        Transform hunterpos = GetComponent<Transform>();
        Instantiate(bullet, hunterpos.position, Quaternion.identity);
        if (tempo > 0.3) StartCoroutine(Funzione(tempo - 0.1f));
        else StartCoroutine(Funzione(0.1f));
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Funzione(5));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
