using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDistanceComparer : IComparer<IPlayer>
{

    private Transform startPoint;

    public PlayerDistanceComparer(Transform referenceTransform)
    {
        this.startPoint = referenceTransform;
    }

    public int Compare(IPlayer a, IPlayer b)
    {
        GameObject goA, goB;
        goA = (a as MonoBehaviour).gameObject;
        goB = (b as MonoBehaviour).gameObject;
        float distanzaA = Vector3.Distance(startPoint.position, goA.transform.position);
        float distanzaB = Vector3.Distance(startPoint.position, goB.transform.position);
        return distanzaA.CompareTo(distanzaB);
    }

}
