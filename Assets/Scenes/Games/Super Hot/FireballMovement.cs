using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, 10);
    }
    private void FixedUpdate()
    {
        this.transform.Translate(new(0.25f, 0f, 0f));
    }
}
