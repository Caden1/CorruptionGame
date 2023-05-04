using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : CharacterState
{
	private int remainingJumps;

	public JumpingState(CharacterMovement characterMovement) : base(characterMovement) { }

	public override void EnterState() {
		remainingJumps = characterMovement.GemController.GetRightFootGem().numberOfJumps;

		Jump();
	}

	public override void Update() {
		if (remainingJumps > 0 && Input.GetButtonDown("Jump")) {
			Jump();
		}

		if (characterMovement.Rb.velocity.y <= 0) {
			characterMovement.TransitionToState(characterMovement.IdleState);
		}
	}

	private void Jump() {
		float jumpForce = characterMovement.GemController.GetRightFootGem().jumpForce;
		characterMovement.Rb.velocity = new Vector2(characterMovement.Rb.velocity.x, 0);
		characterMovement.Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
		remainingJumps--;
	}
}

