using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBallGenerator : MonoBehaviour
{
    public GameObject Prefab;
    public Animator Lateral, Central;
    public float BallSpeed;
    private bool active = false;

    public void StartGeneration()
    {
        active = true;
        StartCoroutine(StartGenerationWithTimes());
    }
    IEnumerator StartGenerationWithTimes()
    {
        StartCoroutine(GenLevel1(5));
        yield return new WaitForSeconds(15);
        StartCoroutine(GenLevel2(6.5f));
        yield return new WaitForSeconds(15);
        StartCoroutine(GenLevel3(10));
        yield return new WaitForSeconds(15);
        StartCoroutine(GenLevel4(6));
        yield return new WaitForSeconds(15);
        StartCoroutine(GenLevel5(6));
        yield return new WaitForSeconds(30);
        StartCoroutine(GenLevel6(5));
        MoveLateralPlatforms();
        yield return new WaitForSeconds(30);
        MoveCentralPlatform();
    }
    public void EndGeneration()
    {
        active = false;
    }
    public bool IsGenerationActive()
    {
        return active;
    }
    private void SpawnAndThrowLavaBall(Vector2 Spawnpoint, Vector2 Destination)
    {
        GameObject ball = Instantiate(Prefab, Spawnpoint, Quaternion.identity);
        ball.GetComponent<LavaBallBehaviour>().Initialize(Destination, BallSpeed);
    }

    #region Generations

    private void MoveLateralPlatforms()
    {
        Lateral.Play("Down");
    }

    private void MoveCentralPlatform()
    {
        Central.Play("Down");
    }

    IEnumerator GenLevel1(float spawnSecondsSpeed)
    {
        yield return new WaitForSeconds(spawnSecondsSpeed);
        SpawnAndThrowLavaBall(new Vector2(39.37f, -5.14f), new Vector2(-1, 0));
        StartCoroutine(GenLevel1((spawnSecondsSpeed > 3.5f) ? spawnSecondsSpeed - 0.25f : spawnSecondsSpeed));
    }
    IEnumerator GenLevel2(float spawnSecondsSpeed)
    {
        yield return new WaitForSeconds(spawnSecondsSpeed);
        SpawnAndThrowLavaBall(new Vector2(39.37f, -3.26f), new Vector2(-1, 0f));
        StartCoroutine(GenLevel2((spawnSecondsSpeed > 4f) ? spawnSecondsSpeed - 0.25f : spawnSecondsSpeed));
    }
    IEnumerator GenLevel3(float spawnSecondsSpeed)
    {
        yield return new WaitForSeconds(spawnSecondsSpeed);
        SpawnAndThrowLavaBall(new Vector2(39.37f, 22.57f), new Vector2(-0.6f, -0.5f));
        SpawnAndThrowLavaBall(new Vector2(-39.37f, 22.57f), new Vector2(0.6f, -0.5f));
        StartCoroutine(GenLevel3((spawnSecondsSpeed > 5f) ? spawnSecondsSpeed - 1f : spawnSecondsSpeed));
    }
    IEnumerator GenLevel4(float spawnSecondsSpeed)
    {
        yield return new WaitForSeconds(spawnSecondsSpeed);
        SpawnAndThrowLavaBall(new Vector2(-14f, -22.6f), new Vector2(0, 1));
        SpawnAndThrowLavaBall(new Vector2(14f, -22.6f), new Vector2(0, 1));
        yield return new WaitForSeconds(1.5f);
        SpawnAndThrowLavaBall(new Vector2(-8f, -22.6f), new Vector2(0, 1));
        SpawnAndThrowLavaBall(new Vector2(8f, -22.6f), new Vector2(0, 1));
        StartCoroutine(GenLevel4((spawnSecondsSpeed > 3f) ? spawnSecondsSpeed - 0.1f : spawnSecondsSpeed));
    }
    IEnumerator GenLevel5(float spawnSecondsSpeed)
    {
        yield return new WaitForSeconds(spawnSecondsSpeed);
        SpawnAndThrowLavaBall(new Vector2(-11f, -22.6f), new Vector2(0, 1));
        SpawnAndThrowLavaBall(new Vector2(11f, -22.6f), new Vector2(0, 1));
        yield return new WaitForSeconds(2f);
        SpawnAndThrowLavaBall(new Vector2(-5f, -22.6f), new Vector2(0, 1));
        SpawnAndThrowLavaBall(new Vector2(5f, -22.6f), new Vector2(0, 1));
        StartCoroutine(GenLevel5((spawnSecondsSpeed > 3f) ? spawnSecondsSpeed - 0.1f : spawnSecondsSpeed));
    }

    IEnumerator GenLevel6(float spawnSecondsSpeed)
    {
        yield return new WaitForSeconds(spawnSecondsSpeed);
        SpawnAndThrowLavaBall(new Vector2(39.37f, -8.92f), new Vector2(-1, 0));
        StartCoroutine(GenLevel6((spawnSecondsSpeed > 2f) ? spawnSecondsSpeed - 0.25f : spawnSecondsSpeed));
    }
    #endregion
}
