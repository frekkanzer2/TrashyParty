using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyBallBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2D;
    void FixedUpdate() { if (rb2D.velocity.magnitude > 80) { rb2D.velocity = rb2D.velocity.normalized * 80; } }
}
