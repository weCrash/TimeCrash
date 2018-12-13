using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Rigidbody2D),
	typeof(Collectable),
	typeof(DropItem))]
public class BreakOnStomp : MonoBehaviour {
//script to break the current object when the player is on top

	public Sprite changeSpriteTo;

	private void Start() {
		bool wasCollected = MemoryManager.Was(this).Taken;
		if(wasCollected) {
			breakThis();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player") {
			return;
		}

		breakThis();

		this.GetComponent<DropItem>().drop();
		this.GetComponent<Collectable>().CollectIt();
	}

	private void breakThis() {
		//when set simulated to false, it prevents the collider to interact with the other objects in scene
		this.GetComponent<Rigidbody2D>().simulated = false;
		this.GetComponent<SpriteRenderer>().sprite = changeSpriteTo;
	}
}
