using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent1 : MonoBehaviour {

    public Vector2 camera_fixed_position;
    public float moving_camera_time = 2, moving_door_time = 2;
    public GameObject door, boss, arena_things, boss_life_bar;
    GameObject player;
    bool event_start = false;
    GameObject camera_guide;
    Rigidbody2D rb_camera, rb_door, rb_boss;
    PlayerController player_controller;

    private void Start()
    {
        //Recebe o player e o controle
        player = GameObject.FindGameObjectWithTag("Player");
        player_controller = player.GetComponent<PlayerController>();

        //Recebe os rigidbodys2D e desativa o boss
        rb_door = door.GetComponent<Rigidbody2D>();
        rb_boss = boss.GetComponent<Rigidbody2D>();
        boss.GetComponent<FaceBoss>().enabled = false;
        rb_boss.simulated = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Quando o player sair do collider, começa o evento
        if(collision.CompareTag("Player") && !event_start)
        {
            event_start = true;

            //encontra a camera do jogo
            GameObject main_camera = GameObject.FindGameObjectWithTag("MainCamera");

            //cria o objeto guia da camera e move a camera principal para dentro dele
            create_camera_guide();
            main_camera.transform.parent = camera_guide.transform;

            StartCoroutine(Start_Event());

        }


    }

    IEnumerator Start_Event()
    {
        //desativa o controle do player
        player_controller.set_control_on_off(false);

        //descobre a distancia x e y do CameraGuide em relação ao ponto a qual ele deve parar
        float distance_x = camera_fixed_position.x - camera_guide.transform.position.x,
            distance_y = camera_fixed_position.y - camera_guide.transform.position.y;

        //calcula e aplica a velocidade x e y para que o objeto chegue ao ponto fixo
        rb_camera.velocity = new Vector2(distance_x / moving_camera_time, distance_y / moving_camera_time);

        //espera uma quantidade de tempo para realizar o proximo comando
        yield return new WaitForSeconds(moving_camera_time);

        //zera a velocidade, parando o objeto
        rb_camera.velocity = new Vector2(0, 0);
        StartCoroutine(Close_Door());

    }

    IEnumerator Close_Door()
    {
        //Checa o objeto abaixo da porta e adquire o ponto de colisão entre eles
        RaycastHit2D door_to_floor_hit = Physics2D.Raycast(door.transform.position - new Vector3(0, door.transform.localScale.y / 2 + 0.01f, 0), Vector2.down, 200000, 131073);
        RaycastHit2D floor_to_door_hit = Physics2D.Raycast(door_to_floor_hit.point + new Vector2(0, .01f), Vector2.up, 20000, 131073);

        //descobre a distancia y da porta e o objeto abaixo dela
        float distance_y = door_to_floor_hit.point.y - floor_to_door_hit.point.y;

        //muda a velocidade da porta para fechala
        rb_door.velocity = new Vector2(0, distance_y / moving_door_time);

        //espera uma quantidade de tempo para realizar o proximo comando
        yield return new WaitForSeconds(moving_door_time);

        //zera a velocidade, parando o objeto
        rb_door.velocity = new Vector2(0, 0);
        arena_things.SetActive(true);
        player_controller.set_control_on_off(true);
        boss.GetComponent<FaceBoss>().enabled = true;
        boss_life_bar.SetActive(true);
        Destroy(gameObject);
    }

    private void create_camera_guide()
    {
        //cria o objeto CameraGuide.
        camera_guide = new GameObject();
        camera_guide.name = "CameraGuide";

        //cria uma influencia fisica no objeto e desativa a influencia da gravidade
        rb_camera = camera_guide.AddComponent<Rigidbody2D>();
        rb_camera.gravityScale = 0;

        //move o CameraGuide para a posição do player
        camera_guide.transform.position = player.transform.position;
    }


}
