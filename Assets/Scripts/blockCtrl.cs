using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockCtrl : MonoBehaviour {

	public int checkpoint_id;
	public PlayerController playerMovementRef;
	public float tempo;

	private bool beingBlocked = true;

	void Start() {
		if(MemoryManager.Memory.Checkpoint.ID != checkpoint_id) {
			return;
		}

		if(tempo > 0) {
			StartCoroutine("DisableScript");
			return;
		}

		//ignora a corotine e funciona a partir do On Collision Enter
		playerMovementRef.set_control_on_off(false);
		beingBlocked = true;
	}

	IEnumerator DisableScript() {
		playerMovementRef.set_control_on_off(false);
		yield return new WaitForSeconds(tempo);
		playerMovementRef.set_control_on_off(true);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.collider.tag != "Player")
			{ return; }
		if(!beingBlocked)
			{ return; }
		if(tempo > 0)
			{ return; }

		playerMovementRef.set_control_on_off(true);
		beingBlocked = false;
	}
}
