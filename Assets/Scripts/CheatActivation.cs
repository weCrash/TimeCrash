using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatActivation : MonoBehaviour {

    //Unity Objetos
    public GameObject cheat_particle;
    public GameObject player_life_object;
    public GameObject orb_catcher;


    PlayerLife player_life;
    int index = 0;
    float destroy_radius;
    bool cheat_on = false, invincibility = false, rambo = false;
    GameObject invicibility_particle = null, rambu_particle = null;
    GameObject player;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        player_life = player_life_object.GetComponent<PlayerLife>();
        destroy_radius = orb_catcher.GetComponent<CircleCollider2D>().radius;
	}
	
	void Update () {

        //Checa se as teclas estão sendo precionadas na ordem correta
        switch (index)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.UpArrow)) index++;
                break;
            case 1:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow)) index++;
                    else index = 0;
                }
                break;
            case 2:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow)) index++;
                    else index = 0;
                }
                break;
            case 3:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow)) index++;
                    else index = 0;
                }
                break;
            case 4:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) index++;
                    else index = 0;
                }
                break;
            case 5:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow)) index++;
                    else index = 0;
                }
                break;
            case 6:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) index++;
                    else index = 0;
                }
                break;
            case 7:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow)) index++;
                    else index = 0;
                }
                break;
            case 8:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.B)) index++;
                    else index = 0;
                }
                break;
            case 9:
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(KeyCode.A)) index++;
                    else index = 0;
                }
                break;


            case 10:
                //Ativa o CheatMode, Adiciona a particula ao player e desativa o comando de ativação
                Instantiate(cheat_particle, player.transform);
                cheat_on = true;
                index = -1;
                break;
        }



		if(cheat_on)
        {
            //quando precionar F10, ativa/desativa a invencibilidade, criando ou destruindo as particulas
            if (Input.GetKeyDown(KeyCode.F10))
            {

                if (invincibility)
                {
                    invincibility = false;
                    Destroy(invicibility_particle);
                }
                else
                {
                    invincibility = true;
                    invicibility_particle = Instantiate(cheat_particle, player.transform);
                    ParticleSystem.MainModule s = invicibility_particle.GetComponent<ParticleSystem>().main;
                    s.startColor = new Color(1, .192f, .192f, 1);
                }


            }
            //quando precionar F10, ativa/desativa a modo matador, criando ou destruindo as particulas
            if (Input.GetKeyDown(KeyCode.F11))
                if (rambo)
                {
                    rambo = false;
                    Destroy(rambu_particle);
                }
                else
                {
                    rambo = true;
                    rambu_particle = Instantiate(cheat_particle, player.transform);
                    ParticleSystem.MainModule s = rambu_particle.GetComponent<ParticleSystem>().main;
                    s.startColor = new Color(.192f, 1, .4f, 1);
                }


            //ativa/desativa no player_life a invencibilidade
            if (invincibility)
                player_life.active_damage = false;

            else
                player_life.active_damage = true;

            if(rambo)
            {
                Collider2D G = Physics2D.OverlapCircle(orb_catcher.transform.position, destroy_radius * 5f, 256);
                if (G != null) Destroy(G.gameObject);
            }
        }


	}
}
