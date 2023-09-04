using UnityEngine;

[CreateAssetMenu(fileName = "New Modifier Gem", menuName = "Gems/Modifier Gem")]
public class ModifierGem : ScriptableObject
{
	public string gemName;
	public float addedMoveSpeed;
	public float addedJumpForce;
	public int addedNumberOfJumps;
	public float addedDashForce;
	public float addedDashDuration;
	public float addedRightHandSkillDuration;
	public float addedRightHandSkillCooldown;
	public float addedLeftHandSkillDuration;
	public float addedLeftHandSkillCooldown;
}
