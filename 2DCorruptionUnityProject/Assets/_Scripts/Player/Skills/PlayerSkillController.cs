using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerSkillController : MonoBehaviour
{
	private PlayerInputActions inputActions;
	private Dictionary<PlayerStateType, PlayerSkillStateBase> states
		= new Dictionary<PlayerStateType, PlayerSkillStateBase>();
	private PlayerSkillStateBase currentState;
	private SwapUI swapUI;

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
	public int NumberOfJumps { get; set; } = 0;
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
	public bool CanSwap { get; set; } = true; // SET THIS TO TRUE FOR THE SANDBOX

	private UIDocument swapUIDoc;
	private SwapUISprites swapUISprites;
	private PlayerSkillControllerHelper playerSkillControllerHelper;

	private Dictionary<(string, string, string, string, string, string),
		(Sprite, Sprite, Sprite, Sprite, Sprite,
		HandsBaseGemState,
		FeetBaseGemState,
		LeftHandElementalModifierGemState,
		RightHandElementalModifierGemState,
		RightFootElementalModifierGemState,
		LeftFootElementalModifierGemState)> stateSpriteMapping;

	private void Awake() {
		GameObject swapUIDocGO = GameObject.FindWithTag("SwapUIDocument");
		if (swapUIDocGO != null) {
			swapUIDoc = swapUIDocGO.GetComponent<UIDocument>();
			swapUISprites = swapUIDocGO.GetComponent<SwapUISprites>();
		}
		swapUI = new SwapUI(swapUIDoc);

		swapUI.SetSilhouette(swapUISprites.purOnlyFeetSilhouette);
		swapUI.SetRightHandIcon(swapUISprites.noGemRightHandIcon);
		swapUI.SetLeftHandIcon(swapUISprites.noGemLeftHandIcon);
		swapUI.SetRightFootIcon(swapUISprites.purityOnlyRightFootIcon);
		swapUI.SetLeftFootIcon(swapUISprites.purityOnlyLeftFootIcon);

		GemController = GetComponent<GemController>();
		GemController.OnGemsChanged += HandleGemChange;
		Rb = GetComponent<Rigidbody2D>();
		SpriteRend = GetComponent<SpriteRenderer>();
		OriginalGravity = GetComponent<Rigidbody2D>().gravityScale;
		animationController = GetComponent<PlayerAnimationController>();
		effectController = GetComponent<PlayerEffectController>();
		GroundCheck = GetComponent<GroundCheck>();
	}

	private void OnDestroy() {
		GemController.OnGemsChanged -= HandleGemChange;
	}

	private void Start() {
		states.Add(PlayerStateType.Idle, new IdleSkillState(this, inputActions, GemController));
		states.Add(PlayerStateType.Running, new RunningSkillState(this, inputActions, GemController));
		states.Add(PlayerStateType.Falling, new FallingSkillState(this, inputActions, GemController));
		states.Add(PlayerStateType.RightFoot, new RightFootSkillsState(this, inputActions, GemController));
		states.Add(PlayerStateType.LeftFoot, new LeftFootSkillsState(this, inputActions, GemController));
		states.Add(PlayerStateType.RightHand, new RightHandSkillsState(this, inputActions, GemController));
		states.Add(PlayerStateType.LeftHand, new LeftHandSkillsState(this, inputActions, GemController));

		playerSkillControllerHelper = new PlayerSkillControllerHelper();
		SetupStateSpriteDictionary();

		// Set the initial state
		TransitionToState(
			PlayerStateType.Idle,
			HandsBaseGemState.Corruption,
			FeetBaseGemState.Purity,
			RightHandElementalModifierGemState.Fire,
			LeftHandElementalModifierGemState.Air,
			RightFootElementalModifierGemState.Water,
			LeftFootElementalModifierGemState.Earth
			);
	}

	private void SetupStateSpriteDictionary() {
		stateSpriteMapping = playerSkillControllerHelper.PopulateStateSpriteDictionary();
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
			if (!CanDash && IsGrounded()) {
				CanDash = true;
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
		BaseGem handsBaseGem = GemController.GetBaseHandsGem();
		BaseGem feetBaseGem = GemController.GetBaseFeetGem();
		ModifierGem rightHandModifierGem = GemController.GetRightHandModifierGem();
		ModifierGem leftHandModifierGem = GemController.GetLeftHandModifierGem();
		ModifierGem rightFootModifierGem = GemController.GetRightFootModifierGem();
		ModifierGem leftFootModifierGem = GemController.GetLeftFootModifierGem();

		string handsBaseGemName = handsBaseGem != null ? handsBaseGem.gemName : "None";
		string feetBaseGemName = feetBaseGem != null ? feetBaseGem.gemName : "None";
		string rightHandModifierGemName = rightHandModifierGem != null ? rightHandModifierGem.gemName : "None";
		string leftHandModifierGemName = leftHandModifierGem != null ? leftHandModifierGem.gemName : "None";
		string rightFootModifierGemName = rightFootModifierGem != null ? rightFootModifierGem.gemName : "None";
		string leftFootModifierGemName = leftFootModifierGem != null ? leftFootModifierGem.gemName : "None";

		if (stateSpriteMapping.TryGetValue(
			(handsBaseGemName,
			feetBaseGemName,
			leftHandModifierGemName,
			rightHandModifierGemName,
			rightFootModifierGemName,
			leftFootModifierGemName), out var tupleValue)) {
			swapUI.SetSilhouette(tupleValue.Item1);
			swapUI.SetLeftHandIcon(tupleValue.Item2);
			swapUI.SetRightHandIcon(tupleValue.Item3);
			swapUI.SetRightFootIcon(tupleValue.Item4);
			swapUI.SetLeftFootIcon(tupleValue.Item5);
			CurrentHandsBaseGemState = tupleValue.Item6;
			CurrentFeetBaseGemState = tupleValue.Item7;
			CurrentLeftHandElementalModifierGemState = tupleValue.Item8;
			CurrentRightHandElementalModifierGemState = tupleValue.Item9;
			CurrentRightFootElementalModifierGemState = tupleValue.Item10;
			CurrentLeftFootElementalModifierGemState = tupleValue.Item11;
		} else {
			// Handle the case when the key does not exist.
		}
	}

	public void ResetNumberOfJumps() {
		NumberOfJumps =
			GemController.GetBaseFeetGem().numberOfJumps
			+ GemController.GetRightFootModifierGem().numberOfJumps;
	}
}
