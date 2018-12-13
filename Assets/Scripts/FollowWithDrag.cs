using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithDrag : MonoBehaviour {

	public Transform followThis;
	[Range(0f,1f)]
	[Tooltip("Começa a ser perceptivel no .2 pra baixo")]
	public float drag = 1;

	void FixedUpdate () {
		Vector2 targetPosition = followThis.position;
		Vector2 thisPosition = this.transform.position;

		Vector2 difference = targetPosition - thisPosition;

		this.transform.position += (Vector3)(difference * drag);
	}
}
