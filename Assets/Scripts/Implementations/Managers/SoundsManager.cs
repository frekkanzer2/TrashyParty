using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : Singleton<SoundsManager>, ISoundsManager
{

    [SerializeField]
    private List<AudioClip> GameSoundtracks;
    [SerializeField]
    private AudioClip EndGameSoundtrack;
    [SerializeField]
    private AudioClip WinGameSoundtrack;
    [SerializeField]
    private AudioClip PresentationStartSound;
    [SerializeField]
    private AudioClip CountdownSound;
    [SerializeField]
    private AudioClip PlayerJump;
    [SerializeField]
    private AudioClip PlayerHit;
    [SerializeField]
    private AudioClip PlayerDeath;
    [SerializeField]
    private AudioClip PlayerThrow;
    [SerializeField]
    private AudioClip PlayerEnergyRelease;

    private List<RegisteredAudioSourceDto> AudioSources;

    private void Awake()
    {
        InitializeSingleton(this);
    }

    public void PlayCountdown() => StartCoroutine(ExecutorPlayCountdown());

    private IEnumerator ExecutorPlayCountdown()
    {
        AudioSource source = PickAudioSource("Countdown", false);
        InitializeAudioSource(source, PresentationStartSound, false, 1);
        yield return new WaitForSeconds(3);
        InitializeAudioSource(source, CountdownSound, false, 1);
        yield return new WaitForSeconds(1);
        InitializeAudioSource(source, CountdownSound, false, 1);
        yield return new WaitForSeconds(1);
        InitializeAudioSource(source, CountdownSound, false, 1);
    }

    public void PlayEndGameSoundtrack() => StartCoroutine(ExecutorEndGameSoundtrack());

    private IEnumerator ExecutorEndGameSoundtrack()
    {
        AudioSource source = PickAudioSource("EndGame", false);
        InitializeAudioSource(source, WinGameSoundtrack, false, 0.8f);
        yield return new WaitForSeconds(4);
        InitializeAudioSource(source, EndGameSoundtrack, false, 0.5f);
    }

    public void PlayPlayerSound(ISoundsManager.PlayerSoundType soundType)
    {
        AudioClip toExecute = null;
        switch (soundType)
        {
            case ISoundsManager.PlayerSoundType.Jump:
                toExecute = PlayerJump;
                break;
            case ISoundsManager.PlayerSoundType.Hit:
                toExecute = PlayerHit;
                break;
            case ISoundsManager.PlayerSoundType.Dead:
                toExecute = PlayerDeath;
                break;
            case ISoundsManager.PlayerSoundType.Throw:
                toExecute = PlayerThrow;
                break;
            case ISoundsManager.PlayerSoundType.EnergyRelease:
                toExecute = PlayerEnergyRelease;
                break;
        }
        InitializeAudioSource(PickAudioSource("Player", true), toExecute, false, 1);
    }

    public void PlayRandomGameSoundtrack()
    {
        AudioClip soundtrack = GameSoundtracks.GetRandom();
        AudioSource source = PickAudioSource("Soundtrack", false);
        InitializeAudioSource(source, soundtrack, true, 0.4f);
    }

    private void Start()
    {
        AudioSources = new List<RegisteredAudioSourceDto>();
        foreach (AudioSource a in GetComponents<AudioSource>())
            AudioSources.Add(new RegisteredAudioSourceDto()
            {
                Tag = null,
                Source = a
            });
    }

    private AudioSource? PickAudioSource(string tag, bool shouldBeFree)
    {
        AudioSource source = _PickExistingAudioSource(tag, shouldBeFree);
        if (source == null)
            try
            {
                source = _PickNewAudioSource(tag);
            }
            catch (System.NullReferenceException)
            {
                throw new System.NullReferenceException("No Audio Sources available!");
            }
        return source;
    }

    private void InitializeAudioSource(AudioSource source, AudioClip clip, bool shouldLoop, float volume)
    {
        if (volume < 0 || volume > 1) throw new System.ArgumentException("Volume value must be in the range (0, 1), with 0 and 1 are included values");
        if (clip is null) throw new System.ArgumentException("AudioClip provided is null");
        if (source is null) throw new System.ArgumentException("AudioSource provided is null");
        source.Stop();
        source.clip = clip;
        source.loop = shouldLoop;
        source.volume = volume;
        source.Play();
    }

    private AudioSource? _PickExistingAudioSource(string tag, bool shouldBeFree)
    {
        try
        {
            RegisteredAudioSourceDto source = null;
            if (shouldBeFree)
                source = AudioSources.Find(audioSource => audioSource.Source.isPlaying == false && audioSource.Tag.Equals(tag));
            else source = AudioSources.Find(audioSource => audioSource.Tag.Equals(tag));
            if (source == null) return null;
            return source.Source;
        } catch (System.NullReferenceException)
        {
            return null;
        }
    }

    private AudioSource? _PickNewAudioSource(string tag)
    {
        RegisteredAudioSourceDto source = AudioSources.Find(audioSource => audioSource.Source.isPlaying == false && audioSource.Tag is null);
        if (source == null) return null;
        source.Tag = tag;
        return source.Source;
    }

    public void StopAllSounds()
    {
        foreach (RegisteredAudioSourceDto a in AudioSources)
            a.Source.Stop();
    }
    public void StopAllSoundsDelayed(float seconds) => StartCoroutine(StopAllSoundsDelayedExecutor(seconds));
    private IEnumerator StopAllSoundsDelayedExecutor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopAllSounds();
    }

    public void PlaySound(AudioClip clip, string tag, float volume)
    {
        AudioSource source = PickAudioSource(tag, true);
        InitializeAudioSource(source, clip, false, volume);
    }
}
