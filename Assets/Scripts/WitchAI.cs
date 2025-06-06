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
    [SerializeField] float cooldown = 10f;
    public Transform respawnPos;

    NavMeshAgent agent;
    Animator animator;
    AudioSource aSource;
    AudioSource aSource2;
    bool isOnCooldown = false;

    public delegate void WitchCapturedEvent();
    public delegate void WitchRespawnEvent(Transform respawnPos);
    public static event WitchRespawnEvent OnWitchRespawn;
    public static event WitchCapturedEvent OnWitchCaptured;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponentInChildren<Animator>();
        aSource = gameObject.AddComponent<AudioSource>();
        aSource2 = gameObject.AddComponent<AudioSource>();
        SetUpASources();
    }

    private void OnDisable()
    {
        OnWitchRespawn?.Invoke(respawnPos);
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
            if (!isOnCooldown)
            {
                aSource.Play();
                aSource2.Play();
                isOnCooldown = true;
                StartCoroutine(CooldownRoutine());
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

    IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

}
