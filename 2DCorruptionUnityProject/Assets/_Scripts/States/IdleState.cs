using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : CharacterState
{
	public IdleState(CharacterMovement characterMovement) : base(characterMovement) { }

	public override void EnterState() {
		characterMovement.Rb.velocity = new Vector2(0, characterMovement.Rb.velocity.y);
	}
}
