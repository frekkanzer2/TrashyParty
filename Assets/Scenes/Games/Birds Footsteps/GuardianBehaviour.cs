using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianBehaviour : MonoBehaviour
{

    private new SpriteRenderer renderer;
    private int iteration = 0;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public Sprite normalState, angryState;

    public AudioClip sound;

    public void StartWatch()
    {
        StartCoroutine(StartCycle());
    }

    IEnumerator StartCycle()
    {
        this.renderer.sprite = normalState;
        IsWatching = false;
        iteration = 0;
        renderer.flipX = false;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(WatchStep());
    }

    IEnumerator WatchStep()
    {
        iteration++;
        if (!GameManager.Instance.IsGameEnded()) SoundsManager.Instance.PlaySound(sound, Constants.LEVEL_SPECIFIC_SOUND_TAG, 1);
        yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        if (iteration < 3) StartCoroutine(WatchStep());
        else StartCoroutine(Watch());
    }

    IEnumerator Watch()
    {
        this.renderer.sprite = angryState;
        IsWatching = true;
        yield return new WaitForSeconds(Random.Range(1.5f, 3f));
        StartCoroutine(StartCycle());
    }

    public static bool IsWatching = false;

}
