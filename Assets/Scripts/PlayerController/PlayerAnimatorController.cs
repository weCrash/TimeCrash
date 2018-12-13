using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(
	typeof(Rigidbody2D),
	typeof(PlayerController),
	typeof(newHook))]
[RequireComponent(
	typeof(SpriteRenderer),
	typeof(InputHandler),
	typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour {

	private Animator animator;
	private InputHandler input;
	private SpriteRenderer sprite;
	private newHook hook;
	private PlayerController player;
	private new Rigidbody2D rigidbody;

	private bool updateAnimator = true;
	public bool UpdateAnimator {
		get { return updateAnimator; } 
		set { this.updateAnimator = value; }
	}

	private void Start () {
		animator = GetComponent<Animator>();
		input = GetComponent<InputHandler>();
		sprite = GetComponent<SpriteRenderer>();
		hook = GetComponent<newHook>();
		player = GetComponent<PlayerController>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		if (!updateAnimator) {
			return;
		}

		bool isWalking = input.activityOf("left_walk") || input.activityOf("right_walk");
		animator.SetBool("walk", isWalking);

		flipSprite(isWalking);

		jumpTrigger();

        FallAnimator();
        LayerCtrlAnimator();
		HookedAnimator();

        shooting();
	}

	private void jumpTrigger() {
		if (!player.grounded && !input.activityDown("Jump")) {
			return;
		}
        animator.SetTrigger("jump");
	}

	private void flipSprite(bool isWalking) {
		if(hook.Is_hooked) {
			sprite.flipX = rigidbody.velocity.x < 0;
			return;
		}
		if (!isWalking) {
			return;
		}
		sprite.flipX = input.activityOf("left_walk");
	}


    void LayerCtrlAnimator()
    {
		float groundedWeight = !player.grounded ? 1 : 0;
		animator.SetLayerWeight(1, groundedWeight);

		float hookedWeight = hook.Is_hooked ? 1 : 0;
		animator.SetLayerWeight(2, hookedWeight);
	}

	void FallAnimator()
    {
        if(!player.grounded && rigidbody.velocity.y <=0) {
            shooting();
            animator.SetBool("fall", true);
            animator.ResetTrigger("jump");
        }
        if (player.grounded) {
            animator.SetBool("fall", false);
        }
    }

    void HookedAnimator()
    {
        if (hook.Is_hooked && !player.grounded) { 
            animator.SetBool("hooked", true);
            animator.ResetTrigger("jump");
        }
        if (player.grounded) {
            animator.SetBool("hooked", false);
        }
    }

    void shooting()
    {
        if (MemoryManager.Memory.HasGun)
        {
            bool isShooting = input.activityOf("shot");
            animator.SetBool("shooting", isShooting);
        }
    }

    public void SetInIdle() {
		animator.ResetTrigger("jump");
		animator.SetBool("walk", false);
		animator.SetBool("fall", false);

		if (MemoryManager.Memory.HasGun)
			animator.SetBool("shooting", false);
		if (MemoryManager.Memory.HasHook)
			animator.SetBool("hooked", false);
	}
}
