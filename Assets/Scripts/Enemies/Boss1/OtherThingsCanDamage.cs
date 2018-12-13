using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherThingsCanDamage : MonoBehaviour {

    PlayerLife player_life;
	// Use this for initialization
	void Start () {
        player_life = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerLife>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player_life.TakeDamage(1f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) player_life.TakeDamage(1f);
    }
}
