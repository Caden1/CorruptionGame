using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : CharacterState
{
	private float moveDirection;

	public WalkingState(CharacterMovement characterMovement) : base(characterMovement) { }

	public void SetDirection(float direction) {
		moveDirection = direction;
	}

	public override void FixedUpdate() {
		float moveSpeed = characterMovement.GemController.GetRightFootGem().moveSpeed;
		characterMovement.Rb.velocity = new Vector2(moveDirection * moveSpeed, characterMovement.Rb.velocity.y);
		characterMovement.LastFacingDirection = moveDirection > 0 ? 1 : -1;
	}

	//public override void ExitState() { }

	//public override void EnterState() { }

	//public override void Update() { }
}
