using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeBehaviour : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.IsGameEnded())
            collision.gameObject.GetComponent<IPlayer>().OnDeath();
    }
    private void FixedUpdate()
    {
        this.transform.position = new Vector3(this.transform.position.x - 0.25f, this.transform.position.y, this.transform.position.z);
        if (this.transform.position.x < -50) Destroy(this.gameObject);
    }
}
