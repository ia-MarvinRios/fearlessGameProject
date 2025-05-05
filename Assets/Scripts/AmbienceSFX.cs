using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AmbienceSFX : MonoBehaviour
{
    [SerializeField] private AudioClip morningForestAmbience;
    [SerializeField] private AudioMixerGroup mixerGroup;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = morningForestAmbience;
        audioSource.loop = true;
        audioSource.outputAudioMixerGroup = mixerGroup;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}