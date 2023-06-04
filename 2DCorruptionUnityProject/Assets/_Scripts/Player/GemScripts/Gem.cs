using UnityEngine;

[CreateAssetMenu(fileName = "New Gem", menuName = "Gems/Gem")]
public class Gem : ScriptableObject
{
	public float moveSpeed;
	public float jumpForce;
	public int numberOfJumps;
	public float dashForce;
	public float dashCooldown;
	public float dashDuration;
	public float pushForce;
	public float pushDuration;
	public float pushCooldown;
}
