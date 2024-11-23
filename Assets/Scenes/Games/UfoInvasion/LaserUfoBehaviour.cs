using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUfoBehaviour : MonoBehaviour
{
    public void Initialize(Vector2 direction)
    {
        this.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Force);
        Destroy(this.gameObject, 20);
    }
}
