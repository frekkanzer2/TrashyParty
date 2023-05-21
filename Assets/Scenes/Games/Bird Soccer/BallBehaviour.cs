using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish") && !GameManager.Instance.IsGameEnded())
        {
            if (collision.gameObject.name == "Soccer Net - DX") GameManager.Instance.Teams.Find(t => t.Id == 2).KillAllPlayers();
            else if (collision.gameObject.name == "Soccer Net - SX") GameManager.Instance.Teams.Find(t => t.Id == 1).KillAllPlayers();
        }
    }
}
