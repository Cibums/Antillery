using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public static float masterVolume = 1;

    public AudioSource battleMusic;
    public AudioSource buyingMusic;

    public List<AudioClip> clips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void UpdateMusic()
    {
        battleMusic.volume = EnemyController.waveIsOngoing ? masterVolume : 0;
        buyingMusic.volume = EnemyController.waveIsOngoing ? 0 : masterVolume;
    }

    public static void PlaySound(AudioClip clip, float volume = 1)
    {
        instance.AddAndPlaySound(clip, volume);
    }

    public static void PlaySound(int clip, float volume = 1)
    {
        instance.AddAndPlaySound(clip, volume);
    }

    public void AddAndPlaySound(int clip, float volume = 1)
    {
        AddAndPlaySound(clips[clip], volume);
    }

    public void AddAndPlaySound(AudioClip clip, float volume = 1)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.loop = false;
        source.volume = masterVolume * volume;
        source.clip = clip;
        source.Play();
        StartCoroutine(DestroyWhenDonePlaying(source));
    }

    private IEnumerator DestroyWhenDonePlaying(AudioSource source)
    {
        yield return new WaitUntil(() => source.isPlaying == false);

        source.Stop();
        Destroy(source);
    }
}
