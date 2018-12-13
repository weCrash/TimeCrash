using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(AudioSource))]
public class playOnCollided : MonoBehaviour {
	public float magnitureModifier = 0.001f;

	public float maxPitch = 1.25f;
	public float minPitch = .75f;

	public float maxVolumeVariation = 1.25f;
	public float minVolumeVariation = .9f;


	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag != "Player") {
			return;
		}
		PlayerController player = collision.gameObject.GetComponent<PlayerController>();

		float newVolume = player.LostVelocityVector.sqrMagnitude * magnitureModifier;

		AudioSource source = GetComponent<AudioSource>();
		source.pitch = Random.Range(minPitch, maxPitch);
		source.volume = newVolume * Random.Range(minVolumeVariation, maxVolumeVariation);
		source.Play();
	}
}
