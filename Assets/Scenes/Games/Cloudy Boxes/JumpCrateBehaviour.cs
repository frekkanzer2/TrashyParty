using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCrateBehaviour : MonoBehaviour
{

    public List<Sprite> numbers;
    private int actualNumber;

    private void Start()
    {
        this.actualNumber = Random.Range(5, 11);
        if (this.actualNumber > 10) actualNumber = 10;
        DisplayNumber(actualNumber);
    }

    private void DisplayNumber(int number)
    {
        if (number == 0) return;
        Sprite toDisplay = numbers[number - 1];
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = toDisplay;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IPlayer>().ApplyForce(new Vector2(0, Random.Range(75, 95)));
            if (actualNumber > 0)
            {
                actualNumber--;
                DisplayNumber(actualNumber);
            }
            if (actualNumber == 0)
            {
                Destroy(this.gameObject, 0.1f);
            }
        }
    }
}
