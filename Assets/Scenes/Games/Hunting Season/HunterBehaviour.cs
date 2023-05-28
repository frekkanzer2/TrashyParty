using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBehaveour : MonoBehaviour
{
    
    public GameObject bullet;
    public Vector3 objcoords;
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(bullet, objcoords, Quaternion.identity);
        Debug.Log("sium");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
