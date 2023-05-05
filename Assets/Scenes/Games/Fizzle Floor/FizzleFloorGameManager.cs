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