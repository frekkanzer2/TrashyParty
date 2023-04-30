using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroPlatformsManager : MonoBehaviour
{

    List<ElectroPlatformBehaviour> platforms;
    int cycleNumber = 0;

    public void StartManager()
    {
        GameObject[] plats = GameObject.FindGameObjectsWithTag("Finish");
        platforms = new List<ElectroPlatformBehaviour>();
        foreach (GameObject p in plats)
            platforms.Add(p.GetComponent<ElectroPlatformBehaviour>());
        StartCoroutine(Cycle(7));
    }

    IEnumerator Cycle(float time)
    {
        platforms.Shuffle();
        cycleNumber++;
        int pickLimit = platforms.Count / 2 + cycleNumber;
        if (pickLimit > platforms.Count - 1) pickLimit = platforms.Count - 1;
        List<ElectroPlatformBehaviour> picked = new List<ElectroPlatformBehaviour>(platforms);
        List<ElectroPlatformBehaviour> toActivate = new List<ElectroPlatformBehaviour>();
        List<ElectroPlatformBehaviour> toDeactivate = new List<ElectroPlatformBehaviour>();
        for (int i = 0; i < platforms.Count; i++)
        {
            if (i < pickLimit)
                toActivate.Add(picked[i]);
            else
                toDeactivate.Add(picked[i]);
        }
        foreach (ElectroPlatformBehaviour e in toDeactivate)
        {
            e.Execute(false, 0);
        }
        foreach (ElectroPlatformBehaviour e in toActivate)
        {
            e.Execute(true, time);
        }
        yield return new WaitForSeconds(time + 3.5f);
        if (time > 2.5f) time -= 0.5f;
        StartCoroutine(Cycle(time));
    }

}
