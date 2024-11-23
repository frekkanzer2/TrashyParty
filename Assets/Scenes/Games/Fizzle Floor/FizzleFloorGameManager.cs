using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FizzleFloorGameManager : GameManager
{

    public GameObject BlockPrefab;
    private List<GameObject>[] BlockLines;

    public override void OnPlayerDies()
    {
        base.OnPlayerDies();
    }

    public override void OnPlayerSpawns()
    {

    }

    public override void OnPreparationEndsGameSpecific()
    {
        SoundManager.PlayRandomGameSoundtrack();
        foreach (IPlayer p in players)
        {
            p.IgnoreCollisionsWithOtherPlayers(true);
            ((PlatformerPlayer)p).GetHead().GetComponent<Collider2D>().isTrigger = false;
        }
        StartCoroutine(StartDisappearance());
    }

    IEnumerator StartDisappearance()
    {
        StartCoroutine(Disappearance(2, 1));
        yield return new WaitForSeconds(6);
        StartCoroutine(Disappearance(1.6f, 2));
        yield return new WaitForSeconds(6);
        StartCoroutine(Disappearance(1.6f, 3));
    }

    IEnumerator Disappearance(float time, int level)
    {
        yield return new WaitForSeconds(time);
        if (!GameManager.Instance.IsGameEnded())
        {
            GameObject block = null;
            bool canRemove = true;
            try
            {
                block = BlockLines[level - 1].GetRandomAndRemove();
            } catch (System.ArgumentOutOfRangeException)
            {
                canRemove = false;
            }
            if (canRemove)
            {
                block.GetComponent<DisappearanceBlockBehaviour>().Disappear();
                Destroy(block, 3);
                StartCoroutine(Disappearance((time > 1) ? time - 0.1f : time, level));
            }
        }
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Detonation Bird game");
    }

    protected override void FixedUpdateGameSpecificBehaviour()
    {

    }

    protected override List<TeamDto> GenerateTeamsCriteria(int numberOfPlayers)
    {
        List<TeamDto> teams = new List<TeamDto>();
        for (int i = 0; i < numberOfPlayers; i++)
            teams.Add(new()
            {
                Id = i+1
            });
        InitializeTeamMatchVictories(teams);
        SetMatchesVictoryLimit(1);
        return teams;
    }

    protected override void OnRoomStarts()
    {
        Vector3 firstLineCoord = new Vector3(-28.8f, 7f, 0);
        Vector3 secondLineCoord = new Vector3(-28.8f, -5.5f, 0);
        Vector3 thirdLineCoord = new Vector3(-28.8f, -18f, 0);
        BlockLines = new List<GameObject>[3];
        BlockLines[0] = new List<GameObject>();
        BlockLines[1] = new List<GameObject>();
        BlockLines[2] = new List<GameObject>();
        for (int i = 0; i < 21; i++)
        {
            BlockLines[0].Add(Instantiate(BlockPrefab, firstLineCoord, Quaternion.identity));
            BlockLines[1].Add(Instantiate(BlockPrefab, secondLineCoord, Quaternion.identity));
            BlockLines[2].Add(Instantiate(BlockPrefab, thirdLineCoord, Quaternion.identity));
            firstLineCoord.x += 2.88f;
            secondLineCoord.x += 2.88f;
            thirdLineCoord.x += 2.88f;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
    
}
