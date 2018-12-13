using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Rigidbody2D),
	typeof(Collectable))]
public class StaticOrb : MonoBehaviour {

	public string orbCatcherTag = "OrbCatcher";

	private Rigidbody2D body2d;

	private void Start () {
		body2d = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision){
		if (collision.tag != orbCatcherTag){
			return;
		}

		body2d.constraints = RigidbodyConstraints2D.FreezeRotation;
	}
}
