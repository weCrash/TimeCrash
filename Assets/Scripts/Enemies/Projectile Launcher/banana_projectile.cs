using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banana_projectile : MonoBehaviour {


    public float max_distance;


    GameObject player;
    PlayerLife player_life;
    Vector3 start_position;
    Rigidbody2D rb;
    float fallen_time, distance_x, distance_y, gravity;
	// Use this for initialization
	void Start () {

        //recebe compoente Rigidbody2D e encontra o player e a vida do player
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        player_life = GameObject.FindGameObjectWithTag("PlayerLife").GetComponent<PlayerLife>();

        //defini a posição inicial a posição a qual foi instanciado
        start_position = transform.position;

        //Defini a gravidade do objeto
        gravity = rb.gravityScale * Physics2D.gravity.y;

        //Defini a distancia x e y em relação ao player
        distance_x = transform.position.x - player.transform.position.x;
        distance_y = transform.position.y - player.transform.position.y;

        //descobre o tempo que demora para o objeto, em influencia da gravidade, percorre a distance_y
        fallen_time = Mathf.Sqrt(Mathf.Abs(2 * distance_y / gravity));

        //Adiciona um modificador para a posição onde o projetil atingira
        float target_mod = Random.Range(0.2f, 0.8f);

        //defini a velocidade x, considerando o tempo de queda e a velocidade do player
        rb.velocity = new Vector2(distance_x / fallen_time - player.GetComponent<Rigidbody2D>().velocity.x * target_mod, rb.velocity.y) * -1;

        //defini rotação dependendo da distancia do player ao lançador
        rb.AddTorque(distance_x / distance_y * 700);

	}
	
	// Update is called once per frame
	void Update () {
        //caso passe a distancia maxima, destroi o objeto
        if (Vector2.Distance(start_position, transform.position) > max_distance) Destroy(gameObject);
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //caso colida com o player, da dano e destroi o objeto
        if(collision.CompareTag("Player"))
        {
            player_life.TakeDamage(1);
            Destroy(gameObject);
        }

        //caso não seja um inimigo, o player ou objetos sem colisão, destroi o objeto
        if (collision.gameObject.layer != 9 && collision.gameObject.layer != 8 && 
            collision.gameObject.layer != 14) Destroy(gameObject);
    }

}
