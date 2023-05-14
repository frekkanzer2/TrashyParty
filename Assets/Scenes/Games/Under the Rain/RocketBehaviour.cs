using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{

    public GameObject ExplosionPrefab;
    public AudioClip BombExplosionSound;
    public Rigidbody2D rb;

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + Vector3.up * -1 * Time.deltaTime * 8);
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
            if (collision.gameObject.name.ToLower().Contains("deadzone"))
                Destroy(this.gameObject);
    }

}
