using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Collectable))]
public class CheckPoint : MonoBehaviour
{

	public int id;
	public Sprite changeTo;

	private string player_tag = "Player";

	private void Start() {
		bool wasCollected = MemoryManager.Was(this).Taken;
		if(wasCollected) {
			change_graphics();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.gameObject.tag.Equals(player_tag)) {
			return;
		}
		if(MemoryManager.Memory.Checkpoint.ID == id) {
			return;
		}
		if(MemoryManager.Was(this).Taken) {
			return;
		}

		take();
	}

	public void take()
	{
		//Save to memory: Checkpoint ID and scene index
		MemoryManager.Memory.Checkpoint.ID = id;
		MemoryManager.Memory.Checkpoint.SavedPosition = transform.position;
		MemoryManager.Memory.Checkpoint.SceneIndex = gameObject.scene.buildIndex;

		this.GetComponent<Collectable>().CollectIt();
		MemoryManager.ConsolidateMemory();

		change_graphics();
	}

	void change_graphics()
	{
		if (GetComponent<SpriteRenderer>() != null)
		{
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = changeTo;
		}
	}
}
