using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Collectable))]
public class TakeTheGun : MonoBehaviour {

	public RuntimeAnimatorController newAnimatorController;

	private void Start() {
		GetComponent<Collectable>().OnCollect += TakeTheGun_OnCollect;
	}

	private void TakeTheGun_OnCollect(object sender, System.EventArgs e) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		//change animator to one that has the gun
		Animator playerAnimator = player.GetComponent<Animator>();
		playerAnimator.runtimeAnimatorController = newAnimatorController;

		//Activates gun script
		newGun playerGun = player.GetComponentInChildren<newGun>();
		playerGun.Has_gun = true;

		//Saves this state
		MemoryManager.Memory.AnimatorController = newAnimatorController;
		MemoryManager.Memory.HasGun = true;
	}
}
