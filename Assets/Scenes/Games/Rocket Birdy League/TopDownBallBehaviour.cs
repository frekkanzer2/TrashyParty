using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownBallBehaviour : MonoBehaviour
{

    public float playerBounceForce = 10f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish") && !GameManager.Instance.IsGameEnded())
        {
            if (collision.gameObject.name.Contains("DX")) GameManager.Instance.Teams.Find(t => t.Id == 2).KillAllPlayers();
            else if (collision.gameObject.name.Contains("SX")) GameManager.Instance.Teams.Find(t => t.Id == 1).KillAllPlayers();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 power;
            TopDownPlayer player = collision.gameObject.GetComponent<TopDownPlayer>();
            if (player.IsSprinting()) power = collision.contacts[0].normal * (playerBounceForce * 4);
            else power = collision.contacts[0].normal * playerBounceForce;
            rb.AddForce(power, ForceMode2D.Impulse);
        }
    }

}
