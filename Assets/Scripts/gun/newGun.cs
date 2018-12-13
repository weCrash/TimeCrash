using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(AudioSource))]
public class newGun : MonoBehaviour {

    //Variaveis Unity
    public GameObject bullet;
    public float velocity = 30;
    public float gun_cooldown = 15;
	public Vector3 bulletSpawnOffset;


    InputHandler input;
    PlayerController player;
	AudioSource audiosource;
    int w8 = 0;
    bool can_shot = true;

	private bool has_gun;
	public bool Has_gun {
		get { return has_gun; } 
		set { has_gun = value; }
	}

	void Start ()
    {

		audiosource = GetComponent<AudioSource>();
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		MemoryManager.OnSave += passGunToMemory;
    }

	void OnDestroy() {
		MemoryManager.OnSave -= passGunToMemory;
	}

	private void passGunToMemory(object sender, System.EventArgs e) {
		MemoryManager.Memory.HasGun = Has_gun;
	}

	private void FixedUpdate()
    {
		if (!Has_gun) {
			can_shot = false;
			return;
		}

        //Tempo de "recarga em frames"
        if (!can_shot)
        {
            w8++;
            if (w8 >= gun_cooldown) {
				can_shot = true;
                w8 = 0;
            }
        }
    }

    
    void Update()
    {
        //Rotaciona o objeto em relação ao player
        if (player.Is_turning) {
            transform.localPosition = new Vector3(transform.localPosition.x * -1, transform.localPosition.y, transform.localPosition.z);
        }

        //realiza o tiro no precionar do comando se puder
        if (input.activityOf("Shot") && can_shot)
        {
			//instanciando o tipo da posição da arma
            Instantiate(
				bullet, transform.position + bulletSpawnOffset,
				bullet.transform.rotation);

            can_shot = false;

			audiosource.volume = Random.Range(0.5f, 1f);
			audiosource.Play();
        }
    }
}
