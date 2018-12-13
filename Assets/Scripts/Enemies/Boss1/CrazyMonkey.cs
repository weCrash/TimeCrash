using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyMonkey : MonoBehaviour {

    public float velocity_x, head_jump;
    public Color jump_mode_color;
    Rigidbody2D rb;
    float jump;
    int r;

	void Start () {
        //define propriedades de velocidade e de pulo em relação a direção
        rb = GetComponent<Rigidbody2D>();
        int direction = GameObject.FindGameObjectWithTag("HeadBoss").GetComponent<FaceBoss>().Direction;
        rb.velocity = new Vector2(velocity_x * -1, 0);
        jump = Mathf.Sqrt(head_jump * Physics2D.gravity.y * -2);
        r = Random.Range(1, 4);
        if (r==1) gameObject.GetComponent<SpriteRenderer>().color = jump_mode_color;

    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(transform.position.x - GameObject.FindGameObjectWithTag("Player").transform.position.x) <= 7
            && yes && r== 1)
        {
            yes = false;
            rb.velocity = new Vector2(velocity_x, jump);
        }
        //mantem constantemente andando
        rb.velocity = new Vector2(velocity_x * -1, rb.velocity.y);
    }

    bool yes = true;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //caso encontre com o player, possui a chance de dar um pulo
    //    if (collision.CompareTag("Player") && yes && r == 1)
    //    {
    //        yes = false;
    //        rb.velocity = new Vector2(velocity_x, jump);
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerLife>().TakeDamage(1);
        if(collision.gameObject.layer != 17 && collision.gameObject.layer != 19)
            Destroy(gameObject);
    }

}
