using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(AudioSource),
	typeof(Collectable))]
public class PlaySoundOnCollect : MonoBehaviour {

	public float maxPitch = 1.25f;
	public float minPitch = .75f;

	public float maxVolume = 1f;
	public float minVolume = .7f;

	AudioSource source;
	Collectable collectable;

	private void Start() {
		source = GetComponent<AudioSource>();

		collectable = GetComponent<Collectable>();
		collectable.OnCollect += PlaySound;
	}

	private void PlaySound(object sender, System.EventArgs e) {
		source.pitch = Random.Range(minPitch, maxPitch);
		source.volume = Random.Range(minVolume, maxVolume);
		source.Play();

		if(collectable.autoDestroy) {
			collectable.autoDestroy = false;
			StartCoroutine(destroyAfterClip());
		}
	}

	private IEnumerator destroyAfterClip() {
		GetComponent<Collider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		DestroyAllChildren();

		yield return new WaitWhile(() => source.isPlaying);
		Destroy(this.gameObject);
	}

	private void DestroyAllChildren() {
		var children = new List<GameObject>();
		foreach(Transform child in this.transform) {
			children.Add(child.gameObject);
		}

		children.ForEach(child => Destroy(child));
	}
}