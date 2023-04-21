using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAreaBehaviour : MonoBehaviour
{

    public int TeamReference;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BeachVolleyBall" && !GameManager.Instance.IsGameEnded() && GameManager.Instance.IsGameStarted())
            BeachVolleyGameController.Instance.Teams.Find(t => t.Id == TeamReference).KillRandomPlayer();
    }

}
