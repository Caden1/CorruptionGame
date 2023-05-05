using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : CharacterState
{
	private int remainingJumps;

	public JumpingState(CharacterMovement characterMovement) : base(characterMovement) { }

	public override void EnterState() {
		remainingJumps = characterMovement.GemController.GetRightFootGem().numberOfJumps;

		float jumpForce = characterMovement.GemController.GetRightFootGem().jumpForce;
		characterMovement.Rb.velocity = new Vector2(characterMovement.Rb.velocity.x, 0);
		characterMovement.Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
		remainingJumps--;
	}

	//public override void Update() {}

	//public override void FixedUpdate() { }

	//public override void ExitState() { }
}

