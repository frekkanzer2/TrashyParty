using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBallBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Repels") && !GameManager.Instance.IsGameEnded())
        {
            if (collision.gameObject.name == "Basket - DX") GameManager.Instance.Teams.Find(t => t.Id == 2).KillAllPlayers();
            else if (collision.gameObject.name == "Basket - SX") GameManager.Instance.Teams.Find(t => t.Id == 1).KillAllPlayers();
        }
    }
}
