using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Collectable),
	typeof(DropItem))]
public class EnemyBase : MonoBehaviour {

	EnemyLife thisEnemyLife;

	private void Start() {
		thisEnemyLife = GetComponentInChildren<EnemyLife>();
		if(thisEnemyLife == null) {
			instatiateEnemyLifeInChild();
		}

		thisEnemyLife.OnEnemyDead += deathRoutine;
	}

	private void deathRoutine(object sender, System.EventArgs e) {
		this.GetComponent<Collectable>().CollectIt();
		this.GetComponent<DropItem>().drop();
	}

	private void instatiateEnemyLifeInChild() {
		System.Type[] EnemyLife = {
				typeof(EnemyLife)
		}; //componentes

		Instantiate(new GameObject("EnemyLife", EnemyLife), this.transform);
		thisEnemyLife = GetComponentInChildren<EnemyLife>(); //reload
	}
}
