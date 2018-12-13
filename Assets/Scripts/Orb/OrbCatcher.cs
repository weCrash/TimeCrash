using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbCatcher : MonoBehaviour {

	public string orbTag = "Orb";
    public Sprite[] number;
    public GameObject N1, N10, N100, N1000;

	private AudioSource source;

	private void Start() {
		source = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (orbTag != collision.tag) {
			return;
		}
		//play sound
		source.Play();

		MemoryManager.Memory.Orbs++;
		Destroy(collision.gameObject);

		Collectable orb = collision.GetComponent<Collectable>();
		if (orb != null) {
			orb.CollectIt();
		}
	}

    private void FixedUpdate()
    {
        int d1000, d100, d10, d1;

		int orbCount = MemoryManager.Memory.Orbs;
        d1 = orbCount % 10;
        put_number(d1, N1);
        d10 = ((orbCount % 100) - d1) / 10;
        put_number(d10, N10);
        d100 = ((orbCount % 1000) - (d10 * 10) - d1) / 100;
        put_number(d100, N100);
        d1000 = (orbCount - d100 * 100 - d10 * 10 - d1) / 1000;
        put_number(d1000, N1000);
    }

    void put_number(int index, GameObject obj)
    {
        Image img = obj.GetComponent<Image>();
        switch (index)
        {
            case 0:
                img.sprite = number[0];
                break;
            case 1:
                img.sprite = number[1];
                break;
            case 2:
                img.sprite = number[2];
                break;
            case 3:
                img.sprite = number[3];
                break;
            case 4:
                img.sprite = number[4];
                break;
            case 5:
                img.sprite = number[5];
                break;
            case 6:
                img.sprite = number[6];
                break;
            case 7:
                img.sprite = number[7];
                break;
            case 8:
                img.sprite = number[8];
                break;
            case 9:
                img.sprite = number[9];
                break;
        }
    }
}
