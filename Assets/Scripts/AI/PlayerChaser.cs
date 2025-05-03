using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class PlayerChaser : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private Vector3 radiusCenter = Vector3.zero;
    [SerializeField] private string chasingAnimation = "Name";

    [Header("Events")]
    public UnityEvent onPlayerDetected;
    public UnityEvent onPlayerLost;

    private NavMeshAgent agent;

    Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();   

        trigger.isTrigger = true;
        trigger.radius = detectionRadius;
        trigger.center = new Vector3(radiusCenter.x, radiusCenter.y, radiusCenter.z);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is within the detection range
            Debug.Log("Player detected!");

            RotateToTarget(other);
            onPlayerDetected?.Invoke();

            animator.SetBool("PlayerDetected", true);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(chasingAnimation))
            {
                agent.destination = other.transform.position;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is out of the detection range
            Debug.Log("Player lost!");
            onPlayerLost?.Invoke();

            agent.destination = transform.position; // Stop moving
            animator.SetBool("PlayerDetected", false);
        }
    }

    private void RotateToTarget(Collider other)
    {
        Quaternion lookAt = new Quaternion(0, Quaternion.LookRotation(other.transform.position - transform.position).y,
                                                0, Quaternion.LookRotation(other.transform.position - transform.position).w);
        transform.rotation = lookAt;
    }
}
