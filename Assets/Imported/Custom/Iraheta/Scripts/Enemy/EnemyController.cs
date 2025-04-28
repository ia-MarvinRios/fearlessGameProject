using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Controls the Enemy AI */
[RequireComponent(typeof(EnemyController))]
[RequireComponent (typeof(Enemy))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterCombat))]
public class EnemyController : MonoBehaviour {

	public float lookRadius = 10f;	// Detection range for player

	Transform player;	// Reference to the player
	NavMeshAgent agent; // Reference to the NavMeshAgent
	CharacterCombat combat;
	public Animator animator;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		agent = GetComponent<NavMeshAgent>();
		combat = GetComponent<CharacterCombat>();
		animator = GetComponent<Animator>();
		agent.stoppingDistance = 4; 

		combat.playerStats = player.GetComponent<PlayerStats>();

    }
	
	// Update is called once per frame
	void Update () {
		// Distance to the target
		float distance = Vector3.Distance(player.position, transform.position);

		// If inside the lookRadius
		if (distance <= lookRadius)
		{
			// Move towards the target
			agent.SetDestination(player.position);
			animator.SetBool("IsWalking", true);

			// If within attacking distance
			if (distance <= agent.stoppingDistance)
			{
                /*CharacterStats targetStats = target.GetComponent<CharacterStats>();
				if (targetStats != null)
				{*/
                animator.SetBool("IsWalking", false);
                combat.Attack(); //combat.Attack(targetStats);
				//}

				FaceTarget();	// Make sure to face towards the target
			}
		}
	}

	// Rotate to face the target
	void FaceTarget ()
	{
		Vector3 direction = (player.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
	}

	// Show the lookRadius in editor
	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}
}
