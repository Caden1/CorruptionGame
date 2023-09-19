using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	[Header("Config (Scriptable Object)")]
	public GlobalAudioConfig activeConfig;

	[Header("Global Music Audio Source")]
	public AudioSource musicSource;

	[Header("Global Sounds Audio Source")]
	public AudioSource soundSource;

	private void Awake() {
		// Singleton pattern
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
			return;
		}
	}

	public void PlayPauseMenuTrack(float volume = 1f) {
		musicSource.clip = activeConfig.pauseMenuTrack;
		PlayTrackHelper(volume);
	}

	private void PlayTrackHelper(float volume) {
		musicSource.loop = true;
		musicSource.volume = volume;
		musicSource.Play();
	}

	public void StopCurrentTrack() {
		musicSource.Stop();
	}

	public void PauseCurrentTrack() {
		musicSource.Pause();
	}

	public void UnpauseCurrentTrack() {
		musicSource.Play();
	}

	public void PlayPauseMenuNavigationSound(float volume = 1f) {
		soundSource.PlayOneShot(activeConfig.pauseMenuNavigation, volume);
	}

	public void PlayPauseMenuButtonPressSound(float volume = 1f) {
		soundSource.PlayOneShot(activeConfig.pauseMenuButtonPress, volume);
	}

	public void PlayPlayerFootstepsSound(float volume = 1f) {
		soundSource.clip = activeConfig.playerFootsteps;
		soundSource.pitch = 0.9f;
		PlayAndLoopSound(volume);
	}

	public void StopCurrentLoopingSound() {
		soundSource.Stop();
	}

	public void PlayPlayerRangedAttackSound(float volume = 1f) {
		soundSource.PlayOneShot(activeConfig.playerRangedAttack, volume);
	}

	public void PlayPlayerAirRangedAttackSound(float volume = 1f) {
		soundSource.PlayOneShot(activeConfig.playerAirRangedAttack, volume);
	}

	private void PlayAndLoopSound(float volume) {
		soundSource.loop = true;
		soundSource.volume = volume;
		soundSource.Play();
	}
}
