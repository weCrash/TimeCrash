using System.Collections.Generic;
using UnityEngine;

public class HookArrow : MonoBehaviour
{
    private PlayerController player;
    private GameObject arrow;
    private GameObject target;
    private Vector3 arrow_old_position;
    private bool target_locked = false;
    private InputHandler input;
    private newHook hook;
    private int target_value = 0;
    private List<Transform> hook_points = new List<Transform>();

    private void Start()
    {
        //Objetos de posição
        arrow = GameObject.FindGameObjectWithTag("HookArrow"); //Quando não está proximo de um ponto de hook, objeto se mantem proximo ao player
        target = GameObject.FindGameObjectWithTag("Target"); //Apenas um Objeto de visualização

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hook = GameObject.FindGameObjectWithTag("Player").GetComponent<newHook>();
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();

        arrow_old_position = arrow.transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Adiciona em uma array o Objeto que entre no circulo de colisão caso a tag seja "HookPoint"
        if (collision.tag == "HookPoint")
        {
            
            hook_points.Add(collision.transform);

            //Checa se o Objeto que entrou não é o mesmo que esta hookado, caso seja, mudara a posição do objeto target
            if (hook.Collider_hit != null && hook.Collider_hit.transform.position.x == target.transform.position.x && 
                hook.Collider_hit.transform.position.y == target.transform.position.y && hook.Is_hooked)
                target_value = hook_points.Count - 1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Remove da array o Objeto que sai do circulo de colisão caso a tag seja "HookPoint"
        if (collision.tag == "HookPoint")
        {
            hook_points.Remove(collision.transform);

            //Se não existir mais HookPoints por perto, desativa o Target
            if (hook_points.Count >= 0)
            {
                target.transform.localScale = new Vector3(0, 0, 0);
                arrow.transform.localPosition = new Vector3(arrow_old_position.x * player.Direction, arrow_old_position.y, arrow.transform.localPosition.z);
                target_value = 0;
            }
            
            //Se a index for maior que a quantidade de pontos, torna index a quantidade de pontos
            if (target_value + 1 > hook_points.Count)
                target_value = hook_points.Count;
        }
    }

    void Update()
    {
        //ao precionar o botão de Change_target, mudar o alvo
        if (input.activityDown("Change_Target"))
            change_target();

        //se existir algum ponto, ativa o target_locked
        if (hook_points.Count > 0)
        {
            target.transform.parent = null;
            target_locked = true;
        }
        else
        {
            target.transform.parent = player.transform;
            target_locked = false;
        }

        //Se existir um alvo move o sprite target para o hookpoint
        if (target_locked)
        {
			Transform currentHookPoint = hook_points[target_value];
            //Set target position
            target.transform.localScale = new Vector3(1, 1, 1);
            target.transform.rotation = currentHookPoint.rotation;
            target.transform.position = new Vector3(
				currentHookPoint.position.x,
				currentHookPoint.position.y,
				target.transform.position.z);

            arrow.transform.position = hook_points[target_value].position;

        }
        else if (player.Is_turning)
        {
            //Muda a posição do indicador arrow de acordo com a posição do player
            arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x * -1, arrow.transform.localPosition.y, arrow.transform.localPosition.z);
        }

    }

    public void change_target()
    {

        //muda o index do valor de alvo 
        if (target_value + 1 >= hook_points.Count) target_value = 0;
        else target_value++;
    }
}


