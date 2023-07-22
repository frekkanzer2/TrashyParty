using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestBehaviour : MonoBehaviour
{
    public SpriteRenderer displayer;
    private string player;

    public void CollisionWithPlayer(PlatformerPlayerColorfulNests player)
    {
        this.player = player.GetName();
        this.displayer.sprite = player.GetBirdSprite();
    }
    public string GetAttachedPlayerName() => this.player;
}
