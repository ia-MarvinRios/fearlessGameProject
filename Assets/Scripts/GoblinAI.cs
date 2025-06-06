using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider), typeof(AudioSource))]
public class GoblinAI : MonoBehaviour
{
    [Header("Goblin AI Settings")]
    [SerializeField] float attackWaitTime = 2.3f;
    [SerializeField] Transform jumpscareRoot;
    [Space(10)]
    [Header("Hints & UI Elements")]
    [SerializeField] GameObject torchHint;
    [Space(10)]
    [Header("Sound Fx")]
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioMixerGroup audioMixerGroup;
    [Space(10)]
    public UnityEvent onGameOver;

    private bool playerOnRange = false;
    private bool isAttacking = false;
    private bool isPlayerLooking = false;
    Animator animator;
    AudioSource audioSource;

    public delegate void GoblinGameOverEvent();
    public static event GoblinGameOverEvent OnGoblinAttack;

    public bool IsPlayerLooking { get => isPlayerLooking; set => isPlayerLooking = value; } 

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = attackSound;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 1f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnRange = true;

            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer(other));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnRange = false;
            isAttacking = false;
            torchHint.SetActive(false);
            StopAllCoroutines();
        }
    }

    IEnumerator AttackPlayer(Collider other)
    {
        isAttacking = true;

        // Wait for the attack time before checking the distance again
        int times = 20;
        float timeFrac = attackWaitTime / times;
        for (int i = 0; i < times; i++)
        {
            torchHint.SetActive(!torchHint.activeSelf);
            OnGoblinAttack?.Invoke();
            yield return new WaitForSeconds(timeFrac);
        }

        //---------------------------------

        if (playerOnRange)
        {
            other.gameObject.GetComponent<FirstPersonController>().enabled = false;
            other.gameObject.GetComponentInChildren<Animator>().enabled = false;
            transform.position = jumpscareRoot.position + new Vector3(0, 0.1f, 0);
            transform.rotation = jumpscareRoot.rotation;
            animator.SetTrigger("Attack");
            audioSource.Play();
            onGameOver?.Invoke();
        }

        //---------------------------------

    }
}
