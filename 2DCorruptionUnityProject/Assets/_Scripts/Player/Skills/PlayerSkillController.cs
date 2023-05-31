using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillController : MonoBehaviour
{
	private PlayerInputActions inputActions;
	private Dictionary<PlayerStateType, PlayerSkillStateBase> states
		= new Dictionary<PlayerStateType, PlayerSkillStateBase>();
	private PlayerSkillStateBase currentState;

	public LayerMask groundLayer;
	public float deathSeconds = 1.0f;

	public PurityCorruptionGem CurrentPurCorGemState { get; set; }
	public ElementalModifierGem CurrentElemModGemState { get; set; }

	public PlayerAnimationController animationController { get; set; }
	public PlayerEffectController effectController { get; set; }
	public GemController GemController { get; private set; }
	public Rigidbody2D Rb { get; private set; }
	public GroundCheck GroundCheck { get; private set; }

	public int numberOfJumps { get; set; } = 0;
	public float LastFacingDirection { get; set; } = 1;
	public bool CanDash { get; set; } = true;
	public bool IsDashing { get; set; } = false;
	public bool IsDying { get; set; } = false;

	private void Awake() {
		GemController = GetComponent<GemController>();
		Rb = GetComponent<Rigidbody2D>();
		animationController = GetComponent<PlayerAnimationController>();
		effectController = GetComponent<PlayerEffectController>();
		GroundCheck = GetComponent<GroundCheck>();
		states.Add(PlayerStateType.Idle, new IdleSkillState(this, inputActions));
		states.Add(PlayerStateType.Running, new RunningSkillState(this, inputActions));
		states.Add(PlayerStateType.Jumping, new JumpingSkillState(this, inputActions));
		states.Add(PlayerStateType.Falling, new FallingSkillState(this, inputActions));
		states.Add(PlayerStateType.Dashing, new DashingSkillState(this, inputActions));
	}

	private void Start() {
		// Set the initial state
		TransitionToState(PlayerStateType.Idle);
	}

	public void SetInputActionsInitializeStateClasses(PlayerInputActions inputActions) {
		this.inputActions = inputActions;
	}

	public Coroutine StartStateCoroutine(IEnumerator routine) {
		return StartCoroutine(routine);
	}

	public void StopStateCoroutine(Coroutine coroutine) {
		if (coroutine != null) {
			StopCoroutine(coroutine);
		}
	}

	private void Update() {
		if (!IsDying) {
			if (!IsDashing) {
				currentState?.UpdateState();
			}
		} else {
			PlayerDeath();
		}
	}

	public void TransitionToState(PlayerStateType newState,
		PurityCorruptionGem newPurCorGemState = PurityCorruptionGem.None,
		ElementalModifierGem newElemModGemState = ElementalModifierGem.None) {
		currentState?.ExitState();
		currentState = states[newState];
		CurrentPurCorGemState = newPurCorGemState;
		CurrentElemModGemState = newElemModGemState;
		currentState.EnterState(CurrentPurCorGemState, CurrentElemModGemState);
	}

	public bool IsGrounded() {
		return GroundCheck.IsGrounded();
	}

	private void PlayerDeath() {
		animationController.ExecuteDeathAnim();
		Rb.gravityScale = 0f;
		GetComponent<BoxCollider2D>().enabled = false;
		StartCoroutine(DestroyPlayerAfterSec());
	}

	private IEnumerator DestroyPlayerAfterSec() {
		yield return new WaitForSeconds(deathSeconds);
		Destroy(gameObject);
	}

	public void ResetNumberOfJumps() {
		switch (CurrentPurCorGemState) {
			case PurityCorruptionGem.None:
				numberOfJumps = GemController.GetRightFootGem().numberOfJumps;
				break;
			case PurityCorruptionGem.Purity:
				break;
			case PurityCorruptionGem.Corruption:
				break;
		}

		switch (CurrentElemModGemState) {
			case ElementalModifierGem.None:
				break;
			case ElementalModifierGem.Air:
				break;
			case ElementalModifierGem.Fire:
				break;
			case ElementalModifierGem.Water:
				break;
			case ElementalModifierGem.Earth:
				break;
		}
	}
}
