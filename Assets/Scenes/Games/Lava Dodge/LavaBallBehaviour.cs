using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBallBehaviour : MonoBehaviour
{

    public Vector2 Destination;
    public float Speed = 0;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 destination, float speed)
    {
        this.Destination = destination;
        this.Speed = speed;
        Physics2D.IgnoreLayerCollision(2, 2);
        Physics2D.IgnoreLayerCollision(2, 6);
    }

    private bool forced = false;
    private void FixedUpdate()
    {
        if (Destination == null || Speed == 0 || forced || GameManager.Instance.IsGameEnded()) return;
        forced = true;
        rb.AddForce(Destination * Speed * 50, ForceMode2D.Force);
    }

    private void OnPlayerCollision(GameObject other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.IsGameEnded())
        {
            other.GetComponent<IPlayer>().OnDeath();
            Destroy(this.gameObject);
        }
    }

    private void OnLimitCollision(GameObject other)
    {
        if (other.name == "static_background_limit")
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnPlayerCollision(collision.gameObject);
        OnLimitCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerCollision(other.gameObject);
        OnLimitCollision(other.gameObject);
    }

}
