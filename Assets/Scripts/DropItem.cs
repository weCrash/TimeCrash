using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour {
	
	public int quantity;
	public GameObject item;
	public Vector3 spawnOffset;

	public void drop() {
		for (int i = 0; i < quantity; i++) {
			Instantiate(
				item,
				transform.position + spawnOffset,
				Quaternion.identity);
		}
	}
}
