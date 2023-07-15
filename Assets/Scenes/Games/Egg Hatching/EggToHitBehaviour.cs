using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggToHitBehaviour : MonoBehaviour
{

    private GameObject LastTouchBird = null;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D rb;
    private bool isExploded = false;

    private void Start()
    {
        this.SpriteRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.rb = this.GetComponent<Rigidbody2D>();
        this.rb.gravityScale = 0;
    }

    public void OnStart()
    {
        Vector2 generatedForce = Vector2.zero;
        this.rb.gravityScale = 1.5f;
        generatedForce = new Vector2(Random.Range(-1f, 1f) * 5, Random.Range(0.5f, 1f) * 5);
        this.GetComponent<Rigidbody2D>().AddForce(generatedForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isExploded)
        {
            PlatformerPlayer pp = collision.gameObject.GetComponent<PlatformerPlayer>();
            this.SpriteRenderer.sprite = pp.GetBirdSprite();
            LastTouchBird = collision.gameObject;
        }
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.layer == Constants.LAYER_GROUND) && !isExploded)
        {
            float decrease = 0;
            if (GameManager.Instance.Teams.Count <= 3) decrease = 0.025f;
            else if (GameManager.Instance.Teams.Count <= 5) decrease = 0.02f;
            else if (GameManager.Instance.Teams.Count <= 7) decrease = 0.01f;
            else if (GameManager.Instance.Teams.Count == 8) decrease = 0.008f;
            if (this.transform.localScale.x > 0.5f) this.transform.localScale = new(this.transform.localScale.x - decrease, this.transform.localScale.y - decrease, this.transform.localScale.z);
            else OnExplode();
        }
    }

    private void OnExplode()
    {
        isExploded = true;
        List<TeamDto> loserTeams;
        if (LastTouchBird is null) loserTeams = GameManager.Instance.Teams;
        else loserTeams = GameManager.Instance.Teams.FindAll(t => t.GetAlivePlayers()[0].GetName()!=LastTouchBird.GetComponent<IPlayer>().GetName());
        foreach (TeamDto tDto in loserTeams)
            tDto.KillAllPlayers();
        Destroy(this.gameObject);
    }

}
