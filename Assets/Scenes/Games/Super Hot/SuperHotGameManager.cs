using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHotGameManager : GameManager
{

    public GameObject SpawnPrefab;
    public GameObject FireballPrefab;

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
            ((PlatformerPlayer)p).SetJumpLimit(2);
            p.IgnoreCollisionsWithOtherPlayers(true);
            ((PlatformerPlayer)p).GetHead().GetComponent<Collider2D>().isTrigger = false;
        }
        StartCoroutine(GenerateLevel1(0));
    }

    private void GenerateEffect(Vector3 spawnCoordinates)
    {
        GameObject effect = Instantiate(
            SpawnPrefab,
            spawnCoordinates,
            Quaternion.identity
        );
        Destroy(effect, 1);
    }
    private void GenerateFire(int piecesToCreate, float startingAngle, Vector3 spawnCoordinates)
    {
        float angleVariation = 360f / piecesToCreate;
        for (int i = 0; i < piecesToCreate; i++)
            Instantiate(
                FireballPrefab,
                spawnCoordinates,
                Quaternion.Euler(0, 0, angleVariation * i + startingAngle)
            );
    }

    IEnumerator GenerateLevel1(int iteration, bool diagonal = false)
    {
        iteration++;
        Vector3 spawnPosition = Extensions.Vector3.GenerateWithRandomValues(new Vector2(-38.215f, 38.215f), new Vector2(-21.493f, 21.493f), Vector2.zero);
        yield return new WaitForSeconds(0.5f);
        GenerateEffect(spawnPosition);
        yield return new WaitForSeconds(1f);
        GenerateFire(4, (!diagonal) ? 0 : 45, spawnPosition);
        diagonal = !diagonal;
        yield return new WaitForSeconds(3 - iteration * 0.25f);
        if (iteration == 10 || GameManager.Instance.IsGameEnded()) StartCoroutine(GenerateLevel2(0, diagonal));
        else StartCoroutine(GenerateLevel1(iteration, diagonal));
    }
    IEnumerator GenerateLevel2(int iteration, bool diagonal)
    {
        iteration++;
        Vector3 spawnPosition = Extensions.Vector3.GenerateWithRandomValues(new Vector2(-38.215f, 38.215f), new Vector2(-21.493f, 21.493f), Vector2.zero);
        yield return new WaitForSeconds(0.5f);
        GenerateEffect(spawnPosition);
        yield return new WaitForSeconds(0.85f);
        GenerateFire(6, (!diagonal) ? 0 : 30, spawnPosition);
        diagonal = !diagonal;
        yield return new WaitForSeconds(2 - iteration * 0.15f);
        if (iteration == 10 || GameManager.Instance.IsGameEnded()) StartCoroutine(GenerateLevel3(0, diagonal));
        else StartCoroutine(GenerateLevel2(iteration, diagonal));
    }
    IEnumerator GenerateLevel3(int iteration, bool diagonal)
    {
        iteration++;
        Vector3 spawnPosition = Extensions.Vector3.GenerateWithRandomValues(new Vector2(-38.215f, 38.215f), new Vector2(-21.493f, 21.493f), Vector2.zero);
        yield return new WaitForSeconds(0.3f);
        GenerateEffect(spawnPosition);
        yield return new WaitForSeconds(0.7f);
        GenerateFire(8, (!diagonal) ? 0 : 22.5f, spawnPosition);
        diagonal = !diagonal;
        yield return new WaitForSeconds(2 - iteration * 0.15f);
        if (iteration == 10 || GameManager.Instance.IsGameEnded()) StartCoroutine(GenerateLevel4(0, diagonal));
        else StartCoroutine(GenerateLevel3(iteration, diagonal));
    }
    IEnumerator GenerateLevel4(int iteration, bool diagonal)
    {
        iteration++;
        Vector3 spawnPosition = Extensions.Vector3.GenerateWithRandomValues(new Vector2(-38.215f, 38.215f), new Vector2(-21.493f, 21.493f), Vector2.zero);
        GenerateEffect(spawnPosition);
        yield return new WaitForSeconds(0.5f);
        GenerateFire(10, (!diagonal) ? 0 : 18f, spawnPosition);
        diagonal = !diagonal;
        yield return new WaitForSeconds(2 - iteration * 0.15f);
        if (iteration == 10 || GameManager.Instance.IsGameEnded()) StartCoroutine(GenerateLevelFinal(0, diagonal));
        else StartCoroutine(GenerateLevel4(iteration, diagonal));
    }
    IEnumerator GenerateLevelFinal(int iteration, bool diagonal)
    {
        iteration++;
        Vector3 spawnPosition = Extensions.Vector3.GenerateWithRandomValues(new Vector2(-38.215f, 38.215f), new Vector2(-21.493f, 21.493f), Vector2.zero);
        yield return new WaitForSeconds(1f);
        GenerateEffect(spawnPosition);
        yield return new WaitForSeconds(0.5f);
        int numberOfBullets = 10 + iteration / 10;
        if (numberOfBullets > 18) numberOfBullets = 18;
        GenerateFire(numberOfBullets, (!diagonal) ? 0 : (360f/numberOfBullets) /2f, spawnPosition);
        diagonal = !diagonal;
        StartCoroutine(GenerateLevelFinal(iteration, diagonal));
    }

    public override void RestartMatch()
    {
        throw new System.AccessViolationException("No restart is allowed for Super Hot game");
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

    }

    protected override void UpdateGameSpecificBehaviour()
    {

    }
}
