using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{

    public Sprite NormalSprite, ActiveSprite;
    public GameObject ExplosionPrefab;
    public SpriteRenderer renderer;
    private float EachSpriteSpeed = 0, expDim = 0;
    public AudioClip BombExplosionSound;

    public void Initialize(float loadingSpeed, float explosionDimension)
    {
        EachSpriteSpeed = loadingSpeed / 6f;
        expDim = explosionDimension;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        renderer.sprite = NormalSprite;
        yield return new WaitForSeconds(EachSpriteSpeed);
        renderer.sprite = ActiveSprite;
        yield return new WaitForSeconds(EachSpriteSpeed);
        renderer.sprite = NormalSprite;
        yield return new WaitForSeconds(EachSpriteSpeed);
        renderer.sprite = ActiveSprite;
        yield return new WaitForSeconds(EachSpriteSpeed);
        renderer.sprite = NormalSprite;
        yield return new WaitForSeconds(EachSpriteSpeed);
        renderer.sprite = ActiveSprite;
        yield return new WaitForSeconds(EachSpriteSpeed);
        GameObject exp = Instantiate(ExplosionPrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1.22f, this.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 45)));
        exp.transform.localScale = new Vector3(expDim, expDim, 1);
        if (!GameManager.Instance.IsGameEnded()) SoundsManager.Instance.PlaySound(BombExplosionSound, Constants.LEVEL_SPECIFIC_SOUND_TAG, 1);
        Destroy(exp, 3);
        Destroy(this.gameObject);
    }

}
