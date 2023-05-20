using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggToPickBehaviour : MonoBehaviour
{

    private void Start()
    {
        Vector2 generatedForce = Vector2.zero;
        generatedForce = new Vector2(Random.Range(-1f, 1f)*5, Random.Range(0.5f, 1f)*5);
        this.GetComponent<Rigidbody2D>().AddForce(generatedForce, ForceMode2D.Impulse);
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(3);
        this.GetComponent<Animator>().Play("hidingegg");
        yield return new WaitForSeconds(5);
        ((EggsRushGameManager)GameManager.Instance).OnEggCollided(this.gameObject);
        Destroy(this.gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Constants.LAYER_DEADZONE)
        {
            ((EggsRushGameManager)GameManager.Instance).OnEggCollided(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
