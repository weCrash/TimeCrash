using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInPortal : MonoBehaviour {

    public int check_id;
	public GameObject player;
    public GameObject surge;
    public GameObject gun;
    public float waitToAppear;
    
	void Start () {
        if (MemoryManager.Memory.Checkpoint.ID == check_id)
        {
            togglePlayer(false);
            surge.SetActive(false);

			player.transform.position = this.transform.position;
            StartCoroutine(appear());
        }
	}

	IEnumerator appear(){

        yield return new WaitForSeconds(waitToAppear);
        surge.SetActive(true);
        yield return new WaitForSeconds(0.09f);
        togglePlayer (true);
	}

	private void togglePlayer(bool toggle){
		SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer> ();
		Rigidbody2D playerRigidBody = player.GetComponent<Rigidbody2D> ();
        newHook playerHook = player.GetComponent<newHook> ();
        surge.SetActive(false);
        playerRenderer.enabled = toggle;
		playerRigidBody.simulated = toggle;
        playerHook.enabled = toggle;
        gun.GetComponent<newGun>().enabled = toggle;
    }
}
