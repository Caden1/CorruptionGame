using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillController : MonoBehaviour
{
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

	public float HorizontalInput { get; set; }
	public float LastFacingDirection { get; set; } = 1;

	private void Awake() {
		GemController = GetComponent<GemController>();
		Rb = GetComponent<Rigidbody2D>();
		animationController = GetComponent<PlayerAnimationController>();

		IdleSkillState = new IdleSkillState(this);
		RunningSkillState = new RunningSkillState(this);
		JumpingSkillState = new JumpingSkillState(this);
		FallingSkillState = new FallingSkillState(this);
		DashingSkillState = new DashingSkillState(this);
		DashingSkillState.StartCoroutine = StartCoroutineWrapper;
	}

	private void StartCoroutineWrapper(IEnumerator coroutine) {
		StartCoroutine(coroutine);
	}

	private void Update() {
		CurrentSkillState.UpdateState();
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
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.9f, groundLayer);
		return hit.collider != null;
	}

	public void StopJump() {
		if (Rb.velocity.y > 0) {
			Rb.velocity = new Vector2(Rb.velocity.x, Rb.velocity.y * 0f);
		}
	}
}
