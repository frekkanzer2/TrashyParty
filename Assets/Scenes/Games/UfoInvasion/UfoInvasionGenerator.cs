using System.Collections;
using UnityEngine;

public class UfoInvasionGenerator : MonoBehaviour
{
    public GameObject PrefabBeige, PrefabBlue, PrefabGreen, PrefabPink, PrefabYellow;
    private bool active = false;

    public void StartGeneration()
    {
        if (active) return;
        active = true;
        StartCoroutine(StartGenerationWithTimes());
    }
    IEnumerator StartGenerationWithTimes()
    {
        StartCoroutine(GenLevel1(8));
        yield return new WaitForSeconds(24);
        StartCoroutine(GenLevel2(16));
        yield return new WaitForSeconds(32);
        StartCoroutine(GenLevel3(10));
        yield return new WaitForSeconds(20);
        StartCoroutine(GenLevel4(8));
        yield return new WaitForSeconds(16);
        StartCoroutine(GenLevel5(10));
    }
    public void EndGeneration() => active = false;
    public bool IsGenerationActive() => active;
    private GameObject SpawnUfo(GameObject Prefab, Vector2 Spawnpoint)
    {
        return Instantiate(Prefab, Spawnpoint, Quaternion.identity);
    }

    #region Generations

    IEnumerator GenLevel1(float spawnSecondsSpeed) // BEIGE
    {
        GameObject ufo = SpawnUfo(PrefabBeige, new Vector2(44f, Random.Range(-18.1f, 18.1f)));
        Destroy(ufo, 10);
        yield return new WaitForSeconds(spawnSecondsSpeed);
        StartCoroutine(GenLevel1((spawnSecondsSpeed > 4f) ? spawnSecondsSpeed - 0.05f : spawnSecondsSpeed));
    }

    IEnumerator GenLevel2(float spawnSecondsSpeed) // BLUE
    {
        GameObject ufo = SpawnUfo(PrefabBlue, new Vector2(44f, 0));
        yield return new WaitForSeconds(spawnSecondsSpeed);
        StartCoroutine(GenLevel2((spawnSecondsSpeed > 10f) ? spawnSecondsSpeed - 1f : spawnSecondsSpeed));
    }

    IEnumerator GenLevel3(float spawnSecondsSpeed) // YELLOW
    {
        GameObject ufo = SpawnUfo(PrefabYellow, new Vector2(44f, Random.Range(-18.1f, 18.1f)));
        Destroy(ufo, 20);
        yield return new WaitForSeconds(spawnSecondsSpeed);
        StartCoroutine(GenLevel3((spawnSecondsSpeed > 6f) ? spawnSecondsSpeed - 0.125f : spawnSecondsSpeed));
    }

    IEnumerator GenLevel4(float spawnSecondsSpeed) // GREEN
    {
        GameObject ufo = SpawnUfo(PrefabGreen, new Vector2(44f, 0));
        yield return new WaitForSeconds(spawnSecondsSpeed);
        StartCoroutine(GenLevel4((spawnSecondsSpeed > 3f) ? spawnSecondsSpeed - 0.25f : spawnSecondsSpeed));
    }

    IEnumerator GenLevel5(float spawnSecondsSpeed) // PINK
    {
        GameObject ufo = SpawnUfo(PrefabPink, new Vector2(44f, Random.Range(-18.1f, 18.1f)));
        Destroy(ufo, 20);
        yield return new WaitForSeconds(spawnSecondsSpeed);
        StartCoroutine(GenLevel5((spawnSecondsSpeed > 5f) ? spawnSecondsSpeed - 0.125f : spawnSecondsSpeed));
    }

    #endregion
}
