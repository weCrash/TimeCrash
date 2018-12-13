using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkamo : MonoBehaviour {
    public GameObject walk_check;
    public float walk_velocity = 5;
    float walk_radius;
    GameObject player;
    PlayerLife player_life;
    Rigidbody2D rb;
    bool walk_active;
    private Animator myAnimator;

    void Start () {
        myAnimator = GetComponent<Animator>();

        //Recebe colliders2D para utilizar sua Radius
        walk_radius = walk_check.GetComponent<CircleCollider2D>().radius;

        //procura objetos por tag
        player = GameObject.FindGameObjectWithTag("Player");
        player_life = GameObject.FindGameObjectWithTag("PlayerLife").GetComponent<PlayerLife>();

        //Recebe o componente Rigidbody2D dentro desse objeto
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        
        float velocity_y = rb.velocity.y;

        //Descobrindo a direção do inimigo em relação ao player
        float x_distance = player.transform.position.x - transform.position.x;
        int walk_direction = (int)Mathf.Sign(x_distance);

        if (walk_direction == -1) gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else gameObject.GetComponent<SpriteRenderer>().flipX = false;

        if (walk_active)
        {
            rb.velocity = new Vector2(walk_velocity * walk_direction, velocity_y);
            myAnimator.SetBool("walk", true);
            myAnimator.ResetTrigger("attack");
        }
    }

    private void Update()
    {
        //Checa se o player esta dentro da radius do collider pego em void Start() para o andar
        walk_active = overlapcircle_check(walk_radius, 512, "Player");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Caso atinja o player, da dano nele
        if (collision.gameObject.CompareTag("Player")) { 
            player_life.TakeDamage(2);
            myAnimator.SetTrigger("attack");
        }
    }


    bool overlapcircle_check(float collider_check, int layer_index, string tag)
    {

        GameObject G = null;
        //Checa se existe algum objeto X que possua a layer layer_index, em uma certa area circular centralizada em transform.position em um raio de collider_check * 0.5, faz G receber X
        if ((bool)Physics2D.OverlapCircle(transform.position, collider_check * .5f, layer_index))
            G = Physics2D.OverlapCircle(transform.position, collider_check * .5f, layer_index).gameObject;
        //Se o objeto existir, ser igual a tag e ser diferente do objeto em que esse script esta inserido, retorna true, se não retorna false
        if (G != null && G.CompareTag(tag) && G != gameObject)
        {
            return true;
        }
        else return false;

    }
}
