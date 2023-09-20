using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Global Audio Config", menuName = "Audio/Global Audio Config")]
public class GlobalAudioConfig : ScriptableObject
{
	[Header("Global Music")]
	public AudioClip pauseMenuTrack;

	[Header("Global Sounds")]
	// Pause Menu
	public AudioClip pauseMenuNavigation;
	public AudioClip pauseMenuButtonPress;

	// Player
	public AudioClip playerFootsteps;
	public AudioClip playerJump;
	public AudioClip playerRangedAttack;
	public AudioClip playerAirModSound;
}
