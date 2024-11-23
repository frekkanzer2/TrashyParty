using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBehaviour : MonoBehaviour
{
    public GameObject bullet;

    IEnumerator GenerateBullet(float tempo)
    {
        Vector3 spawnPos;
        System.Random rand = new System.Random();
        if (rand.Next(0, 2) == 0)
            spawnPos = Vector3Extensions.Variation(this.transform.position, new(-3.46f, 3.2f, 0));
        else
            spawnPos = Vector3Extensions.Variation(this.transform.position, new(-3.46f, 1, 0));
        GameObject b = Instantiate(bullet, spawnPos, Quaternion.identity);
        Destroy(b, 5);
        yield return new WaitForSeconds(tempo);
        if (!GameManager.Instance.IsGameEnded())
        {
            if (tempo > 1f) StartCoroutine(GenerateBullet(tempo - 0.25f));
            else StartCoroutine(GenerateBullet(1));
        }
    }

    public void StartGeneration()
    {
        StartCoroutine(GenerateBullet(5));
    }
}
