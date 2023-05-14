using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballWallableMovement : MonoBehaviour
{
    public bool CanBeDestroyed = false;
    private void Start()
    {
        StartCoroutine(Wait());
    }
    private void FixedUpdate()
    {
        this.transform.Translate(new(-0.25f, 0f, 0f));
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        CanBeDestroyed = true;
    }
}
