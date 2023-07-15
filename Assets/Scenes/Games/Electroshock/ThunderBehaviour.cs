using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBehaviour : MonoBehaviour
{

    public List<Sprite> sprites;
    public new SpriteRenderer renderer;

    void Start()
    {
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        renderer.sprite = sprites[0];
        yield return new WaitForSeconds(1);
        renderer.sprite = sprites[1];
        yield return new WaitForSeconds(1);
        renderer.sprite = sprites[2];
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

}
