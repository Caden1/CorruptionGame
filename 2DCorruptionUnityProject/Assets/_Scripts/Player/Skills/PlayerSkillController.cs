using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillController : MonoBehaviour
{
	private PlayerInputActions inputActions;

	public LayerMask groundLayer;

	public PurityCorruptionGem CurrentPurCorGemState { get; set; }
	public ElementalModifierGem CurrentElemModGemState { get; set; }

	public PlayerSkillStateBase CurrentSkillState { get; private set; }
	public PlayerAnimationController animationController { get; set; }
	public IdleSkillState IdleSkillState { get; set; }
	public RunningSkillState RunningSkillState { get; set; }
	public JumpingSkillState JumpingSkillState { get; set; }
	public FallingSkillState FallingSkillState { get; set; }
	public DashingSkillState DashingSkillState { get; set; }
	public GemController GemController { get; private set; }
	public Rigidbody2D Rb { get; private set; }
	public GroundCheck GroundCheck { get; private set; }

	public float LastFacingDirection { get; set; } = 1;
	public bool CanDash { get; set; } = true;
	public bool IsDashing { get; set; } = false;


	private void Awake() {
		GemController = GetComponent<GemController>();
		Rb = GetComponent<Rigidbody2D>();
		animationController = GetComponent<PlayerAnimationController>();
		GroundCheck = GetComponent<GroundCheck>();
	}

	public void SetInputActionsInitializeStateClasses(PlayerInputActions inputActions) {
		this.inputActions = inputActions;

		IdleSkillState = new IdleSkillState(this, inputActions);
		RunningSkillState = new RunningSkillState(this, inputActions);
		JumpingSkillState = new JumpingSkillState(this, inputActions);
		FallingSkillState = new FallingSkillState(this, inputActions);
		DashingSkillState = new DashingSkillState(this, inputActions);
		DashingSkillState.StartCoroutine = StartCoroutineWrapper;
	}

	private void StartCoroutineWrapper(IEnumerator coroutine) {
		StartCoroutine(coroutine);
	}

	private void Update() {
		if (!IsDashing) {
			CurrentSkillState.UpdateState();
		}
	}

	public void TransitionToState(PlayerSkillStateBase newState,
		PurityCorruptionGem newPurCorGemState = PurityCorruptionGem.None,
		ElementalModifierGem newElemModGemState = ElementalModifierGem.None) {
		CurrentSkillState = newState;
		CurrentPurCorGemState = newPurCorGemState;
		CurrentElemModGemState = newElemModGemState;
		CurrentSkillState.EnterState(CurrentPurCorGemState, CurrentElemModGemState);
	}

	public bool IsGrounded() {
		return GroundCheck.IsGrounded();
	}
}
