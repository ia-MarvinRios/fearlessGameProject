using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AmbienceSFX : MonoBehaviour
{
    public AudioClip morningForestAmbience;
    public AudioClip nightForestAmbience;
    public AudioMixerGroup ambienceMixerGroup;

    [Header("Music")]
    public AudioClip[] tracks;
    public float musicVolume = 0.5f;
    public float replayCooldown = 5f;

    private AudioSource audioSource;
    private AudioSource audioSource2;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SetUpAudioClip(morningForestAmbience, ambienceMixerGroup);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void SetUpAudioClip(AudioClip clip, AudioMixerGroup mxGroup)
    {
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.outputAudioMixerGroup = mxGroup;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void SetClipAndPlay(AudioClip newClip)
    {
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();
    }

    public IEnumerator MainMenuMusic()
    {
        audioSource2 = gameObject.AddComponent<AudioSource>();
        while (true)
        {
            audioSource2.clip = tracks[Random.Range(0, tracks.Length)];
            audioSource2.volume = musicVolume;
            audioSource2.outputAudioMixerGroup = ambienceMixerGroup;
            audioSource2.Play();
            yield return new WaitForSeconds(audioSource2.clip.length + replayCooldown);
        }
    }
}