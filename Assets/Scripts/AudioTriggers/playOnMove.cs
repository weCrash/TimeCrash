using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(BoxCollider2D),
	typeof(AudioSource))]
public class playOnMove : MonoBehaviour {

	public float maxPitch = 1.25f;
	public float minPitch = .75f;

	public float maxVolume = 1f;
	public float minVolume = .7f;

	private Vector3 collisionLastPosition;

	private void OnTriggerStay2D(Collider2D collision) {
		if(collision.tag != "Player"){
			return;
		}
		if (collision.transform.position.RoundOneDecimal() == collisionLastPosition) {
			//check if the player has moved
			return;
		}
		collisionLastPosition = collision.transform.position.RoundOneDecimal();

		AudioSource source = GetComponent<AudioSource>();
		if(source.isPlaying) {
			return;
		}

		source.pitch = Random.Range(minPitch, maxPitch);
		source.volume = Random.Range(minVolume, maxVolume);
		source.Play();
	}

}
