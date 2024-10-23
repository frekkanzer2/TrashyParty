using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingSeasonGameManager : GameManager
{

    public GameObject hunter_prefab;
    public Vector3 yhunterpos;
    private List<HunterBehaviour> hunters;
    
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
        hunters.ForEach(h => h.StartGeneration());
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed");
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
        foreach (IPlayer player in players)
        {
            PlatformerPlayer pp = (PlatformerPlayer)player;
            pp.SetJumpLimit(1);
            pp.SetCanJump(true);
            player.IgnoreCollisionsWithOtherPlayers(true);
            player.SetCanWalk(false);
            pp.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 1);
        }
        hunters = new();
        for (int i = 1; i<players.Count+1; i++)
        {
            hunters.Add(Instantiate(hunter_prefab, yhunterpos, Quaternion.identity).GetComponent<HunterBehaviour>());
            yhunterpos.y = yhunterpos.y - 4.75f;
        }
    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
