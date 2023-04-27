using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBehaviour : MonoBehaviour
{

    private float speed;
    private bool isStarted = false;

    public void StartPath()
    {
        isStarted = true;
        speed = 0.06f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStarted) return;
        if (!GameManager.Instance.IsGameEnded()) this.transform.Translate(Vector3.up * -1 * speed);
        if (speed < 2.5f)
            speed += Time.deltaTime / 100f;
        else if (speed < 3.5f)
            speed += Time.deltaTime / 250f;
        else if (speed < 5f)
            speed += Time.deltaTime / 500f;
    }
}
