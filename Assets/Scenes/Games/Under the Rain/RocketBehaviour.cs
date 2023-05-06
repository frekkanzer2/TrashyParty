using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{

    public GameObject ExplosionPrefab;
    public AudioClip BombExplosionSound;
    public Rigidbody2D rb;
    bool m_oneTime = false;

    private void FixedUpdate()
    {
        if (!m_oneTime)
        {
            m_oneTime = true;
            rb.AddForce(new Vector2(0, -400), ForceMode2D.Force);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Constants.LAYER_GROUND || collision.gameObject.CompareTag("Player"))
        {
            GameObject exp = Instantiate(ExplosionPrefab, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 45)));
            if (!GameManager.Instance.IsGameEnded()) SoundsManager.Instance.PlaySound(BombExplosionSound, Constants.LEVEL_SPECIFIC_SOUND_TAG, 1);
            Destroy(exp, 3);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Constants.LAYER_DEADZONE)
            Destroy(this.gameObject);
    }

}
