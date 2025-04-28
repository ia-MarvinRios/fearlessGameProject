using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour {

	
	private float attackCooldown = 0f;
	public float DelayAntesDeAtacar = 2.5f;
    public float DelayEntreAtaques = 1f;

	[Space]
	public Transform _pointWeapon;
	public float _attackRange = 1f;
	public LayerMask _playerLayer;
    
	public event System.Action OnAttack;
	EnemyController enemyController;
	public PlayerStats playerStats;

	CharacterStats myStats;

	public UnityEvent _OnAttack;

    void Start ()
	{
		myStats = GetComponent<EnemyStats>();
        enemyController = GetComponent<EnemyController>();

		if (_pointWeapon == null) _pointWeapon = transform;
    }

	void Update ()
	{
		attackCooldown -= Time.deltaTime;
	}

	public void Attack () //public void Attack (CharacterStats targetStats)
    {
		if (attackCooldown <= 0f)
		{
			
			StartCoroutine(DoAttack(DelayAntesDeAtacar)); //StartCoroutine(DoDamage(targetStats, attackDelay));

            if (OnAttack != null)
				OnAttack();

			attackCooldown = DelayEntreAtaques;
		}
		
	}

	IEnumerator DoAttack (float delay)  //IEnumerator DoDamage (CharacterStats stats, float delay)
    {
		yield return new WaitForSeconds(delay);
		//enemyController.animator.SetTrigger("Attack");

		if(_OnAttack != null) _OnAttack.Invoke();
		


        //stats.TakeDamage(myStats.damage.GetValue());
    }



	public void DoDmg()
	{

       // Detect enemies in range of attack
            Collider[] hitPlayer = Physics.OverlapSphere(_pointWeapon.position, _attackRange, _playerLayer);

            // Damage them
            foreach (Collider player in hitPlayer)
            {
                playerStats._vida -= myStats.damage.GetValue();
            }


	}

}
