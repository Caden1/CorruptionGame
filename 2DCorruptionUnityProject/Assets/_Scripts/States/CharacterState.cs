using UnityEngine;

public abstract class CharacterState
{
	protected CharacterMovement characterMovement;

	public CharacterState(CharacterMovement characterMovement) {
		this.characterMovement = characterMovement;
	}

	public virtual void EnterState() { }

	public virtual void Update() { }

	public virtual void FixedUpdate() { }

	public virtual void ExitState() { }
}
