using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour {

    //Variaveis Unity
    public float health = 10;
    public GameObject enemy;

	public event EventHandler OnEnemyDead;

	private void Start() {
		if (enemy == null) {
			enemy = GetComponentInParent<Transform>().gameObject;
		}
	}

	private void Update()
    {
		//Se o vida do objeto for menor que zero, destrui-lo
		if(health <= 0) {
			Destroy(enemy);
            EnemyDead();
        }
    }

	private void EnemyDead() {
		if (OnEnemyDead == null) {
			return;
		}
		OnEnemyDead(null, null);
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        //se encostar em um tiro do player, que possui tag "Bullet", diminuir a vida de acordo com o dano e destruir o objeto do tiro do player
        if (collision.CompareTag("Bullet"))
        {
            health -= collision.GetComponent<newBullet>().damnum;
            Destroy(collision.gameObject);
        }
    }
}
