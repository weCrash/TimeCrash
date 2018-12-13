using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {

	public float forceModifier = 10;

	void Start () {
		addRandomForce();
	}

	private void addRandomForce() {
		float baseValue = .0001f * forceModifier;

		float y = Random.Range(-baseValue, baseValue);
		float x = Random.Range(-baseValue, baseValue);

		this.GetComponent<Rigidbody2D>()
			.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
	}
}
