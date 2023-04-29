using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IPlayer winner = collision.gameObject.GetComponent<IPlayer>();
            List<TeamDto> loserTeams = GameManager.Instance.Teams.FindAll(t => t.GetAlivePlayers().Count > 0 && !t.players[0].Equals(winner));
            foreach (TeamDto team in loserTeams)
                team.players[0].OnDeath();
            ((PlatformerPlayer)winner).gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            Destroy(this.gameObject);
        }
    }
}
