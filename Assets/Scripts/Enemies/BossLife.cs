using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BossLife: MonoBehaviour {

    //Variaveis Unity
    public float health, show_damage_max_frames = 40;
    public GameObject Boss, boss_life_bar;

	public event EventHandler OnEnemyDead;

    RectTransform boss_life;
    float total_health;

	private void Start() {
        boss_life = boss_life_bar.GetComponent<RectTransform>();
        if (Boss == null) {
			Boss = GetComponentInParent<Transform>().gameObject;
		}
        total_health = health;
	}

	private void Update()
    {
        boss_life.localScale = new Vector3(health / total_health * -1, boss_life.localScale.y, boss_life.localScale.z);

		//Se o vida do objeto for menor que zero, destrui-lo
		if(health <= 0) {
			Destroy(Boss);
            EnemyDead();
        }
    }

	private void EnemyDead() {
		if (OnEnemyDead == null) {
			return;
		}
		OnEnemyDead(null, null);
	}
    //208, 156
    private IEnumerator TakeDamage()
    {
        float min_red_index =  (0.2f / show_damage_max_frames);
        Color color = Boss.GetComponent<SpriteRenderer>().color;
        for (int i = 1; i <= show_damage_max_frames; i++)
        {
            Boss.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g - min_red_index * i, color.b - min_red_index * i);
            yield return new WaitForFixedUpdate();
        }
        Boss.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b);

    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        //se encostar em um tiro do player, que possui tag "Bullet", diminuir a vida de acordo com o dano e destruir o objeto do tiro do player
        if (collision.CompareTag("Bullet"))
        {
            StartCoroutine (TakeDamage());
            health -= collision.GetComponent<newBullet>().damnum;
            Destroy(collision.gameObject);
        }
    }
}
