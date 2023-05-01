using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPawBehaviour : MonoBehaviour
{

    private float Speed = 10;
    private float Time = 10;

    public void StartMovement()
    {
        StartCoroutine(Move(Speed, Time));
    }

    IEnumerator Move(float speed, float time)
    {
        this.GetComponent<Rigidbody2D>().velocity = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized * speed;
        yield return new WaitForSeconds(time);
        StartCoroutine(Move(speed + 3, (time > 5) ? time - 1 : time));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GameManager.Instance.IsGameEnded())
            collision.gameObject.GetComponent<IPlayer>().OnDeath();
    }

}
