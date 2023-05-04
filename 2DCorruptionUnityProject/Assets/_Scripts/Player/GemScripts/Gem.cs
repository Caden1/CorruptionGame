using UnityEngine;

[CreateAssetMenu(fileName = "New Gem", menuName = "Gems/Gem")]
public class Gem : ScriptableObject
{
	public float moveSpeed;
	public float jumpForce;
	public float dashForce;
	public float dashDuration;
	public int numberOfJumps;
	public float meleeDamage;
	public float rangedDamage;
}
