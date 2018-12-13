using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(
	typeof(AudioSource))]
public class RandomizeAudioClip : MonoBehaviour {

	public AudioClip[] audios;


	private bool detected = false;
	private AudioSource source;

	private void Awake() {
		source = GetComponent<AudioSource>();
	}

	private void FixedUpdate() {
		if(source.isPlaying && !detected) {
			detected = true;
			StartCoroutine(waitForAudio());
		}
	}

	IEnumerator waitForAudio() {
		yield return new WaitWhile(() => source.isPlaying);

		source.clip = audios[Random.Range(0, audios.Length)];

		detected = false;
	}
}
