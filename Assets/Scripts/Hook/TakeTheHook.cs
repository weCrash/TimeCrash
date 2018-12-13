using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Collectable))]
public class TakeTheHook : MonoBehaviour {

	private void Start() {
		GetComponent<Collectable>().OnCollect += TakeTheHook_OnCollect;
	}

	private void TakeTheHook_OnCollect(object sender, System.EventArgs e) {
		//Activates hook script
		newHook playerHook = FindObjectOfType<newHook>();
		playerHook.Has_hook = true;

		//Saves this state
		MemoryManager.Memory.HasHook = true;
	}
}
