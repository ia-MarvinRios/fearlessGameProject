using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AmbienceSFX : MonoBehaviour
{
    public AudioClip morningForestAmbience;
    public AudioClip nightForestAmbience;
    public AudioMixerGroup ambienceMixerGroup;

    private AudioSource audioSource;

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
}
