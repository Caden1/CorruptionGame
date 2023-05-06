using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkillController : MonoBehaviour
{
	public LayerMask groundLayer;

	public PlayerSkillState CurrentSkillState { get; private set; }
	public PurityCorruptionGem CurrentPurCorGemState { get; set; }
	public ElementalModifierGem CurrentElemModGemState { get; set; }
	public PlayerAnimationController animationController { get; set; }
	public IdleSkillState IdleSkillState { get; set; }
	public RunningSkillState RunningSkillState { get; set; }
	public JumpingSkillState JumpingSkillState { get; set; }
	public FallingSkillState FallingSkillState { get; set; }
	public DashingSkillState DashingSkillState { get; set; }

	public GemController GemController { get; private set; }
	public Rigidbody2D Rb { get; private set; }
	public float LastFacingDirection { get; set; } = 1;

	public bool IsIdle { get; set; } = false;
	public bool IsRunning { get; set; } = false;
	public bool IsJumping { get; set; } = false;
	public bool IsFalling { get; set; } = false;
	public bool IsDashing { get; set; } = false;

	private float dashForce;
	private float dashDuration;
	private float originalGravityScale;

	private void Awake() {
		GemController = GetComponent<GemController>();
		Rb = GetComponent<Rigidbody2D>();
		animationController = GetComponent<PlayerAnimationController>();

		IdleSkillState = new IdleSkillState(this);
		RunningSkillState = new RunningSkillState(this);
		JumpingSkillState = new JumpingSkillState(this);
		FallingSkillState = new FallingSkillState(this);
		DashingSkillState = new DashingSkillState(this);
	}

	private void FixedUpdate() {
		CurrentSkillState.FixedUpdate();
	}


	public void TransitionToState(PlayerSkillState newState,
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

	private IEnumerator StopDashAfterSeconds() {
		yield return new WaitForSeconds(dashDuration);
		Rb.gravityScale = originalGravityScale;
		IsDashing = false;
	}

	//private void Update() {
	//	switch (CurrentBaseState) {
	//		case BasePlayerState.Idle:
	//			animationController.ExecuteIdleAnim();
	//			Rb.velocity = new Vector2(0, Rb.velocity.y);
	//			break;
	//		case BasePlayerState.Running:
	//			animationController.ExecuteRunAnim();
	//			break;
	//		case BasePlayerState.Falling:
	//			animationController.ExecuteFallAnim();
	//			break;
	//		case BasePlayerState.Jumping:
	//			// Example usage:
	//			TransitionToState(jumpingState, PurityCorruptionGem.None, ElementalModifierGem.None);
	//			if (CanJump) {
	//				CanJump = false;
	//				animationController.ExecuteJumpAnim();
	//				//int remainingJumps = GemController.GetRightFootGem().numberOfJumps;
	//				float jumpForce = GemController.GetRightFootGem().jumpForce;
	//				Rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
	//				//remainingJumps--;
	//			}
	//			switch (CurrentPurCorGemState) {
	//				case PurityCorruptionGem.None:
	//					break;
	//				case PurityCorruptionGem.Purity:
	//					break;
	//				case PurityCorruptionGem.Corruption:
	//					break;
	//			}
	//			switch (CurrentElemModGemState) {
	//				case ElementalModifierGem.Air:
	//					break;
	//				case ElementalModifierGem.Fire:
	//					break;
	//				case ElementalModifierGem.Water:
	//					break;
	//				case ElementalModifierGem.Earth:
	//					break;
	//			}
	//			break;
	//		case BasePlayerState.Dashing:
	//			if (CanDash) {
	//				CanDash = false;
	//				animationController.ExecuteDashAnim();
	//				dashForce = GemController.GetLeftFootGem().dashForce;
	//				dashDuration = GemController.GetLeftFootGem().dashDuration;
	//				originalGravityScale = Rb.gravityScale;
	//				Rb.gravityScale = 0f;
	//				Rb.velocity = new Vector2(LastFacingDirection * dashForce, 0f);
	//				StartCoroutine(StopDashAfterSeconds());
	//			}
	//			switch (CurrentPurCorGemState) {
	//				case PurityCorruptionGem.None:
	//					break;
	//				case PurityCorruptionGem.Purity:
	//					break;
	//				case PurityCorruptionGem.Corruption:
	//					break;
	//			}
	//			switch (CurrentElemModGemState) {
	//				case ElementalModifierGem.Air:
	//					break;
	//				case ElementalModifierGem.Fire:
	//					break;
	//				case ElementalModifierGem.Water:
	//					break;
	//				case ElementalModifierGem.Earth:
	//					break;
	//			}
	//			break;
	//		case BasePlayerState.RightGlove:
	//			switch (CurrentPurCorGemState) {
	//				case PurityCorruptionGem.None:
	//					break;
	//				case PurityCorruptionGem.Purity:
	//					break;
	//				case PurityCorruptionGem.Corruption:
	//					break;
	//			}
	//			switch (CurrentElemModGemState) {
	//				case ElementalModifierGem.Air:
	//					break;
	//				case ElementalModifierGem.Fire:
	//					break;
	//				case ElementalModifierGem.Water:
	//					break;
	//				case ElementalModifierGem.Earth:
	//					break;
	//			}
	//			break;
	//		case BasePlayerState.LeftGlove:
	//			switch (CurrentPurCorGemState) {
	//				case PurityCorruptionGem.None:
	//					break;
	//				case PurityCorruptionGem.Purity:
	//					break;
	//				case PurityCorruptionGem.Corruption:
	//					break;
	//			}
	//			switch (CurrentElemModGemState) {
	//				case ElementalModifierGem.Air:
	//					break;
	//				case ElementalModifierGem.Fire:
	//					break;
	//				case ElementalModifierGem.Water:
	//					break;
	//				case ElementalModifierGem.Earth:
	//					break;
	//			}
	//			break;
	//	}
	//}
}
