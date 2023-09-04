using UnityEngine;

[CreateAssetMenu(fileName = "New Base Gem", menuName = "Gems/Base Gem")]
public class BaseGem : ScriptableObject
{
	public string gemName;
	public float moveSpeed;
	public float jumpForce;
	public int numberOfJumps;
	public float dashForce;
	public float dashDuration;
	public float rightHandSkillDuration;
	public float rightHandSkillCooldown;
	public float leftHandSkillDuration;
	public float leftHandSkillCooldown;
}
