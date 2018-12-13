using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlataform : MonoBehaviour 
{

	public string playerTagName = "Player"; 
    /*Por padrão a variavel string "playerTagName" (Vai receber a "tag" necessaria para fazer a plataforma cair) vai ter valor "Player", porem pode ser modificada*/
    public float fallTime = 3;
    /*Por padrão a variavel float "fallTime"(Responsavel pelo delay entre o player tocar a plataforma e ela cair) vai ter valor "3", porem pode ser modificada*/
    private Rigidbody2D plataform;
    /*Define que a variavel "plataform" é um Rigidbody2D*/
    public GameObject corda;
    /*Define que a variavel "corda" é um GameObject e necessita que seja atribuido algum valor a ela*/
    public float tremor;
    /*Define que "tremor" é uma variavel float e necessita que seja atribuido algum valor*/


    private void Start()
    {
        plataform = GetComponent<Rigidbody2D>();
        /*Associa a variavel "plataform" com o Rigidbody2D do componente onde o script é colocado*/
    }

    IEnumerator plataformShake(float fallTime)
    /*IEnumerator é uma interface que marca as classes que desejam implementá-la para que se saiba que ela possa ter iterável através de um iterador;
       Neste caso esta criando uma coroutine */
    {
        while (fallTime > 0)
            /*Enquanto a variavel float "fallTime" for maior que 0, o que esta dentro do while sera executado*/
        {
            plataform.position = new Vector2(plataform.position.x + (Random.insideUnitCircle.x * tremor), plataform.position.y);
            /*Modifica as posições do gameobject no mapa para dar efeito de tremor*/
            yield return new WaitForSeconds(0.0001f);
            /*Quando chegar nessa parte do codigo, apenas após 0.0001 segundos o que esta pra baixo dele sera executado*/
            fallTime -= Time.deltaTime;
            /*Define que o valor de fallTime vai ser igual a ele mesmo menos o tempo passado desde o ultimo frame*/
        }

        plataform.isKinematic = false;
        /*Define que o Rigidbody2D tem a função isKinematic falsa, fazendo ela "Perder a gravidade"*/
        corda.SetActive(false);
        /*Desabilita o gameobject corda, fazendo ela sumir*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
        /*Define o que vai acontecer apos ela entrar no collider*/
    {
		if (collision.tag != playerTagName)
            /*Se não for o player que entrou no collider, nada vai acontecer*/
        {
			return;
		}
		StartCoroutine(plataformShake(fallTime));
        /*Inicia a coutorine definida antes*/
    }
}
