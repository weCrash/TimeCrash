using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class newBullet : MonoBehaviour
{

    //Variaveis Unity
    public int max_distance = 20;
    public float damnum;
    public LayerMask where_destroy_layer;
    public string[] safe_tags;


    Rigidbody2D rb;
    PlayerController player;
    Vector3 start_position;
    List<string> destroy_tags = new List<string>();

    
    void Start()
    {
        //Adiciona a destroy_tags todas as tags de todos os objetos
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject o in allObjects)
            destroy_tags.Add(o.tag);

        //Remove duplicatas da lista destroy_tags
        destroy_tags = destroy_tags.Distinct().ToList();

        //Editando direção do sprite de acordo com a direção do player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player.Direction == 1) gameObject.GetComponent<SpriteRenderer>().flipX = false;
        else if (player.Direction == -1) gameObject.GetComponent<SpriteRenderer>().flipX = true;

        //recolhe a posição do tiro quando é instanciado
        start_position = transform.position;

        //Editando a velocidade do objeto
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(30 * player.Direction, 0);

        //remove da array destroy_tags, tags segura que não devem destruir o objeto
        foreach (string s in safe_tags)
        {
            destroy_tags.Remove(s);
        }
    }

    void FixedUpdate()
    {
        //Verificando distancia maxima
        if (Vector2.Distance(start_position, transform.position) >= max_distance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Verifica a layer e a tag do objeto atinjido para destruir o objeto
        foreach (string s in destroy_tags)
        {
            //checa se a tag destruira o objeto
            if (collision.CompareTag(s))
                //cria um pequeno circulo de raio CircleCollider2D * .5f em torno do objeto e checa se existe algum objeto que possui tags "where_destroy_layers"
                if (Physics2D.OverlapCircle(transform.position, gameObject.GetComponent<CircleCollider2D>().radius * .5f, where_destroy_layer))
                    //destroi o objeto
                    Destroy(gameObject);
        }
    }
}
