using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier Gem", menuName = "Gems/Modifier Gem")]
public class ModifierGem : ScriptableObject
{
	public string gemName;

	public float addedCorruptionMoveSpeed;
	public float addedCorruptionJumpForce;
	public int addedCorruptionNumberOfJumps;
	public float addedCorruptionDashForce;
	public float addedCorruptionDashDuration;
	public float addedCorruptionRightHandSkillDuration;
	public float addedCorruptionRightHandSkillCooldown;
	public float addedCorruptionLeftHandSkillDuration;
	public float addedCorruptionLeftHandSkillCooldown;

	public float addedPurityMoveSpeed;
	public float addedPurityJumpForce;
	public int addedPurityNumberOfJumps;
	public float addedPurityDashForce;
	public float addedPurityDashDuration;
	public float addedPurityRightHandSkillDuration;
	public float addedPurityRightHandSkillCooldown;
	public float addedPurityLeftHandSkillDuration;
	public float addedPurityLeftHandSkillCooldown;
}
