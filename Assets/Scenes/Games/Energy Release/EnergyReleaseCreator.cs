using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyReleaseCreator : MonoBehaviour
{
    public IPlayer Summoner;
    private void Start()
    {
        Destroy(this.gameObject, 5);
    }
}
