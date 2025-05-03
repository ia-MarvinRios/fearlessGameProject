using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioClipPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip[] audioClips;
    public AudioMixerGroup mixerGroup;
    public float cooldownTime = 1f;

    private AudioSource audioSource;
    private bool isPlaying = false;
    private bool isOnCooldown = false;

    public void PlayClip(int index)
    {
        if (!isPlaying)
        {
            isPlaying = true;

            for (int i = 0; i < audioClips.Length; i++)
            {
                if (i == index)
                {
                    audioSource = transform.AddComponent<AudioSource>();

                    audioSource.clip = audioClips[i];
                    audioSource.outputAudioMixerGroup = mixerGroup;

                    audioSource.Play();
                }
            }

            isPlaying = false;
        }
    }

    public void PlayClipWithCooldown(int index)
    {
        StartCoroutine(PlayClipWithCooldownCoroutine(index));
    }

    private IEnumerator PlayClipWithCooldownCoroutine(int index)
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;
            PlayClip(index);
            yield return new WaitForSeconds(cooldownTime);
            isOnCooldown = false;
        }
    }
}