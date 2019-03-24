using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Collectable),
	typeof(DropItem))]
public class BreakByBullet : MonoBehaviour {

	[Header("Audio")]
	public float maxPitch = 1.25f;
	public float minPitch = .75f;

	public float maxVolume = 1f;
	public float minVolume = .7f;

	AudioSource source;
	private void Start() {
		source = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {

        //se atingir um objeto com tag "Bullet", destruir o proprio objeto e o objeot atingido
		if(!collision.CompareTag("Bullet")) {
			return;
		}

		GetComponent<Collectable>().CollectIt();
		GetComponent<DropItem>().drop();

		disableAllButAudioSource();

		//play sound
		source.pitch = Random.Range(minPitch, maxPitch);
		source.volume = Random.Range(minVolume, maxVolume);
		source.Play();

		Destroy(collision.gameObject);
		Destroy(this.gameObject, source.clip.length);
	}

	private void disableAllButAudioSource() {
		GetComponent<Renderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
	}
}
