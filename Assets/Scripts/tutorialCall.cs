using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialCall : MonoBehaviour
{
    private tutorialCall self;

    public Transform tutorialHolder;
    public GameObject TutorialObject;
    public BoxCollider2D TutorialCollider;
    public bool desaparecerCollider;
    public float duracao;

    void Start ()
    {
        self = GetComponent<tutorialCall>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Se o que entrou no collider não tem a tag "Player", sera iniciado o que está dentro do if*/

        if (collision.gameObject.tag != "Player") {
            return;
        }

        for (int index = 0; index < tutorialHolder.childCount; index++) {
            tutorialHolder.GetChild(index).gameObject.SetActive(false);
        }

        StartCoroutine("tutorial");
    }

	/*IEnumerator é uma interface que marca as classes que desejam implementá-la para que se saiba que ela possa ter iterável através de um iterador;
   Neste caso esta criando uma coroutine */
	IEnumerator tutorial()
    {
        /*Ativa o tutObject (Imagem do tutorial que aparece na tela) e o boxCollider2D associado a variavel "box"*/

        TutorialObject.SetActive(true);
        TutorialCollider.enabled = false;

        yield return new WaitForSeconds(duracao);

        /*Quando chegar nessa parte do codigo, apenas após o valor definido a variavel float "duracao" segundos o que esta pra baixo dele sera executado*/
		/*Desativa o tutObject (Imagem do tutorial que aparece na tela) e o boxCollider2D associado a variavel "box"*/
        /*Desativa este script*/

        TutorialObject.SetActive(false);
		TutorialCollider.enabled = !desaparecerCollider;
		self.enabled = false;
     }
}
