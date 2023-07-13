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
	// SET TO TRUE FOR SANDBOX
	public bool CanSwap { get; set; } = true; // = false;

	private UIDocument swapUIDoc;
	private SwapUISprites swapUISprites;

	private Dictionary<(string, string, string, string, string, string), Sprite> stateSpriteMapping;

	private void Awake() {
		GameObject swapUIDocGO = GameObject.FindWithTag("SwapUIDocument");
		if (swapUIDocGO != null) {
			swapUIDoc = swapUIDocGO.GetComponent<UIDocument>();
			swapUISprites = swapUIDocGO.GetComponent<SwapUISprites>();
		}
		swapUI = new SwapUI(swapUIDoc);

		swapUI.SetSilhouette(swapUISprites.purFeetCorHandsSilhouette);
		swapUI.SetRightHandIcon(swapUISprites.corruptionMeleeIcon);
		swapUI.SetLeftHandIcon(swapUISprites.corruptionRangedIcon);
		swapUI.SetRightFootIcon(swapUISprites.purityJumpIcon);
		swapUI.SetLeftFootIcon(swapUISprites.purityDashIcon);

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
		// Order (circular): Hands, Feet, Left Hand, Right Hand, Right Foot, Left Foot
		stateSpriteMapping = new Dictionary<(string, string, string, string, string, string), Sprite> {
			// Base Gems Only
			{ ("Purity", "None", "None", "None", "None", "None"),
					swapUISprites.purOnlyHandsSilhouette },
			{ ("None", "Purity", "None", "None", "None", "None"),
					swapUISprites.purOnlyFeetSilhouette },
			{ ("Purity", "Corruption", "None", "None", "None", "None"),
					swapUISprites.purHandsCorFeetSilhouette },
			{ ("Corruption", "Purity", "None", "None", "None", "None"),
					swapUISprites.purFeetCorHandsSilhouette },
			// With Modifier Gems
				// Purity Hands and Corruption Feet
			{ ("Purity", "Corruption", "AirModifier", "FireModifier", "WaterModifier", "EarthModifier"),
					swapUISprites.purHandsAllMods1Silhouette },
			{ ("Purity", "Corruption", "EarthModifier", "AirModifier", "FireModifier", "WaterModifier"),
					swapUISprites.purHandsAllMods2Silhouette },
			{ ("Purity", "Corruption", "WaterModifier", "EarthModifier", "AirModifier", "FireModifier"),
					swapUISprites.purHandsAllMods3Silhouette },
			{ ("Purity", "Corruption", "FireModifier", "WaterModifier", "EarthModifier", "AirModifier"),
					swapUISprites.purHandsAllMods4Silhouette },
				// Purity Feet and Corruption Hands
			{ ("Corruption", "Purity", "AirModifier", "FireModifier", "WaterModifier", "EarthModifier"),
					swapUISprites.purFeetAllMods1Silhouette },
			{ ("Corruption", "Purity", "EarthModifier", "AirModifier", "FireModifier", "WaterModifier"),
					swapUISprites.purFeetAllMods2Silhouette },
			{ ("Corruption", "Purity", "WaterModifier", "EarthModifier", "AirModifier", "FireModifier"),
					swapUISprites.purFeetAllMods3Silhouette },
			{ ("Corruption", "Purity", "FireModifier", "WaterModifier", "EarthModifier", "AirModifier"),
					swapUISprites.purFeetAllMods4Silhouette },
		};
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
			leftFootModifierGemName), out Sprite newSilhouette)) {
			swapUI.SetSilhouette(newSilhouette);
		} else {
			// Handle the case when the key does not exist.
		}
	}

	public void ResetNumberOfJumps() {
		switch (CurrentFeetBaseGemState) {
			case FeetBaseGemState.None:
				NumberOfJumps = GemController.GetBaseFeetGem().numberOfJumps;
				break;
			case FeetBaseGemState.Purity:
				NumberOfJumps = GemController.GetBaseFeetGem().numberOfJumps;
				break;
			case FeetBaseGemState.Corruption:
				NumberOfJumps = GemController.GetBaseFeetGem().numberOfJumps;
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
	}
}
