using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public List<LeaderboardPositionManager> Positions;
    private RankingDto newRanking;
    private RankingDto oldRanking;
    private int numberOfPlayers;
    public float WaitTimeForUpdate;
    public ParticleSystem PS1;
    public ParticleSystem PS2;
    public ParticleSystem PS3;

    private void Start()
    {
        PS1.gameObject.SetActive(false); PS2.gameObject.SetActive(false); PS3.gameObject.SetActive(false);
        numberOfPlayers = (int)AppSettings.Get("N_PLAYERS");
        MoveRankPositions();
        GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomOpens();
        SoundsManager.Instance.StopAllSounds();
        newRanking = (RankingDto)AppSettings.Get(Constants.APPSETTINGS_RANKING_LABEL);
        oldRanking = (RankingDto)AppSettings.Get(Constants.APPSETTINGS_RANKING_PREVIOUS_LABEL);
        for (int i = numberOfPlayers; i < 8; i++)
            Positions[i].SetActive(false);
        List<RankingDto.Rank> oldRanks = oldRanking.GetRanking();
        for (int i = 0; i < numberOfPlayers; i++) Positions[i].SetData(oldRanks[i].PlayerSprite, oldRanks[i].Points, null, oldRanks[i].PlayerId);
        UpdateAllRanks(oldRanks);
        StartCoroutine(UpdateLeaderboard(WaitTimeForUpdate));
    }

    private void MoveRankPositions()
    {
        if (numberOfPlayers == 2)
        {
            Positions[0].gameObject.transform.position = new Vector3(-15, 0, 0);
            Positions[1].gameObject.transform.position = new Vector3(15, 0, 0);
        }
        else if (numberOfPlayers == 3)
        {
            Positions[0].gameObject.transform.position = new Vector3(-18, 0, 0);
            Positions[1].gameObject.transform.position = new Vector3(0, 0, 0);
            Positions[2].gameObject.transform.position = new Vector3(18, 0, 0);
        }
        else if (numberOfPlayers == 4)
        {
            Positions[0].gameObject.transform.position = new Vector3(-23.8f, 0, 0);
            Positions[1].gameObject.transform.position = new Vector3(-5.67f, 0, 0);
            Positions[2].gameObject.transform.position = new Vector3(11.33f, 0, 0);
            Positions[3].gameObject.transform.position = new Vector3(26.5f, 0, 0);
        }
        else if (numberOfPlayers == 5)
        {
            Positions[3].gameObject.transform.position = new Vector3(-8f, -15.5f, 0);
            Positions[4].gameObject.transform.position = new Vector3(8f, -15.5f, 0);
        }
        else if (numberOfPlayers == 6)
        {
            Positions[3].gameObject.transform.position = new Vector3(-18f, -15.5f, 0);
            Positions[4].gameObject.transform.position = new Vector3(0f, -15.5f, 0);
            Positions[5].gameObject.transform.position = new Vector3(18f, -15.5f, 0);
        }
        else if (numberOfPlayers == 7)
        {
            Positions[3].gameObject.transform.position = new Vector3(-21f, -15.5f, 0);
            Positions[4].gameObject.transform.position = new Vector3(-7f, -15.5f, 0);
            Positions[5].gameObject.transform.position = new Vector3(7f, -15.5f, 0);
            Positions[6].gameObject.transform.position = new Vector3(21f, -15.5f, 0);
        }
    }

    IEnumerator UpdateLeaderboard(float waitTime)
    {
        yield return new WaitForSeconds(waitTime+1);
        List<RankingDto.Rank> oldRanks = oldRanking.GetRanking();
        List<RankingDto.Rank> newRanks = newRanking.GetRanking();
        List<int> winnerIds = new();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            bool hasWon = false;
            if (oldRanks[i].Points != newRanks.Find(r => r.PlayerId == oldRanks[i].PlayerId).Points)
            {
                hasWon = true;
                winnerIds.Add(oldRanks[i].PlayerId);
            }
            Positions[i].SetData(oldRanks[i].PlayerSprite, oldRanks[i].Points, hasWon, oldRanks[i].PlayerId);
        }
        yield return new WaitForSeconds(waitTime);
        PS1.gameObject.SetActive(true); PS2.gameObject.SetActive(true); PS3.gameObject.SetActive(true);
        PS1.Play(); PS2.Play(); PS3.Play();
        for (int i = 0; i < numberOfPlayers; i++)
            Positions[i].SetData(newRanks[i].PlayerSprite, newRanks[i].Points, winnerIds.Contains(newRanks[i].PlayerId), newRanks[i].PlayerId);
        UpdateAllRanks(newRanks);
        yield return new WaitForSeconds(waitTime*2);
        GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>().StartAnimationOnRoomEnds();
        yield return new WaitForSeconds(2);
        if (newRanks.FindAll(r => r.Points >= 5 && r.Points >= newRanks.Max(r => r.Points)).Count == 1)
        {
            // GAME ENDS
            throw new System.NotImplementedException("Winner screen");
        } else SceneManager.LoadScene("GameLoader", LoadSceneMode.Single);
    }

    private void UpdateAllRanks(List<RankingDto.Rank> ranks)
    {
        int lastRank = 1;
        for (int i = 0; i < numberOfPlayers; i++, lastRank++)
        {
            if (i == 0)
            {
                Positions[i].SetRankingText(lastRank);
                continue;
            }
            else if (ranks[i].Points == ranks[i-1].Points)
            {
                lastRank--;
                Positions[i].SetRankingText(lastRank);
                continue;
            }
            Positions[i].SetRankingText(lastRank);
        }
    }

}
