using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticStunSphereBehaviour : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(((int)AppSettings.Get("N_PLAYERS") <= 4) ? 10 : 20);
        animator.Play("staticstunsphere_ends");
        Destroy(this.gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
