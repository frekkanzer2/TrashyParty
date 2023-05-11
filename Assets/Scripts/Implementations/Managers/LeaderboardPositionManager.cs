using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardPositionManager : MonoBehaviour
{
    public SpriteRenderer renderer;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI rankingText;
    public int rank;
    public int connectedPlayerId;
    private bool active = true;
    public GameObject winBadge, loseBadge, neutralBadge;

    public void SetData(Sprite s, int points, bool? hasWon, int playerId)
    {
        renderer.sprite = s;
        pointsText.text = $"{points}";
        this.connectedPlayerId = playerId;
        if (hasWon != null)
        {
            neutralBadge.SetActive(false);
            if ((bool)hasWon) { 
                winBadge.SetActive(true);
                loseBadge.SetActive(false);
            }
            else if (!(bool)hasWon) { 
                loseBadge.SetActive(true);
                winBadge.SetActive(false);
            }
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
        this.gameObject.SetActive(active);
    }
    public bool IsActive() => this.active;

    private void Start()
    {
        rankingText.text = $"{rank}";
        winBadge.SetActive(false);
        loseBadge.SetActive(false);
    }

    public void SetRankingText(int rank)
    {
        this.rankingText.text = $"{rank}";
    }

}
