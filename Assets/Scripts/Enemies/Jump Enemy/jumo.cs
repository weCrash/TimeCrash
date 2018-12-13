using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumo : MonoBehaviour {

    //Variaveis Unity
    public GameObject walk_check, jump_check;
    public float walk_velocity = 5, head_big_jumps = 8, head_small_jump = 1;
    public Transform ground_check;

    private Animator myAnimator;

    GameObject player;
    PlayerLife player_life;
    Rigidbody2D rb;
    float velocity_y;
    float walk_radius, jump_radius, jump_total_time;
    bool walk_active = false,
        normal_jump_active = false,
        small_jump_active = false,
        is_small_jump = false,
        is_big_jump = false,
        grounded;


    void Start()
    {
        myAnimator = GetComponent<Animator>();

        //Recebe colliders2D para utilizar sua Radius
        walk_radius = walk_check.GetComponent<CircleCollider2D>().radius;
        jump_radius = jump_check.GetComponent<CircleCollider2D>().radius;

        //procura objetos por tag
        player = GameObject.FindGameObjectWithTag("Player");
        player_life = GameObject.FindGameObjectWithTag("PlayerLife").GetComponent<PlayerLife>();

        //Recebe o componente Rigidbody2D dentro desse objeto
        rb = gameObject.GetComponent<Rigidbody2D>();

        float
            fallen_distance = head_big_jumps - 2,
            gravity = Physics2D.gravity.y * rb.gravityScale,
            jump_up_time = Mathf.Sqrt(Mathf.Abs(2 * head_big_jumps / gravity)),
            jump_fallen_time = Mathf.Sqrt(Mathf.Abs(2 * fallen_distance / gravity));

        jump_total_time = jump_up_time + jump_fallen_time;

    }

    private void FixedUpdate()
    {
        FallAnimator();
        LayerCtrlAnimator();

        velocity_y = rb.velocity.y;


        //Descobrindo a direção do inimigo em relação ao player
        float distance_x = player.transform.position.x - transform.position.x;
        int walk_direction = (int)Mathf.Sign(distance_x);

        if (walk_direction == -1) gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else gameObject.GetComponent<SpriteRenderer>().flipX = false;


        if (walk_active && velocity_y == 0)
        {
            rb.velocity = new Vector2(walk_velocity * walk_direction, velocity_y);
            myAnimator.SetBool("walk", true);
            /* myAnimator.SetBool("walk", true);
             myAnimator.ResetTrigger("jump");
             myAnimator.ResetTrigger("attack");*/
        }
        else
        {
            myAnimator.SetBool("walk", false);
        }

        if (is_big_jump)
        {
            rb.velocity = new Vector2(distance_x / jump_total_time + player.GetComponent<Rigidbody2D>().velocity.x * .2f, Mathf.Sqrt(head_big_jumps * Physics2D.gravity.y * -2));
            /*  myAnimator.SetBool("walk", false);
              myAnimator.SetLayerWeight(1, 1);
              myAnimator.SetTrigger("jump");
              myAnimator.ResetTrigger("attack");*/
            myAnimator.SetTrigger("jump");
        }

        if (is_small_jump)
        {
            rb.velocity = new Vector2(0, Mathf.Sqrt(head_small_jump * Physics2D.gravity.y * -2));
            myAnimator.SetTrigger("jump");
        }
    }


    void Update()
    {
        //Checa se o player esta dentro da radius do collider pego em void Start() para o andar
        walk_active = overlapcircle_check(walk_radius, 512, "Player");

        //Checa se o player esta dentro da radius do collider pego em void Start() para o pular
        normal_jump_active = overlapcircle_check(jump_radius, 512, "Player");

        if (normal_jump_active) walk_active = false;


        //Checa se não existem outros "Jumos" por perto, caso exista, ira realizar pequenos pulos
        small_jump_active = overlapcircle_check(jump_radius, 256, "Jumo");

        //Checa se o Objeto esta no chão
        grounded = Physics2D.OverlapCircle(ground_check.position, 0.2f, -1856257);

        //Modifica a velocidade Y quando encosta no chão
        if (rb.velocity.y < 0 && grounded) rb.velocity = new Vector2(rb.velocity.x, 0);

        //Ativa o pulo sobre o player
        if (grounded && !small_jump_active && normal_jump_active)
            is_big_jump = true;
        else
            is_big_jump = false;

        //Ativa os pequenos pulos
        if (grounded && small_jump_active && !walk_active)
            is_small_jump = true;
        else
            is_small_jump = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Caso atinja o player, da dano nele
        if(collision.gameObject.CompareTag("Player"))
            player_life.TakeDamage(2);
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

    void FallAnimator()
    {
        if (!grounded && rb.velocity.y <= 0)
        {
            myAnimator.SetBool("fall", true);
            myAnimator.ResetTrigger("jump");
        }
        if (grounded)
        {
            myAnimator.SetBool("fall", false);
        }
    }

    void LayerCtrlAnimator()
    {
        if (!grounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

}