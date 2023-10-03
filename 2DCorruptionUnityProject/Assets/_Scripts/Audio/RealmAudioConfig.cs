using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Realm Audio Config", menuName = "Audio/Realm Audio Config")]
public class RealmAudioConfig : ScriptableObject
{
	[Header("Realm Music")]
	public AudioClip mainTrack;
	public AudioClip bossFightTrack;

	[Header("Realm Sounds")]
	public AudioClip meleeEnemyFootsteps;
	public AudioClip meleeEnemyTakeDamage;
	public AudioClip meleeEnemyAttack;
	public AudioClip meleeEnemyDying;
}