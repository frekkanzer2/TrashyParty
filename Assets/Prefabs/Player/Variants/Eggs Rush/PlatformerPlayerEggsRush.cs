using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerPlayerEggsRush : PlatformerPlayer
{

    bool HasPickedInThisRound = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnterWithPlayer(collision);
        if (collision.gameObject.CompareTag("Pickable") && !GameManager.Instance.IsGameEnded())
        {
            HasPickedInThisRound = true;
            ((EggsRushGameManager)GameManager.Instance).OnEggCollided(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

    public void NewRound()
    {
        HasPickedInThisRound = false;
    }

    public bool HasPickedEgg { get { return HasPickedInThisRound; } set { HasPickedInThisRound = value; } }

}
