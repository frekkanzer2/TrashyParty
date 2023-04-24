using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundsManager {

    enum PlayerSoundType
    {
        Jump,
        Dead,
        Hit,
        Throw,
        EnergyRelease
    }
    void PlayRandomGameSoundtrack();
    void PlayCountdown();
    void PlaySound(AudioClip clip, string tag, float volume);
    void PlayEndGameSoundtrack();
    void PlayPlayerSound(PlayerSoundType soundType);
    void StopAllSounds();
    void StopAllSoundsDelayed(float seconds);

}
