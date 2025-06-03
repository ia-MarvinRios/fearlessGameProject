using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class WitchAI : MonoBehaviour
{
    public AudioClip scream;
    public AudioClip chasingSound;
    public AudioClip chasingMusic;
    public AudioMixerGroup audioMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    NavMeshAgent agent;
    Animator animator;
    AudioSource aSource;
    AudioSource aSource2;

    public delegate void WitchCapturedEvent();
    public static event WitchCapturedEvent OnWitchCaptured;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponentInChildren<Animator>();
        aSource = gameObject.AddComponent<AudioSource>();
        aSource2 = gameObject.AddComponent<AudioSource>();
        SetUpASources();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            AudioSource.PlayClipAtPoint(scream, transform.position, 1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.destination = other.transform.position;
            animator.SetFloat("Speed", agent.velocity.magnitude);
            if (!aSource.isPlaying)
            {
                aSource.Play();
            }
            if (!aSource2.isPlaying)
            {
                aSource2.volume = 0.5f;
                aSource2.Play();
            }

            if ((other.transform.position - transform.position).magnitude < 2f)
            {
                animator.SetBool("PlayerCaught", true);
                agent.velocity = Vector3.zero;
                aSource.Stop();
                aSource2.Stop();
                OnWitchCaptured?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.ResetPath();
            animator.SetFloat("Speed", 0f);
            aSource.Stop();
            StartCoroutine(StopChasingMusic());

            StopCoroutine(StopChasingMusic());
        }
    }

    void SetUpASources()
    {
        // chasing breath sound
        aSource.clip = chasingSound;
        aSource.loop = true;
        aSource.outputAudioMixerGroup = audioMixerGroup;
        aSource.volume = 1f;
        aSource.spatialBlend = 1f;

        // chasing music
        aSource2.clip = chasingMusic;
        aSource2.loop = true;
        aSource2.outputAudioMixerGroup = audioMixerGroup;
        aSource2.volume = 0.5f;
    }

    IEnumerator StopChasingMusic()
    {
        float duration = 1f;  // Duración del fade out en segundos
        float time = 0f;
        float startVolume = aSource2.volume;  // Para que funcione aunque el volumen cambie

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            aSource2.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        aSource2.volume = 0f; // Asegura que volumen quede a 0
        aSource2.Stop();
    }

}
