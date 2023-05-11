using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDoorBehaviour : MonoBehaviour
{

    private bool isTouched = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.IsGameEnded() && !isTouched)
        {
            isTouched = true;
            IPlayer winner = collision.gameObject.GetComponent<IPlayer>();
            List<TeamDto> loserTeams = GameManager.Instance.Teams.FindAll(t => t.GetAlivePlayers().Count > 0 && !t.players[0].Equals(winner));
            foreach (TeamDto team in loserTeams)
                team.players[0].OnDeath();
        }
    }
}
