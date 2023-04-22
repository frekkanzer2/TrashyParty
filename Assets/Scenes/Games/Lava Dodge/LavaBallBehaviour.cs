using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBallBehaviour : MonoBehaviour
{

    public Vector2 Destination;
    public float Speed = 0;

    public void Initialize(Vector2 destination, float speed)
    {
        this.Destination = destination;
        this.Speed = speed;
        Physics2D.IgnoreLayerCollision(2, 2);
        Physics2D.IgnoreLayerCollision(2, 6);
    }

    private void FixedUpdate()
    {
        if (Destination == null || Speed == 0) return;
        if (GameManager.Instance.IsGameEnded()) return;
        transform.position = Vector3.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
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
