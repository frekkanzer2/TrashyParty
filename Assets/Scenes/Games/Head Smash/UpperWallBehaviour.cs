using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperWallBehaviour : MonoBehaviour
{

    private Animator WallKillAnimator;

    private IEnumerator StartCounterToWallDown()
    {
        yield return new WaitForSeconds(180);
        WallKillAnimator.Play("DOWN");
    }

    // Start is called before the first frame update
    void Start()
    {
        WallKillAnimator = this.GetComponent<Animator>();
        StartCoroutine(StartCounterToWallDown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
