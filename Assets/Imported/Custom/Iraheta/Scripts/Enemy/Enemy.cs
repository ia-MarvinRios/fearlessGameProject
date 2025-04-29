using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles interaction with the Enemy */

[RequireComponent(typeof(CharacterStats))]
public class Enemy : Interactable {

	GameObject playerManager;
	CharacterStats myStats;

	void Start ()
	{
		playerManager = GameObject.FindGameObjectWithTag("Player");
        myStats = GetComponent<CharacterStats>();
	}

	public override void Interact()
	{
		base.Interact();
		CharacterCombat playerCombat = playerManager.GetComponent<CharacterCombat>();
		if (playerCombat != null)
		{
           
            playerCombat.Attack(); //playerCombat.Attack(myStats);
        }
	}

}
