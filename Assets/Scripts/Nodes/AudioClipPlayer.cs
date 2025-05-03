using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioClipPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip[] audioClips;
    public AudioMixerGroup mixerGroup;
    public float cooldownTime = 1f;

    private AudioSource[] audioSources;
    private bool[] isPlaying;
    private bool isOnCooldown = false;

    private void Start()
    {
        audioSources = new AudioSource[audioClips.Length];
        isPlaying = new bool[audioClips.Length];
    }

    public void PlayClip(int index)
    {
        if (index < 0 || index >= audioClips.Length)
            return;

        if (isPlaying[index])
            return;

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = audioClips[index];
        source.outputAudioMixerGroup = mixerGroup;
        source.Play();

        audioSources[index] = source;
        isPlaying[index] = true;

        StartCoroutine(DestroyAfterPlay(index, source.clip.length));
    }

    private IEnumerator DestroyAfterPlay(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (audioSources[index] != null)
        {
            Destroy(audioSources[index]);
            audioSources[index] = null;
        }
        isPlaying[index] = false;
    }

    public void PlayLoop(int index)
    {
        if (index < 0 || index >= audioClips.Length)
            return;

        if (isPlaying[index])
            return;

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = audioClips[index];
        source.outputAudioMixerGroup = mixerGroup;
        source.loop = true;
        source.Play();

        audioSources[index] = source;
        isPlaying[index] = true;
    }

    public void StopLoop(int index)
    {
        if (index < 0 || index >= audioClips.Length)
            return;

        if (audioSources[index] != null)
        {
            audioSources[index].Stop();
            Destroy(audioSources[index]);
            audioSources[index] = null;
        }
        isPlaying[index] = false;
    }

    public void PlayClipWithCooldown(int index)
    {
        if (!isOnCooldown)
            StartCoroutine(PlayClipWithCooldownCoroutine(index));
    }

    private IEnumerator PlayClipWithCooldownCoroutine(int index)
    {
        isOnCooldown = true;
        PlayClip(index);
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
