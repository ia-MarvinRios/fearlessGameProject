using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* Keeps track of enemy stats, loosing health and dying. */

public class EnemyStats : CharacterStats {

    public UnityEvent OnDie;
    public override void Die()
	{
		base.Die();

		// Add ragdoll effect / death animation
		if (OnDie != null) OnDie.Invoke(); 	
		Destroy(gameObject);
	}

}
