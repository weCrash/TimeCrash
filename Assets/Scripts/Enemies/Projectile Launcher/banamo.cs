using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banamo : MonoBehaviour {

    //Variaveis Unity
    public GameObject player_check, projectile;
    public float fire_delay;

    private Animator myAnimator;
    int w8;
    float check_range;
    bool player_stay = false, can_shot = true;

	void Start () {
        myAnimator = GetComponent<Animator>();
        //Recebe colliders2D para utilizar sua Radius
        check_range = player_check.GetComponent<CircleCollider2D>().radius;
	}

    private void FixedUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        int direction = (int)Mathf.Sign(transform.position.x - player.transform.position.x);
        if (direction == -1) gameObject.GetComponent<SpriteRenderer>().flipX = true;
        else gameObject.GetComponent<SpriteRenderer>().flipX = false;
        //enquanto não puder atirar, soma ao contador para checar quando podera atirar de novo
        if (!can_shot)
        {
            w8++;
            myAnimator.SetBool("attack", true);

            //se o contador de frames do tiro estiver maior que a espera do tiro, pode-se atirar de novo
            if (w8 >= fire_delay)
            {
                can_shot = true;
                w8 = 0;
            }
        }

        //Realiza o "tiro" se puder atirar e se o player estiver dentro do raio desse objeto
        if(player_stay && can_shot)
        {
            myAnimator.SetBool("attack", true);
            //traz e estancia o projetil do tiro para a posição do objeto a qual esse script esta dentro e instancia
            Instantiate(projectile,
				new Vector2(transform.position.x, transform.position.y + .5f),
				Quaternion.identity);
            can_shot = false;

        }
    }

    void Update ()
    {

        //Checa se o player esta dentro da radius do collider pego em void Start()
        if (overlapcircle_check(check_range, 512, "Player"))
            player_stay = true;
        else
            player_stay = false;

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
