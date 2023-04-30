using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUfoBehaviour : MonoBehaviour
{

    private bool init = false;
    private Vector2 v;

    public void Initialize(Vector2 direction)
    {
        init = true;
        v = direction;
        this.GetComponent<Rigidbody2D>().AddForce(v, ForceMode2D.Force);
        Destroy(this.gameObject, 20);
    }
}
