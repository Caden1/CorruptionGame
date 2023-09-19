using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Global Audio Config", menuName = "Audio/Global Audio Config")]
public class GlobalAudioConfig : ScriptableObject
{
	[Header("Global Music")]
	public AudioClip pauseMenuTrack;

	[Header("Global Sounds")]
	public AudioClip pauseMenuNavigation;
	public AudioClip pauseMenuButtonPress;
	public AudioClip playerFootsteps;
	public AudioClip playerRangedAttack;
	public AudioClip playerAirRangedAttack;
}
