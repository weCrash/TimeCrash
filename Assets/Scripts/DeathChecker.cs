using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathChecker : MonoBehaviour {

    //Variaveis Unity
    public GameObject death_canvas;
    public GameObject health;


	void Start () {

        //desativa a interface de morte no inicio
        death_canvas.SetActive(false);

	}
	
	void Update () {

        //se a vida do player estiver zerada, desaativa o sprite e o rigidbody2d no player, mostrando a tela de morte
        if(health.GetComponent<PlayerLife>().health <= 0)
        {
            health.GetComponent<PlayerLife>().set00();
            GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().simulated = false;
            death_canvas.SetActive(true);
        }
	}
}
