using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

	public HandsBaseGemState CurrentHandsBaseGemState { get; set; }
	public FeetBaseGemState CurrentFeetBaseGemState { get; set; }
	public RightHandElementalModifierGemState CurrentRightHandElementalModifierGemState { get; set; }
	public LeftHandElementalModifierGemState CurrentLeftHandElementalModifierGemState { get; set; }
	public RightFootElementalModifierGemState CurrentRightFootElementalModifierGemState { get; set; }
	public LeftFootElementalModifierGemState CurrentLeftFootElementalModifierGemState { get; set; }

	public PlayerAnimationController animationController { get; set; }
	public PlayerEffectController effectController { get; set; }
	public GemController GemController { get; private set; }
	public Rigidbody2D Rb { get; private set; }
	public SpriteRenderer SpriteRend { get; private set; }
	public GroundCheck GroundCheck { get; private set; }

	public float OriginalGravity { get; private set; }
	public int numberOfJumps { get; set; } = 0;
	public float LastFacingDirection { get; set; } = 1;
	public bool IsImmune { get; set; } = false;
	public bool CanDash { get; set; } = true;
	public bool IsDashing { get; set; } = false;
	public bool CanUseRightHandSkill { get; set; } = true;
	public bool IsUsingRightHandSkill { get; set; } = false;
	public bool CanUseLeftHandSkill { get; set; } = true;
	public bool IsUsingLeftHandSkill { get; set; } = false;
	public bool IsDying { get; set; } = false;
	public bool HasForceApplied { get; set; } = false;

	private void Awake() {
		GemController = GetComponent<GemController>();
		GemController.OnGemsChanged += HandleGemChange;
		Rb = GetComponent<Rigidbody2D>();
		SpriteRend = GetComponent<SpriteRenderer>();
		OriginalGravity = GetComponent<Rigidbody2D>().gravityScale;
		animationController = GetComponent<PlayerAnimationController>();
		effectController = GetComponent<PlayerEffectController>();
		GroundCheck = GetComponent<GroundCheck>();
		states.Add(PlayerStateType.Idle, new IdleSkillState(this, inputActions, GemController));
		states.Add(PlayerStateType.Running, new RunningSkillState(this, inputActions, GemController));
		states.Add(PlayerStateType.Falling, new FallingSkillState(this, inputActions, GemController));
		states.Add(PlayerStateType.RightFoot, new RightFootSkillsState(this, inputActions, GemController));
		states.Add(PlayerStateType.LeftFoot, new LeftFootSkillsState(this, inputActions, GemController));
		states.Add(PlayerStateType.RightHand, new RightHandSkillsState(this, inputActions, GemController));
		states.Add(PlayerStateType.LeftHand, new LeftHandSkillsState(this, inputActions, GemController));
	}

	private void OnDestroy() {
		GemController.OnGemsChanged -= HandleGemChange;
	}

	private void Start() {
		// Set the initial state
		TransitionToState(
			PlayerStateType.Idle,
			HandsBaseGemState.Corruption,
			FeetBaseGemState.Purity,
			RightHandElementalModifierGemState.None,
			LeftHandElementalModifierGemState.None,
			RightFootElementalModifierGemState.None,
			LeftFootElementalModifierGemState.None
			);
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
			if (!IsDashing && !IsUsingRightHandSkill && !IsUsingLeftHandSkill && !HasForceApplied) {
				currentState?.UpdateState();
			}
		} else {
			PlayerDeath();
		}
	}

	public void TransitionToState(
		PlayerStateType newState,
		HandsBaseGemState newHandsBaseGemState,
		FeetBaseGemState newFeetBaseGemState,
		RightHandElementalModifierGemState newRightHandElementalModifierGemState,
		LeftHandElementalModifierGemState newLeftHandElementalModifierGemState,
		RightFootElementalModifierGemState newRightFootElementalModifierGemState,
		LeftFootElementalModifierGemState newLeftFootElementalModifierGemState
		) {
		currentState?.ExitState();
		currentState = states[newState];
		CurrentHandsBaseGemState = newHandsBaseGemState;
		CurrentFeetBaseGemState = newFeetBaseGemState;
		CurrentRightHandElementalModifierGemState = newRightHandElementalModifierGemState;
		CurrentLeftHandElementalModifierGemState = newLeftHandElementalModifierGemState;
		CurrentRightFootElementalModifierGemState = newRightFootElementalModifierGemState;
		CurrentLeftFootElementalModifierGemState = newLeftFootElementalModifierGemState;
		currentState.EnterState(
			CurrentHandsBaseGemState,
			CurrentFeetBaseGemState,
			CurrentRightHandElementalModifierGemState,
			CurrentLeftHandElementalModifierGemState,
			CurrentRightFootElementalModifierGemState,
			CurrentLeftFootElementalModifierGemState
			);
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

	private void HandleGemChange() {
		UpdatePlayerAbilities();
	}

	private void UpdatePlayerAbilities() {
		BaseGem handsBaseGem = GemController.GetBaseHandsGem();
		BaseGem feetbaseGem = GemController.GetBaseFeetGem();
		ModifierGem rightHandModifierGem = GemController.GetRightHandModifierGem();
		ModifierGem leftHandModifierGem = GemController.GetLeftHandModifierGem();
		ModifierGem rightFootModifierGem = GemController.GetRightFootModifierGem();
		ModifierGem leftFootModifierGem = GemController.GetLeftFootModifierGem();

		// Here, use the properties of the Gems to modify the player's abilities.
		switch (handsBaseGem.gemName) {
			case "None":
				CurrentHandsBaseGemState = HandsBaseGemState.None;
				break;
			case "Corruption":
				CurrentHandsBaseGemState = HandsBaseGemState.Corruption;
				break;
			case "Purity":
				CurrentHandsBaseGemState = HandsBaseGemState.Purity;
				break;
		}

		switch (feetbaseGem.gemName) {
			case "None":
				CurrentFeetBaseGemState = FeetBaseGemState.None;
				break;
			case "Corruption":
				CurrentFeetBaseGemState = FeetBaseGemState.Corruption;
				break;
			case "Purity":
				CurrentFeetBaseGemState = FeetBaseGemState.Purity;
				break;
		}
	}

	public void ResetNumberOfJumps() {
		switch (CurrentFeetBaseGemState) {
			case FeetBaseGemState.None:
				numberOfJumps = GemController.GetBaseFeetGem().numberOfJumps;
				break;
			case FeetBaseGemState.Purity:
				numberOfJumps = GemController.GetBaseFeetGem().numberOfJumps;
				break;
			case FeetBaseGemState.Corruption:
				numberOfJumps = GemController.GetBaseFeetGem().numberOfJumps;
				break;
		}

		switch (CurrentRightFootElementalModifierGemState) {
			case RightFootElementalModifierGemState.None:
				break;
			case RightFootElementalModifierGemState.Air:
				break;
			case RightFootElementalModifierGemState.Fire:
				break;
			case RightFootElementalModifierGemState.Water:
				break;
			case RightFootElementalModifierGemState.Earth:
				break;
		}

		switch (CurrentLeftFootElementalModifierGemState) {
			case LeftFootElementalModifierGemState.None:
				break;
			case LeftFootElementalModifierGemState.Air:
				break;
			case LeftFootElementalModifierGemState.Fire:
				break;
			case LeftFootElementalModifierGemState.Water:
				break;
			case LeftFootElementalModifierGemState.Earth:
				break;
		}
	}
}
