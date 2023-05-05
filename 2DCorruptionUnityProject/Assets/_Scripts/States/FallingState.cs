using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : CharacterState
{
	public FallingState(CharacterMovement characterMovement) : base(characterMovement) {}

	public override void EnterState() {
		if (characterMovement.Rb.velocity.y > 0) {
			characterMovement.Rb.velocity = new Vector2(characterMovement.Rb.velocity.x, characterMovement.Rb.velocity.y * 0f);
		}
	}

	//public override void Update() {}

	//public override void ExitState() {}

	//public override void FixedUpdate() { }
}
