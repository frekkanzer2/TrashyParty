using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTagBehaviour : MonoBehaviour
{
    public AudioClip SoundEachSecond;
    public AudioClip ExplosionSound;
    public GameObject ExplosionPrefab;
    private bool attachable = true;
    private void Start()
    {
        StartCoroutine(Countdown(10));
    }
    IEnumerator Countdown(int s)
    {
        SoundsManager.Instance.PlaySound(SoundEachSecond, Constants.LEVEL_SPECIFIC_SOUND_TAG, 1);
        yield return new WaitForSeconds(1);
        s--;
        if (s == 0) Explode();
        else StartCoroutine(Countdown(s));
    }
    private void Explode() {
        GameObject exp = Instantiate(ExplosionPrefab, this.transform.position, Quaternion.Euler(0, 0, 45));
        SoundsManager.Instance.PlaySound(ExplosionSound, Constants.LEVEL_SPECIFIC_SOUND_TAG, 0.8f);
        ((BombTagGameManager)GameManager.Instance).OnBombDestroyed();
        Destroy(exp, 3);
        Destroy(this.gameObject);
    }
    public bool CanBeAttached() => this.attachable;
    public void OnAssigned() => StartCoroutine(AssignedCoroutine());

    IEnumerator AssignedCoroutine()
    {
        this.attachable = false;
        yield return new WaitForSeconds(1);
        this.attachable = true;
    }
}
