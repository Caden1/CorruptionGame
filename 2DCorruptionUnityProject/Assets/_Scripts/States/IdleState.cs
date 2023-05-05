using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CharacterState
{
	public IdleState(CharacterMovement characterMovement) : base(characterMovement) { }

	public override void EnterState() {
		characterMovement.Rb.velocity = new Vector2(0, characterMovement.Rb.velocity.y);
	}

	//public override void ExitState() { }

	//public override void Update() { }

	//public override void FixedUpdate() { }
}
