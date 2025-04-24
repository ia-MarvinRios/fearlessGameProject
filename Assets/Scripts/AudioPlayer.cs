using UnityEngine;
using UnityEngine.Audio;

//[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [Header("Audio Player")]
    [SerializeField] AudioClip ambienceLoopMorning;
    [SerializeField, Range(0, 1)] float aLMorningVolume;
    [SerializeField] AudioClip ambienceLoopNight;
    [SerializeField, Range(0, 1)] float aLNightolume;

    public AudioMixerGroup mixer;


    // Variables
    private AudioSource audioSource;

    private void Awake()
    {
        if (ambienceLoopMorning != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = ambienceLoopMorning;
            audioSource.loop = true;
            audioSource.volume = aLMorningVolume;
            audioSource.outputAudioMixerGroup = mixer;
        }
    }

    private void FixedUpdate()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
