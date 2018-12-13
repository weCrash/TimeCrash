using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    #region Unity
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
    public float 
        head_scale = 1,
        jump_min_heads,
        jump_max_heads;
    public float ground_check_radius = 0.2f;
    public float
        max_speed,
        momentum_initial_mod,
        momentum_stop_mod,
        hook_force;
    public Transform ground_check;
    public LayerMask where_can_walk;
    //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
    #endregion



    private float 
        jump_max_velocity,
        jump_min_velocity;

    private bool have_control = true;

    //Jump Stuff
    private bool
        is_jumping,
        jump_cancel;

    //Walk Stuff
    private bool
        is_turning = false;
    private int
        last_direction,
        direction = 1,
        walk_value;

    //Components

    public bool grounded;

    private newHook hook;
    private InputHandler input;
    private Rigidbody2D rb;
	private Vector2 lostVelocityVector;

	public int Direction {
        get { return direction; } 
        set { direction = value; }
    }

    public bool Is_turning {
        get { return is_turning; } 
        set { is_turning = value; }
    }

	public Vector2 LostVelocityVector {
		get { return lostVelocityVector; } 
		set { this.lostVelocityVector = value; }
	}

	void Start () {
        //recebe os componentes
        hook = GetComponent<newHook>();
        input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody2D>();

        hook_force *= 0.01f;
        rb.gravityScale = 1;

        //set velocity of jump by head count
        jump_max_velocity = Mathf.Sqrt(jump_max_heads * head_scale * Physics2D.gravity.y * -2);
        jump_min_velocity = Mathf.Sqrt(jump_min_heads * head_scale * Physics2D.gravity.y * -2);

		MemoryManager.OnSave += passAnimator;
	}
	private void OnDestroy() {
		MemoryManager.OnSave -= passAnimator;
	}

	private void passAnimator(object sender, EventArgs e) {
		MemoryManager.Memory.AnimatorController = GetComponent<Animator>().runtimeAnimatorController;
	}
	
	void FixedUpdate () {
        #region Current Velocitys
        float
            velocity_x = rb.velocity.x,
            velocity_y = rb.velocity.y;
        #endregion

        #region Jump Stuff Actions

        if (is_jumping)
        {
            is_jumping = false;
            rb.velocity = new Vector2(velocity_x, jump_max_velocity);
        }
        if (jump_cancel)
        {
            jump_cancel = false;
            if (velocity_y > jump_min_velocity)
                rb.velocity = new Vector2(velocity_x, jump_min_velocity);
        }


        #endregion

        #region Walk Stuff Actions

        //checa se o player esta hookado
        if (!hook.Is_hooked)
        {

            //checa se a velocidade do player não atingiu a velocidade maixma
            if (Mathf.Abs(velocity_x) <= max_speed)
            {
                //checa se ele ele virou, se sim, adiciona um impulso positivo a direção dele
                if (Is_turning)
                    rb.AddForce(new Vector2(velocity_x * momentum_stop_mod * 1.5f, 0));
                //se não, checa se o player não esta andando, adiciona um impulso negativo a direção dele
                else if (walk_value == 0)
                    rb.AddForce(new Vector2(velocity_x * -momentum_stop_mod * 1.5f, 0));
                //se nenhuma das duas, adiciona um impulso na direção em que esta andando
                else
                    rb.AddForce(new Vector2(max_speed * momentum_initial_mod * walk_value, 0));
            }
            //se não estiver no chão, adiciona um momento diferenciado
            else if(!grounded) rb.velocity = new Vector2((max_speed - max_speed * 0.02f) * (velocity_x / Mathf.Abs(velocity_x)), velocity_y);
        }
        else
        {
            //caso o player esteja conectado com o hook e não esteja andando, adiciona um impulso a corda
            if (walk_value != 0)
            {
                rb.velocity = new Vector2(velocity_x + hook_force * walk_value, velocity_y);
                

            }
            //se não, não modifica as velocidades
            else rb.velocity = new Vector2(velocity_x, velocity_y);
        }
        #endregion
    }

    void Update()
    {

        #region Jump Stuff Logic


        //cria um circulo de raio ground_check na posição doground_check para checar se algum objeto de layer where_can_walk esta colidindo, retornando uma variravel bool
        grounded = Physics2D.OverlapCircle(ground_check.position, ground_check_radius, where_can_walk);

		if(rb.velocity.y < 0 && grounded) {
			//muda veocidade em y pra 0 pro player não dar uma parada sempre que cai, perde velocidade/momentum, fica sem fluidez
			lostVelocityVector = rb.velocity; //save velocity state before messing with it, for external use
			rb.velocity = new Vector2(rb.velocity.x, 0);
		}

		//se estiver no chão e apertar o botão de pulo, inicia o pulo
		if (grounded && input.activityDown("Jump") && have_control)
        {
            is_jumping = true;
            jump_cancel = false;
        }
        //se não, caso tenha solto o botão do pulo, cancela o pulo
        else if (!grounded && !input.activityOf("Jump") && have_control)
        {
            is_jumping = false;
            jump_cancel = true;
        }


        #endregion

        #region Walk Stuff Logic

        //Recebe a direção atual
        last_direction = Direction;

        //modifica o index da direção e do andar para a direita e inicia a animação
        if (input.activityOf("Right_Walk") && have_control)
        {
            walk_value = 1;
            Direction = 1;
        }

        //modifica o index da direção e do andar para a esquerda e inicia a animação
        else if (input.activityOf("Left_Walk") && have_control)
        {
            Direction = -1;
            walk_value = -1;
        }

		//zera o valor do andar e desativa a animação
		else {
			walk_value = 0;
		}

		//checa se o player virou
		if (last_direction != Direction)
            Is_turning = true;
        else
            Is_turning = false;

        #endregion
    }

    public void set_control_on_off(bool toggle)
    {
		GetComponent<PlayerAnimatorController>().SetInIdle();
		GetComponent<PlayerAnimatorController>().UpdateAnimator = toggle;

		//if turning on, get from memory
		GetComponentInChildren<newHook>().Has_hook = toggle ? MemoryManager.Memory.HasHook : false;
		GetComponentInChildren<newGun>().Has_gun = toggle ? MemoryManager.Memory.HasGun : false;

        have_control = toggle;
    }
}