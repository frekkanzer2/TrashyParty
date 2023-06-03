using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HottieBlockBehaviour : MonoBehaviour
{
    public Animator animator;
    public void Disappear()
    {
        animator.Play("Disappear");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.transform.position.y > this.gameObject.transform.position.y && !GameManager.Instance.IsGameEnded())
        {
            collision.gameObject.GetComponent<IPlayer>().ApplyForce(new Vector2(0, Random.Range(50, 60)), 0.25f);
            Disappear();
            Destroy(this.gameObject, 3f);
        }
    }
}
